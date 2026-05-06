    using System;
    using System.Buffers;
using Theblueway.Core.BinaryUtilities;

namespace Theblueway.Core.Base64Utilities
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

        private static int ToBase64CharArrayLength(int byteLength)
        {
            return ((byteLength + 2) / 3) * 4;
        }
    }

}
