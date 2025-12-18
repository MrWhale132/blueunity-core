
using Assets._Project.Scripts.UtilScripts;
using UnityEngine;

namespace Packages.com.blueutils.core.Runtime.Misc
{
    [CreateAssetMenu(fileName = "RandomIdReference", menuName = "Scriptable Objects/Utils/RandomIdReference")]
    public class RandomIdReference:ScriptableObject
    {
        public RandomId Id;

        public static implicit operator RandomId(RandomIdReference reference)
        {
            if (reference == null) return RandomId.Default;
            return reference.Id;
        }
    }
}
