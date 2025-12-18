using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts.Misc
{
    public static class AssetUtils
    {
        /// <summary>
        /// Checks whether a Unity object was generated at runtime (not stored in an asset file or scene).
        /// </summary>
        public static bool IsProbablyUnmodifiedCopyOfOriginalAsset(this Object obj)
        {
            if (obj == null)
                return false;

            // Often used for temporary or dynamically-created objects
            //if (!obj.hideFlags.HasFlag(HideFlags.DontSave))
            //    return false;

            // Fallback heuristic: Unity sometimes appends Instance
            if (obj.name.EndsWith("Instance", System.StringComparison.Ordinal)
                || obj.name.EndsWith("(Instance)", System.StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }
    }
}