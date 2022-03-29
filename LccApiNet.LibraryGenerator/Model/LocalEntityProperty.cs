using LccApiNet.LibraryGenerator.SchemeModel;

using System.CodeDom;

namespace LccApiNet.LibraryGenerator.Model
{
    public class LocalEntityProperty
    {
        public bool   Nullable           { get; set; } 
        public object InitialValue       { get; set; }
        public string SchemePropertyName { get; set; }
        public CodeTypeReference Type    { get; set; }

        public LocalEntityProperty(CodeTypeReference reference, ApiEntityProperty property)
        {
            SchemePropertyName = property.Name;
            Nullable = property.Type.Nullable;
            InitialValue = property.InitialValue;
            Type = reference;
        }
    }
}