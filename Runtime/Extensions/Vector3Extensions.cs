using UnityEngine;

namespace Assets._Project.Scripts.UtilScripts.Extensions
{

    public static class Vector3Extensions
    {
        public static Quaternion AsRotation(this  Vector3 velocity)
        {
            return Quaternion.AngleAxis(velocity.magnitude * Mathf.Rad2Deg, velocity.normalized);
        }
    }
}
