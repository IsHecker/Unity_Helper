using UnityEngine;
using UnityHelper;
using UnityEditor;

namespace Assets.Unity_Helper
{
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
            //Debug.Log("OnPlayModeStateChanged!!!");
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
