﻿using Ardalis.SmartEnum;

using LccApiNet.LibraryGenerator.Model;
using LccApiNet.LibraryGenerator.SchemeModel;
using LccApiNet.LibraryGenerator.Utilities;

using Microsoft.CSharp;

using Newtonsoft.Json;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;

namespace LccApiNet.LibraryGenerator.Core
{
    public class ModelGenerator
    {
        public static LocalModel GenerateLocalModel(string outputDirectory, ApiScheme scheme, List<LocalEntityDeclaration> modelDeclarations)
        {
            ConsoleUtils.ShowInfo("Building model code graphs...");
            Dictionary<string, LocalModelEntity> graphs = BuildModelGraphs(scheme, modelDeclarations);
            ConsoleUtils.ShowInfo("Code graphs are built");

            foreach (KeyValuePair<string, LocalModelEntity> graph in graphs) {
                string outputPath = Path.Combine(outputDirectory, graph.Key);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(outputPath, false), "    ");
                new CSharpCodeProvider().GenerateCodeFromCompileUnit(graph.Value.Implementation, writer, new CodeGeneratorOptions());
                writer.Close();
            }

            ConsoleUtils.ShowInfo("Generating local model info...");
            LocalModel model = new LocalModel();
            foreach (LocalEntityDeclaration declaration in modelDeclarations) {
                foreach (LocalModelEntity entity in graphs.Values) {
                    if (entity.Declaration == declaration) {
                        if (declaration.Kind == ApiEntityKind.Parameters) {
                            model.Parameters.Add(entity);
                        } else if (declaration.Kind == ApiEntityKind.Response) {
                            model.Responses.Add(entity);
                        }

                        break;
                    }
                }   
            }
            ConsoleUtils.ShowInfo("Local model info is generated");

            return model;
        }

        private static Dictionary<string, LocalModelEntity> BuildModelGraphs(ApiScheme scheme, List<LocalEntityDeclaration> modelDeclarations)
        {
            Dictionary<string, LocalModelEntity> graphs = new();

            LocalEntityDeclaration? apiResponse = modelDeclarations.FirstOrDefault(d => d.Name == Config.RESPONSE_BASE_CLASS_NAME);
            if (apiResponse == null) {
                throw new GeneratorException($"Class named {Config.RESPONSE_BASE_CLASS_NAME} not found in API model.");
            }

            foreach (ApiEntity entity in scheme.Model.Entities) {
                LocalEntityDeclaration declaration = modelDeclarations[entity.Id - 1];
                string filePath = Path.Combine(Config.PROJECT_NAME, Config.MODEL_FOLDER_NAME, declaration.LocalPath);

                ConsoleUtils.ShowInfo($"Building graph for model entity - {declaration.Name} located at {declaration.LocalPath} that is {declaration.Kind.Name} entity...");
                graphs.Add(filePath, BuildEntityGraph(entity, declaration, modelDeclarations, apiResponse.Namespace));
                ConsoleUtils.ShowInfo($"Entity graph for {declaration.Name} is built");
            }

            foreach (ApiEnum entity in scheme.Model.Enums) {
                LocalEntityDeclaration declaration = modelDeclarations[entity.Id - 1];
               
                ConsoleUtils.ShowInfo($"Building graph for model enum - {declaration.Name} located at {declaration.LocalPath}...");
                graphs.Add(Path.Combine(Config.PROJECT_NAME, Config.MODEL_FOLDER_NAME, declaration.LocalPath), BuildEnumGraph(entity, declaration));
                ConsoleUtils.ShowInfo($"Enum graph for {declaration.Name} is built");
            }

            return graphs;
        } 


        private static LocalModelEntity BuildEntityGraph(ApiEntity entity, LocalEntityDeclaration declaration, List<LocalEntityDeclaration> allDeclarations, string apiResponseReference)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace entityNamespace = new CodeNamespace(declaration.Namespace);
            compileUnit.Namespaces.Add(entityNamespace);

            CodeTypeDeclaration entityClass = new CodeTypeDeclaration(declaration.Name);
            entityClass.Comments.Add(entity.Docs.ToCSharpDoc());
            entityNamespace.Types.Add(entityClass);

            if (declaration.Kind == ApiEntityKind.Response) {
                entityClass.BaseTypes.Add(new CodeTypeReference($"{apiResponseReference}.{Config.RESPONSE_BASE_CLASS_NAME}"));
            }

            CodeConstructor? entityConstructor = null;
            if (declaration.Kind == ApiEntityKind.Parameters) {
                entityConstructor = new CodeConstructor();
                entityConstructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            }

