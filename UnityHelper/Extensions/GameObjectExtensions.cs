using System;
using System.Collections;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Makes the GameObject bounce up and down in place with the specified height and duration.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="height"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Bounce(this GameObject gameObject, float height, float duration)
        {
            Vector3 startPosition = gameObject.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                float yOffset = Mathf.Sin(t * Mathf.PI) * height;
                gameObject.transform.position = startPosition + Vector3.up * yOffset;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.transform.position = startPosition; // Reset position
        }
        /// <summary>
        /// Rotates the GameObject around its up axis by the specified angle over the given duration, creating a twisting motion.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="angle"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Twist(this GameObject gameObject, float angle, float duration)
        {
            Quaternion startRotation = gameObject.transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.AngleAxis(angle, gameObject.transform.up);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                gameObject.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.transform.rotation = targetRotation; // Ensure reaching the target rotation
        }
        /// <summary>
        /// Causes the GameObject to smoothly pulse its scale between its current scale and the specified target scale over time.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="targetScale"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator PulseScale(this GameObject gameObject, Vector3 targetScale, float duration)
        {
            Vector3 startScale = gameObject.transform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, Mathf.PingPong(t * 2f, 1f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gameObject.transform.localScale = startScale; // Reset scale
        }

    }
}
