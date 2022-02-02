using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LccApiNet.DartLibGenerator
{
    public class CategoriesGenerator
    {
        public static void Generate(string projectPath, Dictionary<Type, string> dartModelTypes)
        {
            List<string> categoriesAbstraction = LocateCategoriesAbstraction(new DirectoryInfo(Path.Combine(projectPath, @"Categories\Abstraction")));
            Dictionary<Type, string> categoriesAbstractionTypes = GetCategoriesAbstractionTypes(projectPath, categoriesAbstraction);

            string outputDir = Path.Combine(Environment.CurrentDirectory, "output");
            Dictionary<Type, string> dartCategoriesAbstractionTypes = new Dictionary<Type, string>();
            foreach (KeyValuePair<Type, string> categoryAbstractionType in categoriesAbstractionTypes) {
                string[] splittedPath = categoryAbstractionType.Value.Split("\\");
                string convertedPath = outputDir;
                foreach (string folderName in splittedPath.Take(splittedPath.Length - 1)) {
                    convertedPath = Path.Combine(convertedPath, Utilities.CamelCaseToSnakeCase(folderName));
                    if (!Directory.Exists(convertedPath)) {
                        Directory.CreateDirectory(convertedPath);
                    }
                }

                convertedPath = Path.Combine(convertedPath, Utilities.CamelCaseToSnakeCase(Path.GetFileNameWithoutExtension(splittedPath[^1])));
                dartCategoriesAbstractionTypes.Add(categoryAbstractionType.Key, $"{convertedPath}.dart");
                File.Create($"{convertedPath}.dart").Close();
            }

            foreach (KeyValuePair<Type, string> categoryAbstractionType in categoriesAbstractionTypes) {
                if (!categoryAbstractionType.Key.IsInterface) {
                    throw new Exception("Abstraction must contain only interfaces");
                }

                FillCategory_Abstraction(
                    dartModelTypes,
                    Path.Combine(projectPath, categoryAbstractionType.Value), 
                    dartCategoriesAbstractionTypes[categoryAbstractionType.Key], 
                    outputDir, 
                    categoryAbstractionType.Key);
            }
        }

        private static List<string> LocateCategoriesAbstraction(DirectoryInfo info)
        {
            List<string> result = new List<string>();
            foreach (FileInfo file in info.EnumerateFiles()) {
                result.Add(file.FullName);
            }

            return result;
        }

        private static Dictionary<Type, string> GetCategoriesAbstractionTypes(string projectPath, List<string> model)
        {
            Dictionary<Type, string> categoriesTypes = new Dictionary<Type, string>();
            foreach (string path in model) {
                string relativePath = Path.GetRelativePath(projectPath, path);
                string typeName = Path.GetFileNameWithoutExtension(Path.Combine("LccApiNet", relativePath).Replace("\\", "."));
                Type? t = Utilities.GetTypeByName(typeName);
                if (t == null) {
                    throw new Exception($"There's no such type in the library that is presented in the model folder as '{typeName}'");
                }

                categoriesTypes.Add(t, relativePath);
            }

            return categoriesTypes;
        }

        private static void FillCategory_Abstraction(Dictionary<Type, string> allModelTypes, string csFilePath, string dartFilePath, string outputDirPath, Type interfaceType)
        {
            CsInterfaceInfo interfaceInfo = CsInterfaceInfo.FromCsFile(csFilePath, interfaceType);
            StringBuilder abstractionFileContentBuilder = new StringBuilder();

            List<string> imports = new List<string>();
            for (int i = 0; i < interfaceInfo.Methods.Length; i++) {
                CsMethodInfo csMethod = interfaceInfo.Methods[i];
                if (!typeof(Task).IsAssignableFrom(csMethod.Method.ReturnType)) {
                    throw new Exception("Not async methods are not supported.");
                }

                if (csMethod.Method.ReturnType.GenericTypeArguments.Length > 0) {
                    imports.AddRange(
                        Utilities.ConvertCsTypeToDartImportType(csMethod.Method.ReturnType.GenericTypeArguments[0])
                        .Select(t => $"import 'package:lcc_api_dart/src/{Path.GetRelativePath(outputDirPath, allModelTypes[t]).Replace("\\", "/")}';"));
                }

                foreach (ParameterInfo param in csMethod.Method.GetParameters()) {
                    if (param.ParameterType == typeof(CancellationToken)) {
                        continue;
                    }

                    imports.AddRange(
                        Utilities.ConvertCsTypeToDartImportType(param.ParameterType)
                        .Select(t => $"import 'package:lcc_api_dart/src/{Path.GetRelativePath(outputDirPath, allModelTypes[t]).Replace("\\", "/")}';"));
                }
            }

            imports = imports.Distinct().ToList();
            abstractionFileContentBuilder.AppendLine(string.Join("\r\n", imports));
            abstractionFileContentBuilder.AppendLine();
            abstractionFileContentBuilder.AppendLine($"{string.Join("\r\n", interfaceInfo.CommentLines)}");
            abstractionFileContentBuilder.AppendLine($"abstract class {interfaceType.Name}");
            abstractionFileContentBuilder.AppendLine("{");

            List<string> dartMethods = new List<string>();
            for (int i = 0; i < interfaceInfo.Methods.Length; i++) {
                CsMethodInfo csMethod = interfaceInfo.Methods[i];
                abstractionFileContentBuilder.AppendLine($"\t{string.Join("\r\n\t", csMethod.CommentLines)}");

                string? returnType = null;
                StringBuilder methodStringBuilder = new StringBuilder();
                NullabilityInfoContext nullabilityInfoContext = new NullabilityInfoContext();
                methodStringBuilder.Append("\tFuture");
                if (csMethod.Method.ReturnType.GenericTypeArguments.Length > 0) {
                    NullabilityInfo returnParameterInfo = nullabilityInfoContext.Create(csMethod.Method.ReturnParameter).GenericTypeArguments[0];
                    returnType = Utilities.CsTypeToDartTypeConverter(csMethod.Method.ReturnType.GenericTypeArguments[0], returnParameterInfo);
                }

                if (returnType == null) {
                    methodStringBuilder.Append(" ");
                } else {
                    methodStringBuilder.Append($"<{returnType}> ");
                }

                methodStringBuilder.Append($"{Utilities.CamelCaseToLowerCamelCase(csMethod.Method.Name.Take(csMethod.Method.Name.Length - 5))}(");

                bool defaultParametersArrived = false;
                ParameterInfo[] parameters = csMethod.Method.GetParameters();
                for (int j = 0; j < parameters.Length - 1; j++) {
                    ParameterInfo param = parameters[j];
                    if (param.ParameterType == typeof(CancellationToken)) {
                        continue;
                    }

                    NullabilityInfo nullabilityInfo = nullabilityInfoContext.Create(param);
                    string dartParamType = $"{Utilities.CsTypeToDartTypeConverter(param.ParameterType, nullabilityInfo)}";
                    if (param.HasDefaultValue) {
                        if (!defaultParametersArrived) {
                            methodStringBuilder.Append("{");
                        }

                        defaultParametersArrived = true;
                        if (param.DefaultValue == null) {
                            methodStringBuilder.Append($"{dartParamType} {param.Name}");
                        } else {
                            methodStringBuilder.Append($"{dartParamType} {param.Name} = {param.DefaultValue}");
                        }
                    } else {
                        methodStringBuilder.Append($"{dartParamType} {param.Name}");
                    }

                    if (j != parameters.Length - 2) {
                        methodStringBuilder.Append(", ");
                    }
                }

                if (defaultParametersArrived) {
                    methodStringBuilder.Append("}");
                }

                methodStringBuilder.AppendLine(");");
                dartMethods.Add(methodStringBuilder.ToString());

                abstractionFileContentBuilder.Append(methodStringBuilder);
                if (i != interfaceInfo.Methods.Length - 1) {
                    abstractionFileContentBuilder.AppendLine();
                }
            }

            abstractionFileContentBuilder.AppendLine("}");
            using (StreamWriter writer = new StreamWriter(new FileStream(dartFilePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))) {
                writer.Write(abstractionFileContentBuilder);
            }

            abstractionFileContentBuilder.Clear();


            string implName = interfaceType.Name.Substring(1);
            StringBuilder implFileContentBuilder = new StringBuilder(
                "import 'package:lcc_api_dart/src/i_lcc_api.dart';\r\n"
            );

            implFileContentBuilder.AppendLine(string.Join("\r\n", imports));
            implFileContentBuilder.AppendLine();
            implFileContentBuilder.AppendLine($"{string.Join("\r\n", interfaceInfo.CommentLines)}");
            implFileContentBuilder.AppendLine($"class {implName} implements {interfaceType.Name}");
            implFileContentBuilder.AppendLine("{");
            implFileContentBuilder.AppendLine("\tILccApi _api;");
            implFileContentBuilder.AppendLine($"\t{implName}(this._api);");
            implFileContentBuilder.AppendLine();

            for (int i = 0; i < dartMethods.Count; i++) {
                implFileContentBuilder.AppendLine("\t@override");
                implFileContentBuilder.AppendLine($"{dartMethods[i].Replace("\r\n", "").Replace(";", "")} async {{");
                implFileContentBuilder.AppendLine($"\t\tthrow UnimplementedError();");
                implFileContentBuilder.AppendLine("\t}");

                if (i != dartMethods.Count - 1) {
                    implFileContentBuilder.AppendLine();
                }
            }
            
            implFileContentBuilder.AppendLine("}");

            FileInfo info = new FileInfo(dartFilePath);
            DirectoryInfo categoriesDir = info.Directory!.Parent!;
            string implPath = Path.Combine(categoriesDir.FullName, info.Name.Substring(2));

            using (StreamWriter writer = new StreamWriter(new FileStream(implPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))) {
                writer.Write(implFileContentBuilder);
            }
        }

        private class CsInterfaceInfo
        {
            public string[] CommentLines;
            public CsMethodInfo[] Methods;

            private CsInterfaceInfo(string[] commentLines, CsMethodInfo[] methods)
            {
                CommentLines = commentLines;
                Methods = methods;
            }

            public static CsInterfaceInfo FromCsFile(string csFilePath, Type classType)
            {
                string csFileContent;
                using (StreamReader reader = new StreamReader(new FileStream(csFilePath, FileMode.Open))) {
                    csFileContent = reader.ReadToEnd();
                }

                int csFilePos = 0;
                int classDefStrIndex = -1;
                int classCommentStartStrIndex = -1;
                string[] splittedFileContent = csFileContent.Split("\r\n");
                for (; csFilePos < splittedFileContent.Length; csFilePos++) {
                    string line = splittedFileContent[csFilePos];
                    if (line.Contains($"interface {classType.Name}")) {
                        classDefStrIndex = csFilePos;
                        if (splittedFileContent[csFilePos - 1].Contains("</summary>")) {
                            for (int j = classDefStrIndex; j >= 0; j--) {
                                if (splittedFileContent[j].Contains("<summary>")) {
                                    classCommentStartStrIndex = j;
                                }
                            }
                        }
                        break;
                    }
                }

                if (classDefStrIndex == -1) {
                    throw new Exception("Class not found in file");
                }

                string[] classCommentLines = new string[0];
                if (classCommentStartStrIndex != -1) {
                    classCommentLines = ParseCommentLines(splittedFileContent, classCommentStartStrIndex, classDefStrIndex - 1);
                }

                List<CsMethodInfo> methods = new List<CsMethodInfo>();
                foreach (MethodInfo method in classType.GetMethods()) {
                    Match? methodMatch = null;
                    for (; csFilePos < splittedFileContent.Length; csFilePos++) {
                        methodMatch = Regex.Match(splittedFileContent[csFilePos], $@"(.+)\s+{method.Name}\(.*CancellationToken token = default\);");
                        if (methodMatch.Success) {
                            break;
                        }
                    }

                    if (csFilePos == splittedFileContent.Length) {
                        throw new Exception("Method not found in file");
                    }

                    int methodCommentStartStrIndex = -1;
                    int methodCommentEndStrIndex = -1;
                    for (int i = csFilePos - 1; i > classDefStrIndex; i--) {
                        if (splittedFileContent[i].Contains('[') || splittedFileContent[i].Contains("/// <") && !splittedFileContent[i].Contains("</summary>")) {
                            continue;
                        } else {
                            if (splittedFileContent[i].Contains("</summary>")) {
                                methodCommentEndStrIndex = i;
                                for (int j = i; j >= 0; j--) {
                                    if (splittedFileContent[j].Contains("<summary>")) {
                                        methodCommentStartStrIndex = j;
                                        break;
                                    }
                                }
                            } else {
                                break;
                            }
                        }
                    }

                    string[] methodCommentLines = new string[0];
                    if (methodCommentStartStrIndex != -1) {
                        methodCommentLines = ParseCommentLines(splittedFileContent, methodCommentStartStrIndex, methodCommentEndStrIndex);
                    }

                    methods.Add(new CsMethodInfo(methodCommentLines, methodMatch!.Groups.Count == 2 && methodMatch!.Groups[1].Value.Contains("?"), method));
                }

                return new CsInterfaceInfo(classCommentLines, methods.ToArray());
            }

            private static string[] ParseCommentLines(string[] splittedFileContent, int commentStartStrIndex, int commentEndStrIndex)
            {
                List<string> commentLines = new List<string>();
                for (int i = commentStartStrIndex + 1; i < commentEndStrIndex; i++) {
                    string commentLine = splittedFileContent[i];
                    commentLine = commentLine.Substring(commentLine.IndexOf("///"));
                    commentLines.Add(commentLine);
                }

                return commentLines.ToArray();
            }
        }

        private class CsMethodInfo
        {
            public string[] CommentLines;
            public bool ReturnTypeNullable;
            public MethodInfo Method;

            public CsMethodInfo(string[] commentLines, bool returnTypeNullable, MethodInfo method)
            {
                CommentLines = commentLines;
                ReturnTypeNullable = returnTypeNullable;
                Method = method;
            }
        }
    }
}
