
using System;
using System.Buffers;
using System.Collections.Generic;
using Theblueway.Core.Base64Utilities;
using Theblueway.Core.BinaryUtilities;
using Theblueway.Core.DataStructures;

namespace Theblueway.Core.Compression.Json
{
    public static class JsonCompression
    {
        [Serializable]
        public class CompressedBuffer
        {
            public string codec;      // "lz4", "gzip", etc.
            public int originalBufferElementCount;
            public string dataBase64; // compressed payload
        }

        public static void CompressBuffer<T>(List<T> buffer, CompressedBuffer compressedBuffer, int elementSize) where T : struct
        {
            using PooledBytes packed = BinaryPacking.PackIntoPooled<T>(buffer, elementSize);

            CompressBuffer<T>(packed, elementSize, buffer == null ? 0 : buffer.Count, compressedBuffer);
        }
        public static void CompressBuffer<T>(T[] buffer, CompressedBuffer compressedBuffer, int elementSize) where T : struct
        {
            using PooledBytes packed = BinaryPacking.PackIntoPooled<T>(buffer, elementSize);
            CompressBuffer<T>(packed, elementSize, buffer == null ? 0 : buffer.Length, compressedBuffer);
        }
        public static void CompressBuffer<T>(PooledArray<T> buffer, CompressedBuffer compressedBuffer, int elementSize) where T : struct
        {
            using PooledBytes packed = BinaryPacking.PackIntoPooled<T>(buffer.array, buffer.length, elementSize);
            CompressBuffer<T>(packed, elementSize, buffer.length, compressedBuffer);
        }

        public static void CompressBuffer<T>(PooledBytes packed, int elementSize, int elementCount, CompressedBuffer compressedBuffer) where T : struct
        {
            using PooledBytes compressed = BinaryCompression.CompressGzipPooled(packed);
            string base64 = Base64Util.ToBase64StringPooled(compressed);

            //if (buffer != null)
            {
                //Debug.Log((elementCount, packed.Length, packed.Buffer.Length, compressed.Length, compressed.Buffer.Length, base64.Length, elementSize));
            }

            compressedBuffer.codec = "gzip";
            compressedBuffer.dataBase64 = base64;
            compressedBuffer.originalBufferElementCount = elementCount;
        }




        public static void DecompressBuffer<T>(CompressedBuffer compressedBuffer, ref List<T> buffer, int elementSize) where T : struct
        {
            if (buffer.Capacity < compressedBuffer.originalBufferElementCount)
            {
                buffer.Capacity = compressedBuffer.originalBufferElementCount;
            }

            byte[] compressed = Convert.FromBase64String(compressedBuffer.dataBase64);
            byte[] packed = BinaryCompression.DecompressGzip(compressed);
            BinaryPacking.UnpackArrayInto<T>(packed, ref buffer, elementSize);
        }


        public static void DecompressBuffer<T>(CompressedBuffer compressedBuffer, ref T[] buffer, int elementSize) where T : struct
        {
            byte[] compressed = Convert.FromBase64String(compressedBuffer.dataBase64);
            byte[] packed = BinaryCompression.DecompressGzip(compressed);

            //UnityEngine.Debug.Log((compressedBuffer.originalBufferElementCount, 0,packed.Length, 0, compressed.Length, compressedBuffer.dataBase64.Length, elementSize));
            int bytesLenght = compressedBuffer.originalBufferElementCount * elementSize;
            BinaryPacking.UnpackArrayInto<T>(packed, bytesLenght, ref buffer, elementSize);
        }


        public static T[] DecompressBuffer<T>(CompressedBuffer compressedBuffer, int elementSize) where T : struct
        {
            byte[] compressed = Convert.FromBase64String(compressedBuffer.dataBase64);
            byte[] packed = BinaryCompression.DecompressGzip(compressed);
            T[] buffer = BinaryPacking.UnpackArray<T>(packed, elementSize);

            return buffer;
        }



        public static void DecompressBufferIntoPooled<T>(CompressedBuffer compressedBuffer, ref PooledArray<T> buffer, int elementSize) where T : struct
        {
            DecompressBufferIntoPooled(compressedBuffer, ref buffer.array, elementSize);
            buffer.length = compressedBuffer.originalBufferElementCount;
        }

        public static void DecompressBufferIntoPooled<T>(CompressedBuffer compressedBuffer, ref T[] buffer, int elementSize) where T : struct
        {
            if (buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(compressedBuffer.originalBufferElementCount);
            }
            else if (buffer.Length < compressedBuffer.originalBufferElementCount)
            {
                ArrayPool<T>.Shared.Return(buffer);
                buffer = ArrayPool<T>.Shared.Rent(compressedBuffer.originalBufferElementCount);
            }

            DecompressBuffer(compressedBuffer, ref buffer, elementSize);
        }

    }
}
