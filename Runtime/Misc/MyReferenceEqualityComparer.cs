
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Theblueway.Core.Common
{
    //this sealed has nothing special to it, just a nice-to-have
    public sealed class MyReferenceEqualityComparer : IEqualityComparer<object>
    {
        public static readonly MyReferenceEqualityComparer Instance = new();

        private MyReferenceEqualityComparer() { }

        public new bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }

        public int GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}
