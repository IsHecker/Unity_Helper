using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace UnityHelper
{
    public class FunctionUpdater
    {
        private static readonly Dictionary<int, UpdateProcess> updateFunctions = new();
        
        /// <summary>
        /// Updates <paramref name="function"/> Repeatidly Every Frame While <paramref name="stopCondition"/> is true. 
        /// </summary>
        /// <param name="function"> The Method to be updated.</param>
        /// <param name="stopCondition"> The Condition to keep updating. </param>
        public static void Update(int functionId, Action function, Func<bool> stopCondition)
        {
            updateFunctions.TryAdd(functionId, new UpdateProcess(function, stopCondition));
        }

        public static void Stop(int functionId)
        {
            if (!updateFunctions.TryGetValue(functionId, out UpdateProcess process))
                return;
            process.Stop();
            updateFunctions.Remove(functionId);
        }

        public static void StopAll()
        {
            foreach (var (functionId, process) in updateFunctions)
            {
                if (!updateFunctions.ContainsKey(functionId))
                    continue;
                process.Stop();
                    updateFunctions.Remove(functionId);
            }
        }

        private class UpdateProcess
        {
            private bool stop = false;

            public UpdateProcess(Action function, Func<bool> stopCondition)
            {
                Update();
                async void Update()
                {
                    while (!stopCondition() && IsRunning())
                    {
                        function?.Invoke();
                        await Task.Yield();
                    }
                }
            }
            private bool IsRunning() => !stop;
            public void Stop() => stop = true;
        }
    }


    public static class GameLifecycleHandler
    {
        public delegate void GameStoppedEventHandler();
        public static event GameStoppedEventHandler GameStopped = FunctionUpdater.StopAll;

        private static bool isGameRunning = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            Application.wantsToQuit += WantsToQuit;
#endif
        }

#if UNITY_EDITOR
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            Debug.Log("OnPlayModeStateChanged!!!");
            isGameRunning = false;
            OnGameStopped();
        }
    }
#else
        private static bool WantsToQuit()
        {
            isGameRunning = false;
            OnGameStopped();
            return true; // Return true to allow application to quit
        }
#endif

        public static void StopGame()
        {
            isGameRunning = false;
            OnGameStopped();
        }

        private static void OnGameStopped()
        {
            GameStopped?.Invoke();
        }

        public static bool IsGameRunning()
        {
            return isGameRunning;
        }
    }
}
