
using Assets._Project.Scripts.UtilScripts;
using Theblueway.Core;
using UnityEngine;

namespace Packages.com.blueutils.core.Runtime.Misc
{
    [CreateAssetMenu(fileName = "RandomIdReference", menuName = "Scriptable Objects/Infra/RandomIdReference")]
    public class RandomIdReference:ScriptableObject
    {
        [AllowEdit(RandomIdEditMode =RandomIdEditMode.Paste)]
        public RandomId Id;

        public static implicit operator RandomId(RandomIdReference reference)
        {
            if (reference == null) return RandomId.Default;
            return reference.Id;
        }
    }
}
