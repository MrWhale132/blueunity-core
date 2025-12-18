
using System.Collections.Generic;

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

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection) => collection == null || collection.Count == 0;
        public static bool IsNotNullAndNotEmpty<T>(this ICollection<T> collection) => !collection.IsNullOrEmpty();
    }
}
