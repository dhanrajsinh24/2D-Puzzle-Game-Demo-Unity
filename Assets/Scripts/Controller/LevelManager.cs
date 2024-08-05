using System;
using IG.Level;
using IG.Utils;
using UnityEngine;

namespace IG.Controller 
{
    public class LevelManager : MonoBehaviour
    {
        private const int MaxLevel = 3;

        [SerializeField] private DatabaseManager databaseManager;

        private LevelConfig _currentLevelConfig; 
        [SerializeField] private SpriteNodeGrid gridParentPrefab; // Grid parent to initialize on level load
        
        private int _currentLevel;
        private int _playerMoves;
        private SpriteNodeGrid _currentGridParent;
        private ScoreManager _scoreManager;
        private AddressableLoader _addressableLoader;

        public static Action OnLevelLoaded;
        public static Action OnLevelCompleted;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize() 
        {
            if (databaseManager == null)
            {
                databaseManager = FindObjectOfType<DatabaseManager>();
            }
            _currentLevel = databaseManager.Initialize(this);

            _scoreManager = new ScoreManager();
            _addressableLoader = new AddressableLoader();

            // We will play the level at start which is the last played (Not the last unlocked)
            LoadLevel(_currentLevel);
        }

        private void OnEnable() 
        {
            CircuitValidation.OnValidated += CompleteLevel;
        }

        private void OnDisable() 
        {
            CircuitValidation.OnValidated += CompleteLevel;
        }

        public void LoadLevel(int level)
        {
            //If any level loaded previously then delete the grid
            if(_currentGridParent) 
            {
                Destroy(_currentGridParent.gameObject);
            }
            
            // Making sure the level being loaded is within the valid range
            if (level > 0 && level <= MaxLevel && level <= databaseManager.LastUnlockedLevel)
            {
                _currentLevel = level;

                Debug.Log($"Loading Level {_currentLevel}");

                //Load level config addressable with address
                //It would take some time!
                var address = $"Level {_currentLevel}"; 
                _addressableLoader.LoadScriptableObject<LevelConfig>(address, OnLevelConfigLoaded);
            }
            else
            {
                Debug.LogError($"Level {level} is not valid.");
            }
        }

        // Callback method to handle the loaded object
        private void OnLevelConfigLoaded(LevelConfig levelConfig)
        {
            if (levelConfig != null)
            {
                _currentLevelConfig = levelConfig;

                //Initialize grid of nodes for this level
                _currentGridParent = Instantiate(gridParentPrefab);
                _currentGridParent.Initialize(_currentLevelConfig);

                OnLevelLoaded?.Invoke();
            }
            else
            {
                Debug.LogError("Failed to load LevelConfig.");
            }
        }

        public void CompleteLevel()
        {
            //Cache last unlocked level
            var lastUnlockedLevel = databaseManager.LastUnlockedLevel;
            Debug.Log($"last unlocked {lastUnlockedLevel}");

            //Save level data, unlocking new level, and score to database
            var score = _scoreManager.CalculateScore(
                _currentLevelConfig.minMoves, _currentLevelConfig.maxMoves, _playerMoves);
            Debug.Log($"Level {_currentLevel}, Score {score}");
            databaseManager.SaveLevelData(_currentLevel, score); 

            //If we have finished the level for the first time
            // Check for next level availability
            if(_currentLevel.Equals(lastUnlockedLevel) && lastUnlockedLevel.Equals(MaxLevel)) 
            {
                Debug.Log("Last level");
                // TODO need to disable next level button or show UI for 
                // All levels are completed
            }
            else 
            {
                // TODO need to load next level
                int levelToLoad = _currentLevel + 1;
                LoadLevel(levelToLoad);
            }
        }
    }
}
