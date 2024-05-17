using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class QuaternionExtensions
    {
        /// <summary>
        ///  Applies a tumbling rotation to the Quaternion with the specified intensity over the given duration, creating a disorienting effect.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Tumble(this Quaternion quaternion, Transform transform, float intensity, float duration)
        {
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float angle = Mathf.Sin(t * Mathf.PI * 2) * intensity;
                transform.rotation = startRotation * Quaternion.AngleAxis(angle, Vector3.up);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = startRotation; // Reset rotation
        }
        /// <summary>
        /// Bends the Quaternion around the specified axis by the given angle over the specified duration.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <param name="axis"></param>
        /// <param name="angle"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Bend(this Quaternion quaternion, Transform transform, Vector3 axis, float angle, float duration)
        {
            Quaternion startRotation = transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.AngleAxis(angle, axis);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation; // Ensure reaching the target rotation
        }
        /// <summary>
        /// Adds random jitter to the Quaternion's rotation with the specified intensity over the given duration.
        /// </summary>
        /// <param name="quaternion"></param>
        /// <param name="transform"></param>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Jitter(this Quaternion quaternion, Transform transform, float intensity, float duration)
        {
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float angle = Mathf.PerlinNoise(Time.time * intensity, Time.time * intensity) * intensity * 2f - intensity;
                transform.rotation = startRotation * Quaternion.AngleAxis(angle, Vector3.up);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = startRotation; // Reset rotation
        }


        // Add other unique Quaternion extension methods here...
    }
}
