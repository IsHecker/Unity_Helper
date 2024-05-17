using System.Collections;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class SpriteExtensions
    {
        /// <summary>
        /// Causes the sprite to smoothly pulse its color between the specified start and end colors over the given duration.
        /// </summary>
        /// <param name="spriteRenderer"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static IEnumerator PulseColor(this SpriteRenderer spriteRenderer, Color startColor, Color endColor, float duration)
        {
            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = (Time.time - startTime) / duration;
                Color color = Color.Lerp(startColor, endColor, Mathf.PingPong(t * 2, 1f)); // Ping-pong between start and end colors
                spriteRenderer.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = startColor; // Reset color
        }

        // Add other unique Sprite extension methods here...
    }
}
