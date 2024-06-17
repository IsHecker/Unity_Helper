using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityHelper.Templates;

namespace UnityHelper
{
    public class GameManager : SingletonMono<GameManager>
    {
        // Game State
        public enum GameState { MainMenu, Playing, Paused, GameOver }
        public GameState CurrentGameState { get; private set; }

        // Persistent Data
        public int PlayerScore { get; private set; }
        public int PlayerLives { get; private set; }

        // Settings
        public float MasterVolume { get; private set; }
        public float MusicVolume { get; private set; }
        public float SFXVolume { get; private set; }
        public bool IsFullScreen { get; private set; }
        public int ScreenResolutionIndex { get; private set; }

        // Event System
        public delegate void GameStateChanged(GameState newState);
        public event GameStateChanged OnGameStateChanged;

        // Object Pooling
        private Dictionary<string, Queue<GameObject>> objectPools;

        private void Initialize()
        {
            PlayerScore = 0;
            PlayerLives = 3;
            MasterVolume = 1f;
            MusicVolume = 1f;
            SFXVolume = 1f;
            IsFullScreen = true;
            ScreenResolutionIndex = 0;
            objectPools = new Dictionary<string, Queue<GameObject>>();
            CurrentGameState = GameState.MainMenu;
        }

        // Game State Management
        public void ChangeGameState(GameState newState)
        {
            CurrentGameState = newState;
            OnGameStateChanged?.Invoke(newState);
        }

        // Score Management
        public void AddScore(int points)
        {
            PlayerScore += points;
            // Update UI or other game elements
        }

        public void ResetScore()
        {
            PlayerScore = 0;
        }

        // Lives Management
        public void LoseLife()
        {
            PlayerLives--;
            if (PlayerLives <= 0)
            {
                ChangeGameState(GameState.GameOver);
            }
        }

        public void ResetLives()
        {
            PlayerLives = 3;
        }

        // Settings Management
        public void SetMasterVolume(float volume)
        {
            MasterVolume = volume;
            // Apply to audio system
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            // Apply to music audio source
        }

        public void SetSFXVolume(float volume)
        {
            SFXVolume = volume;
            // Apply to SFX audio sources
        }

        public void SetFullScreen(bool isFullScreen)
        {
            IsFullScreen = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }

        public void SetScreenResolution(int index)
        {
            ScreenResolutionIndex = index;
            // Assume resolutions are stored in a predefined array
            Resolution[] resolutions = Screen.resolutions;
            if (index >= 0 && index < resolutions.Length)
            {
                Screen.SetResolution(resolutions[index].width, resolutions[index].height, IsFullScreen);
            }
        }

        // Level Management
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }

        // Pause and Resume
        public void PauseGame()
        {
            ChangeGameState(GameState.Paused);
            Time.timeScale = 0f; // Freeze the game
                                 // Show pause menu
        }

        public void ResumeGame()
        {
            ChangeGameState(GameState.Playing);
            Time.timeScale = 1f; // Unfreeze the game
                                 // Hide pause menu
        }

        // Object Pooling
        public void CreatePool(string tag, GameObject prefab, int poolSize)
        {
            if (!objectPools.ContainsKey(tag))
            {
                objectPools[tag] = new Queue<GameObject>();
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.SetActive(false);
                    objectPools[tag].Enqueue(obj);
                }
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!objectPools.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
                return null;
            }

            GameObject objToSpawn = objectPools[tag].Dequeue();
            objToSpawn.SetActive(true);
            objToSpawn.transform.position = position;
            objToSpawn.transform.rotation = rotation;

            objectPools[tag].Enqueue(objToSpawn);

            return objToSpawn;
        }
    }

}