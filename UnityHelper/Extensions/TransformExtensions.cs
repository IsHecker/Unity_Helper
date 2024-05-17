using System.Collections;
using UnityEngine;

namespace Assets.UnityHelpers.Extensions
{
    public static class TransformExtensions
    {
        /// <summary>
        /// Moves the Transform in an arc trajectory to the specified target position with the given arc height over the specified duration.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="targetPosition"></param>
        /// <param name="arcHeight"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator MoveInArc(this Transform transform, Vector3 targetPosition, float arcHeight, float duration)
        {
            Vector3 startPosition = transform.position;
            Vector3 controlPoint = startPosition + (targetPosition - startPosition) / 2f + Vector3.up * arcHeight;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Vector3 bezierPoint = BezierCurve(startPosition, controlPoint, targetPosition, t);
                transform.position = bezierPoint;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition; // Ensure reaching the target position
        }

        // Helper method for calculating a point on a 3D bezier curve
        private static Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
        }

    }
}
