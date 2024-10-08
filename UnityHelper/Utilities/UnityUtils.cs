using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEngine.LowLevel.PlayerLoopSystem;

namespace UnityHelper.Utilities
{
    /// <summary>
    /// Provides Utility Functions and Extensions for Unity stuff.
    /// </summary>
    public static class UnityUtils
    {
        // Members
        private static Camera _camera;
        private static readonly UniqueRandomNumber randomNumber = new UniqueRandomNumber();

        // Functions
        public static Camera Camera { get { if (!_camera) { _camera = Camera.main; } return _camera; } }

        /// <summary>
        /// Searches the Collection <paramref name="self"/> for an Element and returns it if <paramref name="predicate"/> is True.
        /// </summary>
        /// <typeparam name="T">Element's Type.</typeparam>
        /// <returns></returns>
        public static T Find<T>(this IEnumerable<T> self, Predicate<T> predicate)
        {
            foreach (var item in self)
                if (predicate(item)) return item;

            return default;
        }

        public static Vector3 GetMousePosition()
        {
            return Camera.ScreenToWorldPoint(Input.mousePosition);
        }

        /// <summary>
        /// Updates <paramref name="Function"/> Repeatidly Every Frame While <paramref name="condition"/> is true. 
        /// </summary>
        /// <param name="Function"> The Method to be updated.</param>
        /// <param name="condition"> The Condition to keep updating. </param>
        public static void Update(Action Function, Func<bool> condition, params object[] args)
        {
            _ = UpdateProcess();
            async Task UpdateProcess()
            {
                while (condition())
                {
                    //float time = 1 / Time.deltaTime;
                    Function?.Invoke();
                    await Task.Delay((int)(Time.deltaTime * 1000));
                    //await Task.Yield();
                }
            }

        }

        public static async Task CreateUpdate(Action function, Func<bool> stopCondition)
        {
            while (!stopCondition())
            {
                //Debug.Log("Running...");
                function?.Invoke();
                await Task.Yield();
            }
        }

        /// <summary>
        /// Updates <paramref name="Function"/> Repeatidly Each Frame at a fixed time While <paramref name="condition"/> is true. 
        /// </summary>
        /// <param name="Function"> The Method to be updated.</param>
        /// <param name="condition"> The Condition to keep updating. </param>
        public static void FixedUpdate(Delegate Function, Func<bool> condition)
        {
            UpdateProcess();
            async void UpdateProcess()
            {
                while (condition())
                {
                    float time = 1 / Time.fixedDeltaTime;
                    Function?.DynamicInvoke();
                    await Task.Delay((int)time);
                }
            }

        }

        /// <summary>
        /// Invokes <paramref name="Function"/> after specified <paramref name="time"/>.
        /// </summary>
        /// <param name="Function">Function to be Invoked.</param>
        /// <param name="time">time in Seconds.</param>
        public static void Invoke(Action Function, float time)
        {
            float startTime = Time.time;
            bool done = false;
            FunctionUpdater.Update(() => { if (Time.time - startTime > time) { Function.Invoke(); done = true; } }, () => done);
        }
        public static int GenerateRandomNumber(int minRange, int maxRange)
        {
            return randomNumber.GetRandomNumber(minRange, maxRange);
        }
    }
}
