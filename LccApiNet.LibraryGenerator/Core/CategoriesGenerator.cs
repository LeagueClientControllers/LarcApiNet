﻿
using LccApiNet.LibraryGenerator.Attributes;
using LccApiNet.LibraryGenerator.Model;
using LccApiNet.LibraryGenerator.SchemeModel;
using LccApiNet.LibraryGenerator.Utilities;

using Microsoft.CSharp;

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

namespace LccApiNet.LibraryGenerator.Core
{
    public class CategoriesGenerator
    {
        private const string CATEGORIES_NAMESPACE = $"{Config.PROJECT_NAME}.{Config.CATEGORIES_FOLDER_NAME}";
        private const string CATEGORIES_ABSTRACTION_NAMESPACE = $"{Config.PROJECT_NAME}.{Config.CATEGORIES_FOLDER_NAME}.{Config.CATEGORIES_ABSTRACTION_FOLDER_NAME}";

        public static void GenerateLocalCategories(string outputDirectory, ApiScheme scheme, LocalModel model)
        {
            ConsoleUtils.ShowInfo("Building categories abstraction code graphs...");
            Dictionary<string, CodeCompileUnit> graphs = BuildCategoriesGraphs(scheme, model);
            ConsoleUtils.ShowInfo("Abstraction graphs are built");

            foreach (KeyValuePair<string, CodeCompileUnit> graph in graphs) {
                string outputPath = Path.Combine(outputDirectory, graph.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(outputPath, false), "    ");
                new CSharpCodeProvider().GenerateCodeFromCompileUnit(graph.Value, writer, new CodeGeneratorOptions());
                writer.Close();
            }
        }

        public static Dictionary<string, CodeCompileUnit> BuildCategoriesGraphs(ApiScheme scheme, LocalModel model)
        {
            Dictionary<string, CodeCompileUnit> graphs = new();

            ApiEntityDeclaration? apiResponse = scheme.Model.Declarations.FirstOrDefault(d => d.Name == Config.RESPONSE_BASE_CLASS_NAME);
            if (apiResponse == null) {
                throw new GeneratorException($"Class named {Config.RESPONSE_BASE_CLASS_NAME} not found in API model.");
            }

            for (int i = 0; i < scheme.Categories.Length; i++) {
                ApiCategory category = scheme.Categories[i];
                string categoryName = $"{category.Name.CaseTransform(Case.CamelCase, Case.PascalCase)}Category";
                string abstractionPath = Path.Combine(Config.PROJECT_NAME, Config.CATEGORIES_FOLDER_NAME, Config.CATEGORIES_ABSTRACTION_FOLDER_NAME, $"I{categoryName}.cs");
                string implementationPath = Path.Combine(Config.PROJECT_NAME, Config.CATEGORIES_FOLDER_NAME, $"{categoryName}.cs");

                ConsoleUtils.ShowInfo($"Building abstraction graph for {category.Name} category...");
                CodeCompileUnit abstraction = BuildAbstractionGraph(category, categoryName, model, apiResponse.Id);
                graphs.Add(abstractionPath, abstraction);
                ConsoleUtils.ShowInfo($"Abstraction graph for {category.Name} category is built");

                ConsoleUtils.ShowInfo($"Building implementation graph for {category.Name} category...");
                CodeCompileUnit implementation = BuildImplementationGraph(abstraction, category, categoryName, model, apiResponse.Id);
                graphs.Add(implementationPath, implementation);
                ConsoleUtils.ShowInfo($"Abstraction graph for {category.Name} category is built");
            }

            return graphs;
        }

        public static CodeCompileUnit BuildAbstractionGraph(ApiCategory category, string categoryName, LocalModel model, int apiResponseId)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace entityNamespace = new CodeNamespace(CATEGORIES_ABSTRACTION_NAMESPACE);
            compileUnit.Namespaces.Add(entityNamespace);

            CodeTypeDeclaration categoryInterface = new CodeTypeDeclaration($"I{categoryName}");
            categoryInterface.IsInterface = true;
            categoryInterface.Comments.Add(category.Docs.ToCSharpDoc());
            entityNamespace.Types.Add(categoryInterface);

