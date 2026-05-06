namespace Theblueway.Core.BinaryUtilities
{
    public class Bin64
    {
        public static void SetFlag(ulong field, ulong flag, bool value = true)
        {
            if (value)
                field |= flag;
            else
                field &= ~flag;
        }

        public static bool HasFlag(ulong field, ulong flag) => (field & flag) != 0;
    }
}
