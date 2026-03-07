using System.Reflection;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace Theblueway.Core.Editor.Extensions
{
    public static class SerializedPropertyExtensions
    {
        public static Dictionary<Type, Dictionary<string, MemberInfo>> _memberInfoCache = new();


        public static bool EqualContents(this SerializedProperty property1, SerializedProperty property2)
        {
            return SerializedProperty.EqualContents(property1, property2);
        }

        public static bool IsDefined<T>(this SerializedProperty property, bool inherit = false) where T : Attribute
        {
            var member = GetReflectionMemberInfo(property);

            if (member == null)
            {
                return false;
            }

            return member.IsDefined(typeof(T), inherit);
        }

        public static bool HasAttribute<T>(this SerializedProperty property, out T attribute) where T : Attribute
        {
            var member = GetReflectionMemberInfo(property);

            if (member == null)
            {
                attribute = null;
                return false;
            }

            attribute = member.GetCustomAttribute<T>();

            return attribute != null;
        }


        public static MemberInfo GetReflectionMemberInfo(this SerializedProperty prop)
        {
            Type origHost = prop.serializedObject.targetObject.GetType();

            if (_memberInfoCache.TryGetValue(origHost, out var members)
                && members.TryGetValue(prop.propertyPath, out var member))
            {
                return member;
            }


            string[] elements = prop.propertyPath
                .Replace(".Array.data[", "[")
                .Split('.');

            Type host = origHost;
            member = null;


            foreach (var element in elements)
            {
                if (element.Contains("["))
                {
                    string name = element.Substring(0, element.IndexOf("["));
                    var field = host.GetField(name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    Type elementType =
                        field.FieldType.IsArray
                            ? field.FieldType.GetElementType()
                            : field.FieldType.GetGenericArguments()[0];

                    host = elementType;
                    member = field;
                }
                else
                {
                    var field = host.GetField(element,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    host = field.FieldType;
                    member = field;
                }
            }

            if (!_memberInfoCache.ContainsKey(origHost))
            {
                _memberInfoCache.Add(origHost, new());
            }

            _memberInfoCache[origHost].Add(prop.propertyPath, member);

            return member;
        }
    }
}
