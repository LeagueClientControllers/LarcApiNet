using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        public static string CsTypeToDartTypeConverter(Type type)
        {
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) {
                if (type.GenericTypeArguments.Length == 0) {
                    string innerType = CsTypeToDartTypeConverter(GetTypeByName(type.FullName!.Replace("[]", ""))!);
                    return $"List<{innerType}>";
                } else if (type.GenericTypeArguments.Length == 1) {
                    string innerType = CsTypeToDartTypeConverter(type.GenericTypeArguments[0]);
                    return $"List<{innerType}>";
                } else if (type.GenericTypeArguments.Length == 2) {
                    string innerType1 = CsTypeToDartTypeConverter(type.GenericTypeArguments[0]);
                    string innerType2 = CsTypeToDartTypeConverter(type.GenericTypeArguments[1]);
                    return $"Map<{innerType1}, {innerType2}>";
                }
            }

            if (type == typeof(int) || type == typeof(long)) {
                return "int";
            } else if (type == typeof(double) || type == typeof(float) || type == typeof(decimal)) {
                return "double";
            } else if (type == typeof(bool)) {
                return "bool";
            } else if (type == typeof(string)) {
                return "String";
            } else if (type == typeof(object)) {
                return "Object";
            } else {
                return type.Name;
            }
        }

        public static bool CsTypeDartImportRequired(Type type)
        {
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) {
                if (type.GenericTypeArguments.Length == 0) {
                    type = GetTypeByName(type.FullName!.Replace("[]", ""))!;
                } else if (type.GenericTypeArguments.Length == 1) {
                    type = type.GenericTypeArguments[0];
                } else if (type.GenericTypeArguments.Length == 2) {
                    return CsTypeDartImportRequired(type.GenericTypeArguments[0]) || CsTypeDartImportRequired(type.GenericTypeArguments[1]);
                }
            }

            return !(type == typeof(int) || type == typeof(long)) 
                && !(type == typeof(double) || type == typeof(float) || type == typeof(decimal)) 
                && type != typeof(bool)
                && type != typeof(string) 
                && type != typeof(object);
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

    }
}
