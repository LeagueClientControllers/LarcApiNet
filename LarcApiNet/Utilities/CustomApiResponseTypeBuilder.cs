using LarcApiNet.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LarcApiNet.Utilities
{
    public class CustomApiResponseTypeBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="additionalPropertyName"></param>
        /// <returns></returns>
        public static Type GetCustomApiResponseType<TProperty>(string additionalPropertyName)
        {
            TypeBuilder tb = GetTypeBuilder(additionalPropertyName);
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            CreateProperty(tb, additionalPropertyName, typeof(TProperty));

            Type objectType = tb.CreateType()!;
            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string additionalPropertyName)
        {
            var typeSignature = $"ApiResponseWith{string.Concat(additionalPropertyName[0].ToString().ToUpper(), additionalPropertyName.AsSpan(1))}";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature,
                    TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout,
                    typeof(ApiResponse));

            return tb;
        }   
        
        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(string.Concat(propertyName[0].ToString().ToUpper(), propertyName.AsSpan(1)), PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
            propertyBuilder.SetCustomAttribute(BuildCustomAttribute(new JsonPropertyAttribute(propertyName)));
        }

        private static CustomAttributeBuilder BuildCustomAttribute(Attribute attribute)
        {
            Type type = attribute.GetType();
            var constructor = type.GetConstructor(Type.EmptyTypes);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanWrite).ToArray();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            var propertyValues = from p in properties
                                 select p.GetValue(attribute, null);
            var fieldValues = from f in fields
                              select f.GetValue(attribute);

            return new CustomAttributeBuilder(constructor!,
                                             Type.EmptyTypes,
                                             properties,
                                             propertyValues.ToArray(),
                                             fields,
                                             fieldValues.ToArray());
        }
    }
}
