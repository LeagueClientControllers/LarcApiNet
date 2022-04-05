using LccApiNet.Model;

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LccApiNet.DartLibGenerator
{
    public class ModelsGenerator
    {
        public static Dictionary<Type, string> Generate(string projectPath)
        {
            List<string> model = LocateModel(new DirectoryInfo(Path.Combine(projectPath, "Model")));
            Dictionary<Type, string> modelTypes = GetModelTypes(projectPath, model);

            string outputDir = Path.Combine(Environment.CurrentDirectory, "output");
            Dictionary<Type, string> dartModelTypes = new Dictionary<Type, string>();
            foreach (KeyValuePair<Type, string> modelType in modelTypes) {
                string[] splittedPath = modelType.Value.Split("\\");
                string convertedPath = outputDir;
                foreach (string folderName in splittedPath.Take(splittedPath.Length - 1)) {
                    convertedPath = Path.Combine(convertedPath, Utilities.CamelCaseToSnakeCase(folderName));
                    if (!Directory.Exists(convertedPath)) {
                        Directory.CreateDirectory(convertedPath);
                    }
                }

                convertedPath = Path.Combine(convertedPath, Utilities.CamelCaseToSnakeCase(Path.GetFileNameWithoutExtension(splittedPath[^1])));
                dartModelTypes.Add(modelType.Key, $"{convertedPath}.dart");
                File.Create($"{convertedPath}.dart").Close();
            }

            foreach (KeyValuePair<Type, string> modelType in modelTypes) {
                if (modelType.Key.IsClass) {
                    if (modelType.Key.BaseType == typeof(object)) {
                        FillModel_Class(dartModelTypes, Path.Combine(projectPath, modelType.Value), $"{dartModelTypes[modelType.Key]}", outputDir, modelType.Key);
                        Console.WriteLine($"{modelType.Key.Name}: Object");
                    } else if (modelType.Key.BaseType!.Name == "SmartEnum`1") {
                        FillModel_SmartEnum(Path.Combine(projectPath, modelType.Value), $"{dartModelTypes[modelType.Key]}", outputDir, modelType.Key);
                        Console.WriteLine($"{modelType.Key.Name}: Enum");
                    } else if (modelType.Key.BaseType!.Name == "ApiResponse") {
                        FillModel_Class(dartModelTypes, Path.Combine(projectPath, modelType.Value), $"{dartModelTypes[modelType.Key]}", outputDir, modelType.Key, true);
                        Console.WriteLine($"{modelType.Key.Name}: ApiResponse");
                    }
                }
            }

            StringBuilder exportFileContentBuilder = new StringBuilder();
            foreach (KeyValuePair<Type, string> dartModelType in dartModelTypes) {
                exportFileContentBuilder.AppendLine($"export '{Path.GetRelativePath(Path.Combine(outputDir, "model"), dartModelType.Value).Replace("\\", "/")}';");
            }

            using (StreamWriter writer = new StreamWriter(new FileStream(Path.Combine(outputDir, "model", "lcc_api_dart_model.dart"), FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))) {
                writer.Write(exportFileContentBuilder);
            }

            return dartModelTypes;
        }

        private static List<string> LocateModel(DirectoryInfo info)
        {
            List<string> result = new List<string>();
            foreach (DirectoryInfo dir in info.EnumerateDirectories()) {
                result.AddRange(LocateModel(dir));
            }
            
            result.AddRange(info.EnumerateFiles().Select(file => file.FullName));
            return result;
        }

        private static Dictionary<Type, string> GetModelTypes(string projectPath, List<string> model)
        {
            Dictionary<Type, string> modelTypes = new Dictionary<Type, string>();
            foreach (string path in model) {
                string relativePath = Path.GetRelativePath(projectPath, path);
                string typeName = Path.GetFileNameWithoutExtension(Path.Combine("LccApiNet", relativePath).Replace("\\", "."));
                Type? t = Utilities.GetTypeByName(typeName);
                if (t == null) {
                    throw new Exception($"There's no such type in the library that is presented in the model folder as '{typeName}'");
                }

                modelTypes.Add(t, relativePath);
            }

            return modelTypes;
        }

        private static void FillModel_Class(Dictionary<Type, string> allModelTypes, string csFilePath, string dartFilePath, string outputDirPath, Type modelType, bool isApiResponse = false)
        {
            CsClassInfo classInfo = CsClassInfo.FromCsFile(csFilePath, modelType);
            StringBuilder fileContentBuilder = new StringBuilder(
               $"import 'package:json_annotation/json_annotation.dart';\r\n"
            );

            if (isApiResponse) {
                fileContentBuilder.AppendLine("import 'package:lcc_api_dart/src/model/general/api_response.dart';");
                fileContentBuilder.AppendLine("import 'package:lcc_api_dart/src/model/general/method_error.dart';");
                fileContentBuilder.AppendLine("import 'package:lcc_api_dart/src/model/general/execution_result.dart';");
            } else {
                fileContentBuilder.AppendLine("import 'package:lcc_api_dart/src/utils/base_json_serializable.dart';");
            }

            List<string> imports = new List<string>();
            foreach (CsPropertyInfo csProperty in classInfo.Properties) {
                imports.AddRange(
                    Utilities.ConvertCsTypeToDartImportType(
                        csProperty.Property.PropertyType)
                    .Select(t => $"import 'package:lcc_api_dart/src/{Path.GetRelativePath(outputDirPath, allModelTypes[t]).Replace("\\", "/")}';"));
            }

            fileContentBuilder.AppendLine(string.Join("\r\n", imports.Distinct()));
            fileContentBuilder.AppendLine();
            fileContentBuilder.AppendLine($"part '{Path.GetFileNameWithoutExtension(dartFilePath)}.g.dart';");
            fileContentBuilder.AppendLine();
            fileContentBuilder.AppendLine($"{string.Join("\r\n", classInfo.CommentLines)}");
            fileContentBuilder.AppendLine($"@JsonSerializable()");

            if (isApiResponse) {
                fileContentBuilder.AppendLine($"class {modelType.Name} extends ApiResponse");
            } else {
                fileContentBuilder.AppendLine($"class {modelType.Name} implements BaseJsonSerializable<{modelType.Name}>");
            }

            fileContentBuilder.AppendLine("{");

            foreach (CsPropertyInfo csProperty in classInfo.Properties) {
                JsonPropertyAttribute? jsonPropertyAttr = csProperty.Property.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonPropertyAttr == null) {
                    continue;
                }

                fileContentBuilder.AppendLine($"\t{string.Join("\r\n\t", csProperty.CommentLines)}");
                if (jsonPropertyAttr.PropertyName != null) {
                    fileContentBuilder.AppendLine($"\t@JsonKey(name: \"{jsonPropertyAttr.PropertyName}\")");
                }

                if (csProperty.Nullability.ReadState == NullabilityState.Nullable) {
                    fileContentBuilder.AppendLine($"\t{Utilities.CsTypeToDartTypeConverter(csProperty.Property.PropertyType, csProperty.Nullability)} {Utilities.CamelCaseToLowerCamelCase(csProperty.Property.Name)};");
                } else {
                    fileContentBuilder.AppendLine($"\tlate {Utilities.CsTypeToDartTypeConverter(csProperty.Property.PropertyType, csProperty.Nullability)} {Utilities.CamelCaseToLowerCamelCase(csProperty.Property.Name)};");
                }

                fileContentBuilder.AppendLine();
            }

            ConstructorInfo[] constructors = modelType.GetConstructors();
            ParameterInfo[] constructorParams = constructors[0].GetParameters();
            if (constructorParams.Length > 0) {
                fileContentBuilder.Append($"\t{modelType.Name}(");
                bool defaultParametersArrived = false;
                for (int i = 0; i < constructorParams.Length; i++) {
                    ParameterInfo param = constructorParams[i]; 
                    if (param.HasDefaultValue) {
                        if (!defaultParametersArrived) {
                            fileContentBuilder.Append("{");
                        }

                        defaultParametersArrived = true;
                        if (param.DefaultValue == null) {
                            fileContentBuilder.Append($"this.{param.Name}");
                        } else {
                            fileContentBuilder.Append($"this.{param.Name} = {param.DefaultValue}");
                        }
                    } else {
                        fileContentBuilder.Append($"this.{param.Name}");
                    }

                    if (i != constructorParams.Length - 1) {
                        fileContentBuilder.Append(", ");
                    }
                }

                if (defaultParametersArrived) {
                    fileContentBuilder.AppendLine("}): super();");
                } else {
                    fileContentBuilder.AppendLine("): super();");
                }
            } else {
                fileContentBuilder.AppendLine($"\t{modelType.Name}(): super();");
            }

            fileContentBuilder.AppendLine();
            fileContentBuilder.AppendLine($"\t@override");
            fileContentBuilder.AppendLine($"\tfactory {modelType.Name}.fromJson(Map<String, dynamic> json) => _${modelType.Name}FromJson(json);");
            fileContentBuilder.AppendLine();
            fileContentBuilder.AppendLine($"\t@override");
            fileContentBuilder.AppendLine($"\tMap<String, dynamic> toJson() => _${modelType.Name}ToJson(this);");
            fileContentBuilder.AppendLine("}");

            using (StreamWriter writer = new StreamWriter(new FileStream(dartFilePath, FileMode.Open, FileAccess.Write, FileShare.Write))) {
                writer.Write(fileContentBuilder);
            }

            string str = fileContentBuilder.ToString();
            ;
        }

        private static void FillModel_SmartEnum(string csFilePath, string dartFilePath, string outputDirPath, Type modelType)
        {
            CsClassInfo classInfo = CsClassInfo.FromCsFile(csFilePath, modelType);
            StringBuilder fileContentBuilder = new StringBuilder(
               $"import 'package:json_annotation/json_annotation.dart';\r\n" +
               $"\r\n" +
               $"{string.Join("\r\n", classInfo.CommentLines)}\r\n" +
               $"enum {modelType.Name}\r\n" +
               $"{{\r\n"
            );

            for (int i = 0; i < classInfo.Fields.Length; i++) {
                CsFieldInfo csField = classInfo.Fields[i];

                fileContentBuilder.AppendLine($"\t{string.Join("\r\n\t", csField.CommentLines)}");
                fileContentBuilder.AppendLine($"\t@JsonValue(\"{csField.EnumValue}\")");
                fileContentBuilder.AppendLine($"\t{Utilities.CamelCaseToLowerCamelCase(csField.Field.Name)},");

                if (i != classInfo.Fields.Length - 1) {
                    fileContentBuilder.AppendLine();
                }
            }

            fileContentBuilder.AppendLine("}");

            using (StreamWriter writer = new StreamWriter(new FileStream(dartFilePath, FileMode.Open, FileAccess.Write, FileShare.Write))) {
                writer.Write(fileContentBuilder);
            }

        }

        private class CsClassInfo 
        {
            public string[] CommentLines;
            public CsPropertyInfo[] Properties;
            public CsFieldInfo[] Fields;

            private CsClassInfo(string[] commentLines, CsPropertyInfo[] properties, CsFieldInfo[] fields) {
                CommentLines = commentLines;
                Properties = properties;
                Fields = fields;
            }

            public static CsClassInfo FromCsFile(string csFilePath, Type classType)
            {
                string csFileContent;
                using (StreamReader reader = new StreamReader(new FileStream(csFilePath, FileMode.Open))) {
                    csFileContent = reader.ReadToEnd();
                }

                int csFilePos = 0;
                int interfaceDefStrIndex = -1;
                int interfaceCommentStartStrIndex = -1;
                string[] splittedFileContent = csFileContent.Split("\r\n");
                for (; csFilePos < splittedFileContent.Length; csFilePos++) {
                    string line = splittedFileContent[csFilePos];
                    if (line.Contains($"class {classType.Name}")) {
                        interfaceDefStrIndex = csFilePos;
                        if (splittedFileContent[csFilePos - 1].Contains("</summary>")) {
                            for (int j = interfaceDefStrIndex; j >= 0; j--) {
                                if (splittedFileContent[j].Contains("<summary>")) {
                                    interfaceCommentStartStrIndex = j;
                                }
                            }
                        }
                        break;
                    }
                }

                if (interfaceDefStrIndex == -1) {
                    throw new Exception("Class not found in file");
                }

                string[] classCommentLines = new string[0];
                if (interfaceCommentStartStrIndex != -1) {
                    classCommentLines = ParseCommentLines(splittedFileContent, interfaceCommentStartStrIndex, interfaceDefStrIndex - 1);
                }

                int oldCsFilePos = csFilePos;
                List<CsPropertyInfo> properties = new List<CsPropertyInfo>();
                foreach (PropertyInfo property in classType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)) {
                    for (; csFilePos < splittedFileContent.Length; csFilePos++) {
                        Match match = Regex.Match(splittedFileContent[csFilePos], $@"public\s+.+\s+{property.Name}");
                        if (match.Success) {
                            break;
                        }
                    }

                    if (csFilePos == splittedFileContent.Length) {
                        throw new Exception("Property not found in file");
                    }

                    int propertyCommentStartStrIndex = -1;
                    int propertyCommentEndStrIndex = -1;
                    for (int i = csFilePos - 1; i > interfaceDefStrIndex; i--) {
                        if (splittedFileContent[i].Contains('[')) {
                            continue;
                        } else {
                            if (splittedFileContent[i].Contains("</summary>")) {
                                propertyCommentEndStrIndex = i;
                                for (int j = i; j >= 0; j--) {
                                    if (splittedFileContent[j].Contains("<summary>")) {
                                        propertyCommentStartStrIndex = j;
                                        break;
                                    }
                                }
                            } else {
                                break;
                            }
                        }
                    }

                    string[] propertyCommentLines = new string[0];
                    if (propertyCommentStartStrIndex != -1) {
                        propertyCommentLines = ParseCommentLines(splittedFileContent, propertyCommentStartStrIndex, propertyCommentEndStrIndex);
                    }

                    NullabilityInfoContext nullabilityInfoContext = new NullabilityInfoContext();
                    NullabilityInfo propertyNullabilityInfo = nullabilityInfoContext.Create(property);
                    properties.Add(new CsPropertyInfo(propertyCommentLines, propertyNullabilityInfo.ReadState == NullabilityState.Nullable, property, propertyNullabilityInfo));
                }

                csFilePos = oldCsFilePos;
                List<CsFieldInfo> fields = new List<CsFieldInfo>();
                foreach (FieldInfo field in classType.GetFields()) {
                    Match? propertyMatch = null;
                    for (; csFilePos < splittedFileContent.Length; csFilePos++) {
                        propertyMatch = Regex.Match(splittedFileContent[csFilePos], $@"{classType.Name}\s+{field.Name}\s+=\s+new\s+{classType.Name}\((.+),\s+\d+\)");
                        if (propertyMatch.Success) {
                            break;
                        }
                    }

                    if (csFilePos == splittedFileContent.Length) {
                        throw new Exception("Field not found in file");
                    }

                    int fieldCommentStartStrIndex = -1;
                    int fieldCommentEndStrIndex = -1;
                    for (int i = csFilePos - 1; i > interfaceDefStrIndex; i--) {
                        if (splittedFileContent[i].Contains('[')) {
                            continue;
                        } else {
                            if (splittedFileContent[i].Contains("</summary>")) {
                                fieldCommentEndStrIndex = i;
                                for (int j = i; j >= 0; j--) {
                                    if (splittedFileContent[j].Contains("<summary>")) {
                                        fieldCommentStartStrIndex = j;
                                        break;
                                    }
                                }
                            } else {
                                break;
                            }
                        }
                    }

                    string[] propertyCommentLines = new string[0];
                    if (fieldCommentStartStrIndex != -1) {
                        propertyCommentLines = ParseCommentLines(splittedFileContent, fieldCommentStartStrIndex, fieldCommentEndStrIndex);
                    }

                    fields.Add(new CsFieldInfo(propertyCommentLines, propertyMatch!.Groups[1].Value.Replace("\"", ""), field));
                }

                return new CsClassInfo(classCommentLines, properties.ToArray(), fields.ToArray());
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

        private class CsPropertyInfo
        {
            public string[] CommentLines;
            public bool Nullable;
            public PropertyInfo Property;
            public NullabilityInfo Nullability;

            public CsPropertyInfo(string[] commentLines, bool nullable, PropertyInfo property, NullabilityInfo nullability)
            {
                CommentLines = commentLines;
                Nullable = nullable;
                Property = property;
                Nullability = nullability;
            }
        }

        private class CsFieldInfo
        {
            public string[] CommentLines;
            public string EnumValue;
            public FieldInfo Field;

            public CsFieldInfo(string[] commentLines, string enumValue, FieldInfo field)
            {
                CommentLines = commentLines;
                EnumValue = enumValue;
                Field = field;
            }
        }
    }
}
