
using Theblueway.Core.Attributes;
using Theblueway.Core.DataStructures;
using UnityEngine;

namespace Theblueway.Core.Common
{
    [CreateAssetMenu(fileName = "RandomIdReference", menuName = "Scriptable Objects/Theblueway/Infra/RandomIdReference")]
    public class RandomIdReference:ScriptableObject
    {
        [AllowEdit(RandomIdEditMode =RandomIdEditMode.Paste | RandomIdEditMode.Generate)]
        public RandomId Id;

        public static implicit operator RandomId(RandomIdReference reference)
        {
            if (reference == null) return RandomId.Default;
            return reference.Id;
        }
    }
}
