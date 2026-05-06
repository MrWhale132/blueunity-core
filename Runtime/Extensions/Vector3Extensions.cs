using UnityEngine;

namespace Theblueway.Core.Extensions
{

    public static class Vector3Extensions
    {
        public static Quaternion AsRotation(this  Vector3 velocity)
        {
            return Quaternion.AngleAxis(velocity.magnitude * Mathf.Rad2Deg, velocity.normalized);
        }
    }
}
