
using System;
using System.Buffers;

namespace Theblueway.Core.BinaryUtilities
{
    public struct PooledBytes : IDisposable
    {
        public byte[] Buffer;
        public int Length;
        internal bool _disposed;

        public readonly bool IsDefault => Buffer == null;

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            if (Buffer != null)
            {
                ArrayPool<byte>.Shared.Return(Buffer);
                Buffer = null;
                Length = 0;
            }
        }
    }

}
