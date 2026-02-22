    using System;
    using System.Buffers;

namespace Theblueway.Core.Runtime.Packages.com.blueutils.core.Runtime.BinaryUtilScripts
{
    public static class Base64Util
    {
        public static string ToBase64StringPooled(PooledBytes pooledBytes)
        {
            return ToBase64String(pooledBytes.Buffer, pooledBytes.Length);
        }

        public static string ToBase64String(byte[] data, int length)
        {
            if (data == null || length == 0) return string.Empty;

            // exact char count required
            int charCount = ToBase64CharArrayLength(length);

            char[] rented = ArrayPool<char>.Shared.Rent(charCount);

            try
            {
                Convert.TryToBase64Chars(
                    data.AsSpan(0, length),
                    rented,
                    out int written);
                
                return new string(rented, 0, written);
            }
            finally
            {
                ArrayPool<char>.Shared.Return(rented);
            }
        }

        // .NET doesn't expose this publicly, so we compute it
        private static int ToBase64CharArrayLength(int byteLength)
        {
            return ((byteLength + 2) / 3) * 4;
        }
    }

}
