
using System;
using System.Buffers;
using System.Collections.Generic;
using Theblueway.Core.DataStructures;

namespace Theblueway.Core.PooledArrayUtilities
{
    public static class ArrayUtils
    {
        public static void CopyToPooled<T>(List<T> source, ref PooledArray<T> destination)
        {
            CopyToPooled(source, ref destination.array);
            destination.length = source == null ? 0 : source.Count;
        }


        public static void CopyToPooled<T>(List<T> source, ref T[] destionation)
        {
            int offset = 0;
            CopyToPooled(source, ref destionation, ref offset);
        }
        public static void CopyToPooled<T>(List<T> source, ref T[] destination, ref int destOffset)
        {
            if (destination == null)
            {
                destination = ArrayPool<T>.Shared.Rent(source.Count);
            }
            else if (destination.Length < source.Count)
            {
                ArrayPool<T>.Shared.Return(destination);
                destination = ArrayPool<T>.Shared.Rent(source.Count);
            }

            source.CopyTo(destination, destOffset);
        }

        public static void Copy<T>(T[] source, T[] destination, ref int offset)
        {
            Array.Copy(source, 0, destination, offset, source.Length);
            offset += source.Length;
        }
    }
}
