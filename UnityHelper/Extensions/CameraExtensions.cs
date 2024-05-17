using System.Collections;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class CameraExtensions
    {
        /// <summary>
        /// Causes the camera to wiggle or shake with the specified intensity over the given duration.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Wiggle(this Camera camera, float intensity, float duration)
        {
            Vector3 startPosition = camera.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Vector3 offset = UnityEngine.Random.insideUnitSphere * intensity;
                camera.transform.position = startPosition + offset;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            camera.transform.position = startPosition; // Reset position
        }
        /// <summary>
        /// Smoothly zooms the camera to focus on the specified target Transform over the given duration.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator ZoomTo(this Camera camera, Transform target, float duration)
        {
            Vector3 startPosition = camera.transform.position;
            Quaternion startRotation = camera.transform.rotation;
            float startSize = camera.orthographicSize;

            Vector3 targetPosition = target.position;
            Quaternion targetRotation = target.rotation;
            float targetSize = camera.orthographicSize;

            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                camera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                camera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure reaching the target position, rotation, and size
            camera.transform.position = targetPosition;
            camera.transform.rotation = targetRotation;
            camera.orthographicSize = targetSize;
        }
        /// <summary>
        /// Pans the camera along the specified direction by the given distance over the specified duration.
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Pan(this Camera camera, Vector3 direction, float distance, float duration)
        {
            Vector3 startPosition = camera.transform.position;
            Vector3 targetPosition = startPosition + direction.normalized * distance;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                camera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure reaching the target position
            camera.transform.position = targetPosition;
        }

        // Add other unique Camera extension methods here...
    }
}
