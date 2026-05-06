using System;
using Theblueway.Core.Extensions;
using UnityEngine;

namespace Theblueway.Core.Common
{
    public class ObjectFactory
    {
        public static object CreateInstance(Type type)
        {
            try
            {
                var instance = Activator.CreateInstance(type, nonPublic: true);
                return instance;
            }
            catch (Exception e)
            {
                Debug.LogError($"{nameof(ObjectFactory)} cant create instance of type {type.CleanAssemblyQualifiedName()}.\n" +
                    $"Exception: {e}");
                throw;
            }
        }

        public static T CreateInstance<T>(Type type)
        {
            var instance = CreateInstance(type);
            return (T)instance;
        }
        public static T CreateInstance<T>()
        {
            return CreateInstance<T>(typeof(T));
        }
    }
}
