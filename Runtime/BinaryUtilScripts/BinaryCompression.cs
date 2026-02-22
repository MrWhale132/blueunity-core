
using System.Buffers;
using System.IO;
using System.IO.Compression;

namespace Theblueway.Core.Runtime.Packages.com.blueutils.core.Runtime.BinaryUtilScripts
{
    public static class BinaryCompression
    {
        //public static PooledBytes CompressGzipPooled(PooledBytes input)
        //{
        //    using var ms = new MemoryStream();

        //    using (var gzip = new GZipStream(ms, CompressionLevel.Fastest, leaveOpen: true))
        //        gzip.Write(input.Buffer, 0, input.Length);

        //    var compressed = ms.ToArray();
        //    if (input.Buffer.Length == 0)
        //    {
        //        UnityEngine.Debug.Log(compressed.Length);
        //    }

        //    byte[] rented = ArrayPool<byte>.Shared.Rent(compressed.Length);
        //    compressed.CopyTo(rented, 0);

        //    return new PooledBytes
        //    {
        //        Buffer = rented,
        //        Length = rented.Length
        //    };
        //}
        public static PooledBytes CompressGzipPooled(PooledBytes input)
        {
            // Worst-case gzip can slightly expand, so give headroom
            int maxSize = input.Length + (input.Length / 10) + 64;

            byte[] rented = ArrayPool<byte>.Shared.Rent(maxSize);
            int written;

            using (var ms = new MemoryStream(rented, writable: true))
            {
                using (var gzip = new GZipStream(ms, CompressionLevel.Fastest, leaveOpen: true))
                {
                    gzip.Write(input.Buffer, 0, input.Length);
                } // ← IMPORTANT: dispose gzip first because it writes footer on dispose that advances the stream position

                written = (int)ms.Position;
            }

            return new PooledBytes
            {
                Buffer = rented,
                Length = written
            };
        }


        public static byte[] DecompressGzip(byte[] input)
        {
            using var inputMs = new MemoryStream(input);
            using var gzip = new GZipStream(inputMs, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);
            return output.ToArray();
        }

    }
}
