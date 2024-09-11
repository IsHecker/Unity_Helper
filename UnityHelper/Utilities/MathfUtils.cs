using UnityEngine;

namespace UnityHelper.Utilities
{
    public static class MathfUtils
    {
        /// <summary>
        ///  Performs smooth Hermite interpolation between 0 and 1 when t is in the range [edge0, edge1].
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Smoothstep(float edge0, float edge1, float t)
        {
            t = Mathf.Clamp01((t - edge0) / (edge1 - edge0));
            return t * t * (3f - 2f * t);
        }
        /// <summary>
        /// Performs smoother Hermite interpolation between 0 and 1 when t is in the range [edge0, edge1].
        /// </summary>
        /// <param name="edge0"></param>
        /// <param name="edge1"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Smootherstep(float edge0, float edge1, float t)
        {
            t = Mathf.Clamp01((t - edge0) / (edge1 - edge0));
            return t * t * t * (t * (t * 6f - 15f) + 10f);
        }
        public static float Map(float value, float minInput, float maxInput, float minOutput, float maxOutput)
        {
            return minOutput + (value - minInput) * (maxOutput - minOutput) / (maxInput - minInput);
        }
        /// <summary>
        /// Checks if <paramref name="number"/> is in specified Range.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool IsInRange(float number, float min, float max)
        {
            return min <= number && number <= max;
        }
    }
}
