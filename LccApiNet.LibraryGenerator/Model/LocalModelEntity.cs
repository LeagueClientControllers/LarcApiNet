using System.CodeDom;

namespace LccApiNet.LibraryGenerator.Model
{
    public class LocalModelEntity
    {
        public LocalEntityDeclaration Declaration { get; set; }
        public CodeCompileUnit Implementation { get; set; }
        public List<LocalEntityProperty> Properties { get; set; }

        public LocalModelEntity(LocalEntityDeclaration declaration, CodeCompileUnit implementation, List<LocalEntityProperty> properties)
        {
            Declaration = declaration;
            Implementation = implementation;
            Properties = properties;
        }
    }
}