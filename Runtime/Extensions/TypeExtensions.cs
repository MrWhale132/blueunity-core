
using System;
using System.Text.RegularExpressions;

namespace Theblueway.Core.Runtime.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableTo(this Type type, Type targetType)
        {
            return targetType.IsAssignableFrom(type);
        }

        public static string CleanAssemblyQualifiedName(this Type type)
        {
            if (type == null) return null;

            var assemblyTypeName = type.AssemblyQualifiedName;
            if (string.IsNullOrEmpty(assemblyTypeName)) return assemblyTypeName;

            // Remove Version=..., Culture=..., PublicKeyToken=... tokens (case-insensitive)
            assemblyTypeName = Regex.Replace(assemblyTypeName, @",\s*Version=[^,\]]*", "", RegexOptions.IgnoreCase);
            assemblyTypeName = Regex.Replace(assemblyTypeName, @",\s*Culture=[^,\]]*", "", RegexOptions.IgnoreCase);
            assemblyTypeName = Regex.Replace(assemblyTypeName, @",\s*PublicKeyToken=[^,\]]*", "", RegexOptions.IgnoreCase);

            // Collapse any accidental double-commas produced by removals (", ,") into a single ","
            assemblyTypeName = Regex.Replace(assemblyTypeName, @",\s*,\s*", ", ");

            return assemblyTypeName;
        }
    }
}
