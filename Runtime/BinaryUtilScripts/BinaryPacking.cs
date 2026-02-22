
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Theblueway.Core.Runtime.Packages.com.blueutils.core.Runtime.BinaryUtilScripts
{
    //todo: add a IsBlittable(Type) safeguard
    public static class BinaryPacking
    {
        public const int Vector2PackSize = 8;
        public const int Vector3PackSize = 12;
        public const int Vector4PackSize = 16;
        public const int ColorPackSize = 4;


        public static Dictionary<Type, int> BlittableTypePackSize = new()
        {
            { typeof(Vector2), Vector2PackSize },
            { typeof(Vector3), Vector3PackSize },
            { typeof(Vector4), Vector4PackSize },
            { typeof(Color), ColorPackSize },
            { typeof(int), 4 },
        };



        public static PooledBytes PackIntoPooled<T>(List<T> arr, int size) where T : struct
        {
            if (_IsNullOrEmpty(arr, out var pooledBytes)) return pooledBytes;

            int length = arr.Count;

            T[] rented = ArrayPool<T>.Shared.Rent(length);

            arr.CopyTo(rented);

            pooledBytes = PackIntoPooled(rented, length, size);

            ArrayPool<T>.Shared.Return(rented);

            return pooledBytes;
        }



        public static PooledBytes PackIntoPooled<T>(T[] arr, int size) where T : struct
        {
            return PackIntoPooled(arr, arr == null ? 0 : arr.Length, size);
        }

        public static PooledBytes PackIntoPooled<T>(T[] arr, int length, int size) where T : struct
        {
            if (_IsNullOrEmpty<T>(arr, length, out var pooledBytes)) return pooledBytes;


            int byteCount = length * size;

            var buffer = ArrayPool<byte>.Shared.Rent(byteCount);

            var srcBytes = MemoryMarshal.AsBytes(arr.AsSpan(0, length));
            srcBytes.CopyTo(buffer.AsSpan(0, byteCount));

            return new PooledBytes
            {
                Buffer = buffer,
                Length = byteCount
            };
        }







        public static void UnpackArrayInto<T>(byte[] bytes, ref List<T> destination, int elementSize) where T : struct
        {
            CheckByteRemainder(bytes.Length, elementSize);
            //CheckIfBytesFitIntoDestionation(bytes.Length, destination.Capacity, elementSize);

            int length = destination.Capacity;

            T[] rented = ArrayPool<T>.Shared.Rent(length);

            Span<byte> dstBytes = MemoryMarshal.AsBytes(rented.AsSpan(0, length));
            bytes.AsSpan().CopyTo(dstBytes);

            //todo: better copy
            for (int i = 0; i < length; i++)
            {
                if (destination.Count > i)
                    destination[i] = rented[i];
                else
                    destination.Add(rented[i]);
            }
        }

        public static void UnpackArrayInto<T>(byte[] bytes, int bytesLength, ref T[] destination, int elementSize) where T : struct
        {
            CheckByteRemainder(bytesLength, elementSize);
            CheckIfBytesFitIntoDestionation(bytesLength, destination.Length, elementSize);

            Span<byte> dstBytes = MemoryMarshal.AsBytes(destination.AsSpan());
            bytes.AsSpan(0, bytesLength).CopyTo(dstBytes);
        }


        public static T[] UnpackArray<T>(byte[] bytes, int size) where T : struct
        {
            if (bytes == null || bytes.Length == 0)
                return Array.Empty<T>();

            CheckByteRemainder(bytes.Length, size);
            //int remainder = bytes.Length % size;
            //if (remainder != 0)
            //    throw new ArgumentException($"Invalid {typeof(T).FullName} byte buffer." +
            //        $"Byte buffer length: {bytes.Length}, element size: {size}, remainder: {remainder}");

            var arr = new T[bytes.Length / size];

            Span<byte> dstBytes = MemoryMarshal.AsBytes(arr.AsSpan());
            bytes.AsSpan().CopyTo(dstBytes);

            return arr;
        }





        public static PooledBytes CreateDefaultPooledBytes()
        {
            return new PooledBytes
            {
                Buffer = ArrayPool<byte>.Shared.Rent(0),
                Length = 0
            };
        }


        public static bool _IsNull<T>(object arr, out PooledBytes pooledBytes)
        {
            if (arr == null)
            {
                pooledBytes = CreateDefaultPooledBytes();
                return true;
            }
            pooledBytes = default;
            return false;
        }


        public static bool _IsNullOrEmpty<T>(object arr, int length, out PooledBytes pooledBytes)
        {
            if (_IsNull<T>(arr, out pooledBytes)) return true;

            if (length == 0)
            {
                pooledBytes = CreateDefaultPooledBytes();
                return true;
            }

            pooledBytes = default;
            return false;
        }


        public static bool _IsNullOrEmpty<T>(T[] arr, out PooledBytes pooledBytes)
        {
            return _IsNullOrEmpty<T>(arr, arr.Length, out pooledBytes);
        }

        public static bool _IsNullOrEmpty<T>(List<T> arr, out PooledBytes pooledBytes)
        {
            return _IsNullOrEmpty<T>(arr, arr.Count, out pooledBytes);
        }





        public static void CheckIfBytesFitIntoDestionation(int byteCount, int destionationCount, int elementSize)
        {
            int requiredByteCount = destionationCount * elementSize;
            if (byteCount > requiredByteCount)
                throw new ArgumentException($"Byte buffer does not fit into destination array." +
                    $"Byte buffer length: {byteCount}, destination element count: {destionationCount}, element size: {elementSize}, required byte count: {requiredByteCount}");
        }


        public static void CheckIfBytesFitExactlyIntoDestionation(int byteCount, int destionationCount, int elementSize)
        {
            int requiredByteCount = destionationCount * elementSize;
            if (byteCount != requiredByteCount)
                throw new ArgumentException($"Byte buffer does not fit exactly into destination array." +
                    $"Byte buffer length: {byteCount}, destination element count: {destionationCount}, element size: {elementSize}, required byte count: {requiredByteCount}");
        }

        public static void CheckByteRemainder(int length, int size)
        {
            int remainder = length % size;
            if (remainder != 0)
                throw new ArgumentException($"Invalid byte buffer." +
                    $"Byte buffer length: {length}, element size: {size}, remainder: {remainder}");
        }
    }
}