            List<LocalEntityProperty> localProperties = new List<LocalEntityProperty>();
            foreach (ApiEntityProperty property in entity.Properties.OrderBy(p => p.InitialValue == null)) {
                string propertyName = property.Name.CaseTransform(Case.CamelCase, Case.PascalCase);
                CodeMemberField entityProperty = new CodeMemberField {
                    Attributes = MemberAttributes.Public | MemberAttributes.Final,
                    Name = propertyName,
                };

                entityProperty.Type = property.Type.ToTypeReference(allDeclarations);
                entityProperty.Comments.Add(property.Docs.ToCSharpDoc());
                entityProperty.CustomAttributes.Add(BuildJsonPropertyAttributeDeclaration(property.JsonName));

                string namePostfix = " { get; set; }//";                
                if (property.InitialValue == null) {
                    if (!property.Type.Nullable) {
                        namePostfix = " { get; set; } = default!";
                    }
                } else {
                    entityProperty.InitExpression = new CodePrimitiveExpression(property.InitialValue);
                }

                entityProperty.Name += namePostfix;
                entityClass.Members.Add(entityProperty);
                localProperties.Add(new LocalEntityProperty(entityProperty.Type, property));

                if (entityConstructor == null) {
                    continue;
                }
                
                if (property.InitialValue == null) {
                    entityConstructor.Parameters.Add(new CodeParameterDeclarationExpression(entityProperty.Type, property.Name));
                } else {
                    CodeParameterDeclarationExpression parameterDeclaration = new CodeParameterDeclarationExpression(entityProperty.Type, property.Name.CaseTransform(Case.PascalCase, Case.CamelCase));
                    parameterDeclaration.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference(typeof(OptionalAttribute))));
                    if (property.InitialValue != null) {
                        parameterDeclaration.CustomAttributes.Add(BuildDefaultValueAttributeDeclaration(property.InitialValue));
                    }

                    entityConstructor.Parameters.Add(parameterDeclaration);
                }

                CodeAssignStatement statement = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), propertyName), new CodeArgumentReferenceExpression(property.Name));
                entityConstructor.Statements.Add(statement);
            }

            if (entityConstructor != null) { 
                entityClass.Members.Add(entityConstructor);
            }

            LocalModelEntity localEntity = new LocalModelEntity(declaration, compileUnit, localProperties);
            return localEntity;
        }

        private static LocalModelEntity BuildEnumGraph(ApiEnum entity, LocalEntityDeclaration declaration)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            CodeNamespace entityNamespace = new CodeNamespace(declaration.Namespace);
            compileUnit.Namespaces.Add(entityNamespace);

            CodeTypeDeclaration entityEnum = new CodeTypeDeclaration(declaration.Name);
            entityEnum.Comments.Add(entity.Docs.ToCSharpDoc());
            entityNamespace.Types.Add(entityEnum);

            CodeTypeReference smartEnum = new CodeTypeReference(typeof(SmartEnum<>));
            smartEnum.TypeArguments.Add(new CodeTypeReference(declaration.Name));
            entityEnum.BaseTypes.Add(smartEnum);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "name"));
            constructor.Parameters.Add(new CodeParameterDeclarationExpression(typeof(int), "value"));
            constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("name"));
            constructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("value"));

            entityEnum.Members.Add(constructor);

            for (int i = 0; i < entity.Members.Length; i++) {
                ApiEnumMember member = entity.Members[i];
                CodeMemberField entityMember = new CodeMemberField {
                    Name = member.Name,
                    Attributes = MemberAttributes.Public | MemberAttributes.Static,
                    Type = new CodeTypeReference(declaration.Name)
                };

                entityMember.Comments.Add(member.Docs.ToCSharpDoc());
                entityMember.InitExpression = new CodeObjectCreateExpression(declaration.Name, new CodePrimitiveExpression(member.Value), new CodePrimitiveExpression(i + 1));
                entityEnum.Members.Add(entityMember);
            }

            LocalModelEntity localEntity = new LocalModelEntity(declaration, compileUnit, new List<LocalEntityProperty>());
            return localEntity;
        }

        private static CodeAttributeDeclaration BuildJsonPropertyAttributeDeclaration(string jsonKey)
        {
            CodeTypeReference propertyAttributeReference = new CodeTypeReference(typeof(JsonPropertyAttribute));
            CodeAttributeArgument jsonKeyArgument = new CodeAttributeArgument(new CodePrimitiveExpression(jsonKey));
            return new CodeAttributeDeclaration(propertyAttributeReference, jsonKeyArgument);
        }

        public static CodeAttributeDeclaration BuildDefaultValueAttributeDeclaration(object defaultValue)
        {
            CodeTypeReference propertyAttributeReference = new CodeTypeReference(typeof(DefaultParameterValueAttribute));
            CodeAttributeArgument jsonKeyArgument = new CodeAttributeArgument(new CodePrimitiveExpression(defaultValue));
            return new CodeAttributeDeclaration(propertyAttributeReference, jsonKeyArgument);
        }
    }
}