            foreach (ApiMethod method in category.Methods) {
                CodeMemberMethod categoryMethod = new CodeMemberMethod();
                categoryMethod.Comments.Add(method.Docs.ToCSharpDoc());
                categoryMethod.Name = method.Name.CaseTransform(Case.CamelCase, Case.PascalCase);
                categoryInterface.Members.Add(categoryMethod);

                if (method.ResponseId == apiResponseId) {
                    categoryMethod.ReturnType = new CodeTypeReference(typeof(Task));
                } else {
                    CodeTypeReference taskReference = new CodeTypeReference(typeof(Task<>));
                    LocalModelEntity? response = model[method.ResponseId];
                    if (response == null) {
                        throw new GeneratorException($"Response entity with id {method.ResponseId} not found in scheme.");
                    }

                    if (response.Properties.Count > 1) {
                        taskReference.TypeArguments.Add(new CodeTypeReference(response.Declaration.Namespace));
                    } else {
                        taskReference.TypeArguments.Add(response.Properties[0].Type);
                    }

                    categoryMethod.ReturnType = taskReference;
                }

                CodeParameterDeclarationExpression cancellationToken = new CodeParameterDeclarationExpression(typeof(CancellationToken), "token");
                cancellationToken.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(OptionalAttribute))));
                categoryMethod.Parameters.Add(cancellationToken);

                if (method.AccessibleFrom == MethodAccessPolicy.Controller) {
                    categoryMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(ControllerOnlyAttribute))));
                } else {
                    categoryMethod.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(DeviceOnlyAttribute))));
                }

                if (method.ParametersId == null) {
                    continue;
                }

                int i = 0;
                LocalModelEntity parameters = model[(int)method.ParametersId];
                foreach (LocalEntityProperty property in parameters.Properties) {
                    CodeParameterDeclarationExpression parameterExpression = new CodeParameterDeclarationExpression(property.Type, property.SchemePropertyName);

                    if (property.InitialValue != null) {
                        parameterExpression.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(OptionalAttribute))));
                        parameterExpression.CustomAttributes.Add(ModelGenerator.BuildDefaultValueAttributeDeclaration(property.InitialValue));
                    }

                    categoryMethod.Parameters.Insert(i, parameterExpression);
                    i++;
                }
            }

            return compileUnit;
        }

        public static CodeCompileUnit BuildImplementationGraph(CodeCompileUnit abstraction, ApiCategory category, string categoryName, LocalModel model, int apiResponseId)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace entityNamespace = new CodeNamespace(CATEGORIES_NAMESPACE);
            compileUnit.Namespaces.Add(entityNamespace);

            CodeTypeDeclaration categoryClass = new CodeTypeDeclaration(categoryName);
            categoryClass.Comments.Add(new CodeCommentStatement("<inheritdoc />", true));
            categoryClass.BaseTypes.Add(new CodeTypeReference($"{CATEGORIES_ABSTRACTION_NAMESPACE}.I{categoryName}"));
            entityNamespace.Types.Add(categoryClass);

            CodeTypeReference coreClassReference = new CodeTypeReference($"{Config.PROJECT_NAME}.{Config.CORE_LIBRARY_CLASS_TYPE_NAME}");
            CodeMemberField apiField = new CodeMemberField(coreClassReference, "_api");
            apiField.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            categoryClass.Members.Add(apiField);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(coreClassReference, "api"));
            constructor.Statements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_api"), new CodeArgumentReferenceExpression("api")));
            categoryClass.Members.Add(constructor);


            List<CodeMemberMethod> methods = new List<CodeMemberMethod>();
            foreach (CodeTypeMember member in abstraction.Namespaces[0].Types[0].Members) {
                if (member is CodeMemberMethod method) {
                    methods.Add(method);
                }
            }

            for (int i = 0; i < methods.Count; i++) {
                ApiMethod method = category.Methods[i];
                CodeMemberMethod abstractionMethod = methods[i];
                CodeMemberMethod categoryMethod = new CodeMemberMethod();
                categoryMethod.Name = abstractionMethod.Name;
                categoryMethod.ReturnType =  abstractionMethod.ReturnType;
                categoryMethod.Parameters.AddRange(abstractionMethod.Parameters);
                categoryMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                categoryMethod.Comments.Add(new CodeCommentStatement("<inheritdoc />", true));

                CodeMethodInvokeExpression executeMethodInvokation = new CodeMethodInvokeExpression();
                CodeFieldReferenceExpression apiFieldReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_api");
                executeMethodInvokation.Method = new CodeMethodReferenceExpression(apiFieldReference, "ExecuteAsync");
                executeMethodInvokation.Parameters.Add(new CodePrimitiveExpression($"/{category.Name}/{method.Name}"));

                if (method.ParametersId != null) {
                    LocalModelEntity parameters = model[(int)method.ParametersId];
                    CodeTypeReference parametersType = new CodeTypeReference($"{parameters.Declaration.Namespace}.{parameters.Declaration.Name}");
                    CodeVariableDeclarationStatement parametersCreationStatement = new CodeVariableDeclarationStatement(parametersType, "parameters");
                    executeMethodInvokation.Method.TypeArguments.Add(parametersType);
                    
                    CodeObjectCreateExpression parameterCreateExpression = new CodeObjectCreateExpression(parametersType);
                    parametersCreationStatement.InitExpression = parameterCreateExpression;
                    
                    for (int b = 0; b < categoryMethod.Parameters.Count - 1; b++) {
                        parameterCreateExpression.Parameters.Add(new CodeArgumentReferenceExpression(categoryMethod.Parameters[b].Name));
                    }

                    executeMethodInvokation.Parameters.Add(new CodeVariableReferenceExpression("parameters"));

                    categoryMethod.Statements.Add(new CodeCommentStatement("<auto-generated-safe-area> Code within tag borders shouldn't be harmful and won't cause incorrect behavior except you are very dumb.", false));
                    categoryMethod.Statements.Add(new CodeCommentStatement("TODO: Add parameters validation", false));
                    categoryMethod.Statements.Add(new CodeCommentStatement("</auto-generated-safe-area>", false));
                    categoryMethod.Statements.Add(parametersCreationStatement);
                }

                if (method.ResponseId != apiResponseId) {
                    LocalModelEntity responseEntity = model[(int)method.ResponseId];
                    CodeTypeReference parametersType = new CodeTypeReference($"{responseEntity.Declaration.Namespace}.{responseEntity.Declaration.Name}");
                    executeMethodInvokation.Method.TypeArguments.Insert(0, parametersType);

                    CodeVariableDeclarationStatement responseDeclaration = new CodeVariableDeclarationStatement(parametersType, "response");
                    responseDeclaration.InitExpression = executeMethodInvokation;
                    categoryMethod.Statements.Add(responseDeclaration);

                    categoryMethod.Statements.Add(new CodeCommentStatement("<auto-generated-safe-area> Code within tag borders shouldn't be harmful and won't cause incorrect behavior except you are very dumb.", false));
                    categoryMethod.Statements.Add(new CodeCommentStatement("TODO: Add response validation", false));
                    categoryMethod.Statements.Add(new CodeCommentStatement("</auto-generated-safe-area>", false));

                    CodeVariableReferenceExpression responseReference = new CodeVariableReferenceExpression("response");
                    if (categoryMethod.ReturnType.BaseType == parametersType.BaseType) {
                        categoryMethod.Statements.Add(new CodeMethodReturnStatement(responseReference));
                    } else {
                        CodeFieldReferenceExpression propertyReference = new CodeFieldReferenceExpression(
                            responseReference, responseEntity.Properties[0].SchemePropertyName.CaseTransform(Case.CamelCase, Case.PascalCase));

                        categoryMethod.Statements.Add(new CodeMethodReturnStatement(propertyReference));
                    }
                } else {
                    categoryMethod.Statements.Add(new CodeExpressionStatement(executeMethodInvokation));
                }

                executeMethodInvokation.Parameters.Add(new CodePrimitiveExpression(method.RequireAccessToken));
                executeMethodInvokation.Parameters.Add(new CodeArgumentReferenceExpression("token"));
                categoryClass.Members.Add(categoryMethod);
            }

            return compileUnit;
        }
    }
}