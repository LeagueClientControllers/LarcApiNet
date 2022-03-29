using LccApiNet.LibraryGenerator.Model;
using LccApiNet.LibraryGenerator.SchemeModel;
using LccApiNet.LibraryGenerator.Utilities;

using System.CodeDom;

namespace LccApiNet.LibraryGenerator.Core
{
    public class Generator
    {
        public static void GenerateLibrary(ApiScheme scheme)
        {
            ConsoleUtils.ShowInfo("Transforming declarations to local...");

            List<LocalEntityDeclaration> localDeclarations = new();
            foreach (ApiEntityDeclaration declaration in scheme.Model.Declarations) {
                localDeclarations.Add(new LocalEntityDeclaration(declaration));
            }

            ConsoleUtils.ShowInfo("Declarations transformed");

            LocalModel model = ModelGenerator.GenerateLocalModel(Path.Combine(Environment.CurrentDirectory, "output"), scheme, localDeclarations);
            CategoriesGenerator.GenerateLocalCategories(Path.Combine(Environment.CurrentDirectory, "output"), scheme, model);

            ;
        }
    }
}