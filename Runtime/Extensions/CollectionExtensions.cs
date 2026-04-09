
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Project.Scripts.UtilScripts.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static string StringJoin(this IEnumerable<string> strings, string separator) => string.Join(separator, strings);

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection == null || !collection.Any();
        public static bool IsNotNullAndNotEmpty<T>(this IEnumerable<T> collection) => !collection.IsNullOrEmpty();

        public static bool Has<T>(this IEnumerable<T> collection, Func<T,bool> predicate, out T match)
        {
            match = collection.FirstOrDefault(predicate);

            return !Equals(match, default(T));
        }
    }
}
