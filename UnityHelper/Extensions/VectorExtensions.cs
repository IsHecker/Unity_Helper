using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 RotateAround(this Vector3 vector, Vector3 axis, float angle)
        {
            Quaternion rotation = Quaternion.AngleAxis(angle, axis);
            return rotation * vector;
        }

        public static Vector3 ProjectOntoPlane(this Vector3 vector, Vector3 planeNormal)
        {
            return vector - Vector3.Project(vector, planeNormal);
        }

        // Add other unique Vector extension methods here...
    }

}
