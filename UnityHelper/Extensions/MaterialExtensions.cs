using System.Collections;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class MaterialExtensions
    {
        /// <summary>
        /// Applies a ripple effect to the material with the specified intensity over the given duration.
        /// </summary>
        /// <param name="material"></param>
        /// <param name="intensity"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator Ripple(this Material material, float intensity, float duration)
        {
            float startTime = Time.time;
            Vector2 offset = Vector2.zero;

            while (Time.time - startTime < duration)
            {
                float t = (Time.time - startTime) / duration;
                float wave = Mathf.Sin(t * Mathf.PI * 2) * intensity;
                offset.x = wave;
                material.SetTextureOffset("_MainTex", offset);
                yield return null;
            }

            offset.x = 0f; // Reset offset
            material.SetTextureOffset("_MainTex", offset);
        }

        // Add other unique Material extension methods here...
    }
}
