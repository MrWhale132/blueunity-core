using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts.Misc
{
    public static class AssetUtils
    {
        /// <summary>
        /// Checks whether a Unity object was generated at runtime (not stored in an asset file or scene).
        /// </summary>
        public static bool IsDefensiveCopyOfOriginal(this Object obj)
        {
            return IsDefensiveCopyOfOriginal(obj, out _);
        }
        public static bool IsDefensiveCopyOfOriginal(this Object obj, out string origName)
        {
            if (obj == null)
            {
                origName = null;
                return false;
            }

            // Often used for temporary or dynamically-created objects
            //if (!obj.hideFlags.HasFlag(HideFlags.DontSave))
            //    return false;

            // Fallback heuristic: Unity sometimes appends Instance
            if (obj.name.EndsWith("Instance", System.StringComparison.Ordinal)
                || obj.name.EndsWith("(Instance)", System.StringComparison.Ordinal))
            {
                origName= obj.name.Substring(0, obj.name.LastIndexOf(' '));
                return true;
            }

            origName = null;
            return false;
        }
    }
}