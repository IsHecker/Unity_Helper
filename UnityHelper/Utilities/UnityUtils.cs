using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Threading.Tasks;

namespace UnityHelper.Utilities
{
    /// <summary>
    /// Provides Utility Functions and Extensions for Unity stuff.
    /// </summary>
    public static class UnityUtils
    {
        // Members
        private static Camera _camera;
        private readonly static Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();
        private static readonly UniqueRandomNumber randomNumber = new UniqueRandomNumber();

        // Functions
        public static Camera Camera { get { if (!_camera) { _camera = Camera.main; } return _camera; } }
        public static WaitForSeconds WaitFor(float seconds) => waitDictionary.TryGetValue(seconds, out var wait) ? wait : waitDictionary[seconds] = new WaitForSeconds(seconds);
        public static bool IsOverUI(Touch touch)
        {
            if (touch.phase != TouchPhase.Began)
                return false;

            return !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
        }
        public static GameObject GetPressedUI() => EventSystem.current.currentSelectedGameObject;
        public static Vector3 CanvasPositionToWorldPosition(RectTransform canvas)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, canvas.position, Camera, out var result);
            return result;
        }
        public static void TransitionUI(GameObject currentUI, GameObject targetUI, Animator animator, string boolName, bool state, float duration)
        {
            targetUI.SetActive(true);
            animator.SetBool(boolName, state);

            if (currentUI.name != "SelectMode_UI")
                Invoke(() => currentUI.SetActive(false), duration);
        }
        public static void TransitionToUI(this GameObject currentUI, GameObject targetUI, Animator animator, string boolName, bool state, float duration)
        {
            //Just Extension function of TransitionUI()
            TransitionUI(currentUI, targetUI, animator, boolName, state, duration);
        }
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

        /// <summary>
        /// Updates <paramref name="Function"/> Repeatidly Every Frame While <paramref name="condition"/> is true. 
        /// </summary>
        /// <param name="Function"> The Method to be updated.</param>
        /// <param name="condition"> The Condition to keep updating. </param>
        public static void Update(Delegate Function, Func<bool> condition, params object[] args)
        {
            UpdateProcess();
            async void UpdateProcess()
            {
                while (condition())
                {
                    //float time = 1 / Time.deltaTime;
                    Function?.DynamicInvoke(args);
                    //await Task.Delay((int)time);
                    await Task.Yield();
                }
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
            float startTime = Time.unscaledTime;
            bool done = false;
            Update(new Action(() => { if (Time.unscaledTime >= startTime + time) { Function?.Invoke(); done = true; } }), () => !done);
        }
        /// <summary>
        /// Deletes all the children of <paramref name="transform"/>.
        /// </summary>
        public static void DeleteChildren(this Transform transform)
        {
            foreach (Transform child in transform) UnityEngine.Object.Destroy(child.gameObject);
        }
        public static int GenerateRandomNumber(int minRange, int maxRange)
        {
            return randomNumber.GetRandomNumber(minRange, maxRange);
        }
    }
}
