using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper.Extensions
{
    public static class ComponentExtensions
    {
        /// <summary>
        ///  Makes the component blink between the specified start and end colors over the given duration.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="duration"></param>
        /// <param name="startColor"></param>
        /// <param name="endColor"></param>
        /// <returns></returns>
        public static IEnumerator Blink(this Component component, float duration, Color startColor, Color endColor)
        {
            Material material = component.GetComponent<Renderer>().material;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                Color color = Color.Lerp(startColor, endColor, Mathf.PingPong(t * 2, 1f)); // Ping-pong between start and end colors
                material.color = color;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            material.color = startColor; // Reset color
        }
        /// <summary>
        /// Rotates the GameObject continuously around its up axis at the specified speed.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static IEnumerator Spin(this Component component, float speed)
        {
            while (true)
            {
                component.transform.Rotate(Vector3.up, speed * Time.deltaTime);
                yield return null;
            }
        }
        /// <summary>
        ///  Makes the GameObject follow a path defined by a list of waypoints at the specified speed.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="path"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public static IEnumerator FollowPath(this Component component, List<Vector3> path, float speed)
        {
            int index = 0;
            while (index < path.Count)
            {
                Vector3 targetPosition = path[index];
                while (Vector3.Distance(component.transform.position, targetPosition) > 0.01f)
                {
                    component.transform.position = Vector3.MoveTowards(component.transform.position, targetPosition, speed * Time.deltaTime);
                    yield return null;
                }
                index = (index + 1) % path.Count;
            }
        }

        // Add other unique Component extension methods here...
    }
}
