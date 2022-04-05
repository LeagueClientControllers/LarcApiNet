using ICSharpCode.NRefactory.CSharp;

using LccApiNet.LibraryGenerator.Model;
using LccApiNet.LibraryGenerator.Utilities;

namespace LccApiNet.LibraryGenerator.Core
{
    public class CoreClassModifier
    {
        public static void AppendCategories(string outputPath, string libraryPath, List<LocalCategory> categories)
        {
            if (categories.Count == 0) {
                return;
            }

            Console.WriteLine();
            ConsoleUtils.ShowInfo("----------------------------- Modifying core class ---------------------------------");

            string coreAbstractionPath = Path.Combine(libraryPath, $"I{Config.CORE_LIBRARY_TYPE_NAME}.cs");
            string coreImplementationPath = Path.Combine(libraryPath, $"{Config.CORE_LIBRARY_TYPE_NAME}.cs");

            SyntaxTree coreAbstraction = null!;
            string? coreAbstractionContent = null!;
            using (StreamReader reader = new StreamReader(new FileStream(coreAbstractionPath, FileMode.Open, FileAccess.Read))) {
                coreAbstractionContent = reader.ReadToEnd();
                coreAbstraction = Generator.CodeParser.Parse(coreAbstractionContent);
            }

            SyntaxTree coreImplementation = null!;
            string? coreImplementationContent = null!;
            using (StreamReader reader = new StreamReader(new FileStream(coreImplementationPath, FileMode.Open, FileAccess.Read))) {
                coreImplementationContent = reader.ReadToEnd();
                coreImplementation = Generator.CodeParser.Parse(coreImplementationContent);
            }

            ModifyAbstraction(coreAbstractionContent, coreAbstraction, categories);
        }

        public static string ModifyAbstraction(string content, SyntaxTree syntax, List<LocalCategory> categories)
        {
            List<PropertyDeclaration> properties = syntax.ExtractTypeProperties($"I{Config.CORE_LIBRARY_TYPE_NAME}");


            return "";
        }
    }
}