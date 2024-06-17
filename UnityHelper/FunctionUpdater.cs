using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityHelper
{
    public class FunctionUpdater
    {
        private static readonly Dictionary<string, FunctionActiveState> updateFunctions = new();

        /// <summary>
        /// Updates <paramref name="function"/> Repeatidly Every Frame While <paramref name="stopCondition"/> is true. 
        /// </summary>
        /// <param name="function"> The Method to be updated.</param>
        /// <param name="stopCondition"> The Condition to keep updating. </param>
        public static void Update(string actionName, Action function, Func<bool> stopCondition)
        {
            if (!updateFunctions.TryAdd(actionName, null)) 
            {
                Stop(actionName);
            }

            _ = CreateUpdate(actionName, function, stopCondition);
        }

        public static void Stop(string actionName)
        {
            if (!updateFunctions.TryGetValue(actionName, out var func))
                return;
            func.Value = false;
        }

        public static void StopAll()
        {
            foreach (var (key, state) in updateFunctions)
            {
                state.Value = false;
                updateFunctions.Remove(key);
            }
        }

        private static async Task CreateUpdate(string actionName, Action function, Func<bool> stopCondition)
        {
            FunctionActiveState activeState = new FunctionActiveState { Value = true };

            updateFunctions[actionName] = activeState;


            while (!stopCondition() && activeState.Value)
            {
                Debug.Log("Running...");
                function.Invoke();
                await Task.Yield();
            }

            updateFunctions.Remove(actionName);
        }


        private unsafe class BoolPtr
        {
            private readonly bool* ptr;
            public BoolPtr(bool* value)
            {
                ptr = value;
            }
            public void SetPtr(bool value) => *ptr = value;
            public bool GetPtr() => *ptr;
        }

        private class FunctionActiveState
        {
            public bool Value { get; set; }
        }
    }
}
