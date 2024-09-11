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

        public static Vector2Int ToVector2Int(this Vector2 vector)
        {
            return new Vector2Int((int)vector.x, (int)vector.y);
        }

        public static Vector3Int ToVector3Int(this Vector3 vector)
        {
            return new Vector3Int((int)vector.x, (int)vector.y, (int)vector.z);
        }
    }
}
