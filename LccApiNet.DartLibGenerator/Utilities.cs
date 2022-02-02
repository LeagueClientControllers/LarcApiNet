using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LccApiNet.DartLibGenerator
{
    public class Utilities
    {
        public static Type? GetTypeByName(string name)
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                var tt = assembly.GetType(name);
                if (tt != null) {
                    return tt;
                }
            }

            return null;
        }
        
        public static string CsTypeToDartTypeConverter(Type type, NullabilityInfo? info = null)
        {
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) {
                if (type.GenericTypeArguments.Length == 0) {
                    string innerType = CsTypeToDartTypeConverter(GetTypeByName(type.FullName!.Replace("[]", ""))!, info);
                    return $"List<{innerType}>{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
                } else if (type.GenericTypeArguments.Length == 1) {
                    string innerType = CsTypeToDartTypeConverter(type.GenericTypeArguments[0], info?.GenericTypeArguments[0]);
                    return $"List<{innerType}>{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
                } else if (type.GenericTypeArguments.Length == 2) {
                    string innerType1 = CsTypeToDartTypeConverter(type.GenericTypeArguments[0], info?.GenericTypeArguments[0]);
                    string innerType2 = CsTypeToDartTypeConverter(type.GenericTypeArguments[1], info?.GenericTypeArguments[1]);
                    return $"Map<{innerType1}, {innerType2}>{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
                } else {
                    throw new Exception("Too many generic arguments in type");
                }
            }

            if (type == typeof(int) || type == typeof(long)) {
                return $"int{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            } else if (type == typeof(double) || type == typeof(float) || type == typeof(decimal)) {
                return $"double{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            } else if (type == typeof(bool)) {
                return $"bool{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            } else if (type == typeof(string)) {
                return $"String{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            } else if (type == typeof(object)) {
                return $"Object{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            } else {
                return $"{type.Name}{(info != null && info.ReadState == NullabilityState.Nullable ? "?" : "")}";
            }
        }

        public static bool CsTypeDartImportRequired(Type type)
        {
            return !(type == typeof(int) || type == typeof(long)) 
                && !(type == typeof(double) || type == typeof(float) || type == typeof(decimal)) 
                && type != typeof(bool)
                && type != typeof(string) 
                && type != typeof(object);
        }

        public static List<Type> ConvertCsTypeToDartImportType(Type type)
        {
            List<Type> imports = new List<Type>();
            if (CsTypeDartImportRequired(type)) {
                if (type.Name.Contains("[]")) {
                    imports.AddRange(ConvertCsTypeToDartImportType(GetTypeByName(type.FullName!.Replace("[]", ""))!));
                } else if (type.Name != "List`1" && type.Name != "Dictionary`2") {
                    imports.Add(type);
                }
                
                foreach (Type genericType in type.GenericTypeArguments) {
                    imports.AddRange(ConvertCsTypeToDartImportType(genericType));
                }   
            } 

            return imports;
        }

        public static string CamelCaseToSnakeCase(string str)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < str.Length; i++) {
                char c = str[i];
                if (char.IsUpper(c)) {
                    if (i != 0) {
                        builder.Append("_");
                    }

                    builder.Append(char.ToLower(c));
                } else {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

        public static string CamelCaseToLowerCamelCase(string str) =>
            string.Concat(char.ToLower(str[0]), str.Substring(1));

        public static string CamelCaseToLowerCamelCase(IEnumerable<char> str) =>
            string.Concat(char.ToLower(str.ElementAt(0)), new string(str.ToArray()).Substring(1));
    }
}
