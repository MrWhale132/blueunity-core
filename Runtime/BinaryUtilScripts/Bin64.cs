using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Scripts.UtilScripts.BinaryUtilScripts
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
