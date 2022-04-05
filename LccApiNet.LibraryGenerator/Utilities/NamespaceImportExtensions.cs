﻿using LccApiNet.LibraryGenerator.Core;
using System.CodeDom;

namespace LccApiNet.LibraryGenerator.Utilities
{
    public static class NamespaceImportExtensions
    {
        public static void AddImportsForEnum(this CodeNamespace @namespace)
        {
            @namespace.Imports.Add(new CodeNamespaceImport($"Ardalis.SmartEnum"));
        }

        public static void AddImportsForModelEntity(this CodeNamespace @namespace)
        {
            @namespace.Imports.Add(new CodeNamespaceImport($"System"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Collections.Generic"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Runtime.InteropServices"));
            @namespace.Imports.Add(new CodeNamespaceImport($"Newtonsoft.Json"));
            @namespace.Imports.Add(new CodeNamespaceImport($"Ardalis.SmartEnum.JsonNet"));
        }

        public static void AddImportsForCategoryAbstraction(this CodeNamespace @namespace)
        {
            @namespace.Imports.Add(new CodeNamespaceImport($"System"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Collections.Generic"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Runtime.InteropServices"));
            @namespace.Imports.Add(new CodeNamespaceImport("System.Threading.Tasks.Task"));
            @namespace.Imports.Add(new CodeNamespaceImport(ModelGenerator.MODEL_NAMESPACE));
            @namespace.Imports.Add(new CodeNamespaceImport($"{Config.PROJECT_NAME}.LibraryGenerator.Attributes"));
        }

        public static void AddImportsForCategoryImplementation(this CodeNamespace @namespace)
        {
            @namespace.Imports.Add(new CodeNamespaceImport($"System"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Collections.Generic"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Runtime.InteropServices"));
            @namespace.Imports.Add(new CodeNamespaceImport($"System.Threading.Tasks.Task"));
            @namespace.Imports.Add(new CodeNamespaceImport(Config.PROJECT_NAME));
            @namespace.Imports.Add(new CodeNamespaceImport(CategoriesGenerator.CATEGORIES_ABSTRACTION_NAMESPACE));
            @namespace.Imports.Add(new CodeNamespaceImport(ModelGenerator.MODEL_NAMESPACE));
        }
    }
}