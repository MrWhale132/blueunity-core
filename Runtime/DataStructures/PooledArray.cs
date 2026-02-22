
using System;
using System.Buffers;

namespace Theblueway.Core.Runtime.Packages.com.blueutils.core.Runtime.DataStructures
{
    public struct PooledArray<T>:IDisposable
    {
        public T[] array;
        public int length;

        internal bool _isDisposed;

        public PooledArray(T[] array, int length)
        {
            this.array = array;
            this.length = length;
            _isDisposed = false;
        }

        public void Dispose()
        {
            if(_isDisposed) return;
            _isDisposed = true;

            if (array != null)
            {
                ArrayPool<T>.Shared.Return(array);
                array = null;
                length = 0;
            }
        }


        public static implicit operator T[](PooledArray<T> pooledArray)
        {
            return pooledArray.array;
        }
    }
}
