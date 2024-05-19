using System.Collections.Generic;
using UnityEngine;

namespace Unity_Helper.UnityHelper.Utilities
{
    public static class CoroutineUtils
    {
        private readonly static Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitFor(float seconds) =>
            waitDictionary.TryGetValue(seconds, out var wait) ? wait : waitDictionary[seconds] = new WaitForSeconds(seconds);
    }
}
