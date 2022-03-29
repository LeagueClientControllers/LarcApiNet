using LccApiNet.LibraryGenerator.Model;
using LccApiNet.LibraryGenerator.SchemeModel;

using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;

namespace LccApiNet.LibraryGenerator.Utilities
{
    public static class Extensions
    {
        public static CodeCommentStatement ToCSharpDoc(this IEnumerable<JsDocumentationNode> nodes)
        {
            string comment = "<summary>\r\n ";
            foreach (JsDocumentationNode node in nodes) {
                if (node.IsReference) {
                    comment += $"<see cref=\"{node.Text}\"/>";
                } else {
                    comment += node.Text;
                }
            }

            comment += "\r\n </summary>";
            return new CodeCommentStatement(comment, true);
        }

        public static CodeTypeReference ToTypeReference(this ApiPropertyType type, List<LocalEntityDeclaration> localTypes)
        {
            CodeTypeReference typeReference;

            if (type.ReferenceId != null) {
                LocalEntityDeclaration referencedDeclaration = localTypes[(int)type.ReferenceId - 1];
                typeReference = new CodeTypeReference($"{referencedDeclaration.Namespace}.{referencedDeclaration.Name}");
            } else if (type.Primitive != null) { 
                if (type.Primitive == PrimitiveType.Number) {
                    typeReference = new CodeTypeReference(typeof(int));
                } else if (type.Primitive == PrimitiveType.Decimal) {
                    typeReference = new CodeTypeReference(typeof(decimal));
                } else if (type.Primitive == PrimitiveType.String) {
                    typeReference = new CodeTypeReference(typeof(string));
                } else if (type.Primitive == PrimitiveType.Boolean) {
                    typeReference = new CodeTypeReference(typeof(bool));
                } else if (type.Primitive == PrimitiveType.Object) {
                    typeReference = new CodeTypeReference(typeof(object));
                } else if (type.Primitive == PrimitiveType.Date) {
                    typeReference = new CodeTypeReference(typeof(DateTime));
                } else if (type.Primitive == PrimitiveType.Array) {
                    typeReference = new CodeTypeReference(typeof(Array));
                } else if (type.Primitive == PrimitiveType.Dictionary) {
                    typeReference = new CodeTypeReference("System.Collections.Generic.Dictionary");
                } else {
                    throw new ArgumentException("Primitive type is defined but is not recognizable.");
                }
            } else {
                throw new ArgumentException("Type is not defined by either primitive or the reference id");
            }

            foreach (ApiPropertyType innerType in type.GenericTypeArguments) {
                typeReference.TypeArguments.Add(innerType.ToTypeReference(localTypes));
            }

            if (type.Nullable) {
                string typeString;
                using (MemoryStream stream = new MemoryStream()) {
                    StreamWriter writer = new StreamWriter(stream);
                    CodeDomProvider.CreateProvider("CSharp").GenerateCodeFromExpression(new CodeTypeReferenceExpression(typeReference), writer, new CodeGeneratorOptions());
                    writer.Flush();
                    stream.Position = 0;

                    using (StreamReader reader = new StreamReader(stream)) {
                        typeString = reader.ReadToEnd();
                    }
                }

                typeReference = new CodeTypeReference($"{typeString}?");
            }

            return typeReference;
        }
    }
}