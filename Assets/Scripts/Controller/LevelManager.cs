using System;
using IG.Command;
using IG.Level;
using IG.Utils;
using UnityEngine;

namespace IG.Controller 
{
    /// <summary>
    /// Manages level loading, progression, and score calculation.
    /// Initializes all required classes
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        public const int MaxLevel = 4;

        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SpriteGridManager gridParentPrefab; // Grid parent to initialize on level load
        
        private int _currentLevel;
        private SpriteGridManager _currentGridParent;
        private ScoreManager _scoreManager;
        private AddressableLoader _addressableLoader;
        private LevelConfig _currentLevelConfig; 
        public static Action<int, int> OnLevelLoaded; //Called When the level loaded with level number, top score
        public static Action<int, int> OnLevelCompleted; //Called when a level is completed with level number, current score

        private void Awake()
        {
            //Auto initialization of this main manager script of the game
            Initialize();
        }

        private void Initialize() 
        {
            // Set maximum target framerate
            Application.targetFrameRate = 60;

            if (databaseManager == null)
            {
                databaseManager = FindObjectOfType<DatabaseManager>();
            }
            _currentLevel = databaseManager.Initialize(this);

            // Initialize UIManager which handles UI communication
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
            }
            uiManager.Initialize(this, databaseManager);

            _scoreManager = new ScoreManager();
            _addressableLoader = new AddressableLoader();

            // Add required scripts to handle node clicking
            var nodeClickManager = gameObject.AddComponent<NodeClickManager>();
            var clickCommandExecutor = gameObject.AddComponent<ClickCommandExecutor>();
            nodeClickManager.Initialize(clickCommandExecutor, _scoreManager);
        }

        private void OnEnable() 
        {
            CircuitValidation.OnValidated += CompleteLevel;
        }

        private void OnDisable() 
        {
            CircuitValidation.OnValidated += CompleteLevel;
        }

        private void Start() 
        {
            // We will play the level at start which is the last played (Not the last unlocked)
            LoadLevel(_currentLevel);
        }

        public void DestroyCurrentLevel() 
        {
             _scoreManager.PlayerMoves = 0;

            //If any level loaded previously then delete the grid
            if(_currentGridParent) 
            {
                Destroy(_currentGridParent.gameObject);
            }
        }

        public void LoadLevel(int level)
        {
            DestroyCurrentLevel();

            // Making sure the level being loaded is within the valid range
            if (level > 0 && level <= MaxLevel 
                && level <= databaseManager.LastUnlockedLevel)
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

        public void LoadNextLevel() 
        {
            _currentLevel++;
            LoadLevel(_currentLevel);
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

                //Event called to update UI and others
                var levelData = databaseManager.GetLevelData(_currentLevel);
                var topScore = levelData != null ? levelData.topScore : 0;
                OnLevelLoaded?.Invoke(_currentLevel, topScore);
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
            var minMoves = _currentLevelConfig.minMoves;
            var maxMoves = _currentLevelConfig.maxMoves;
            var score = _scoreManager.CalculateScore(minMoves, maxMoves);
            Debug.Log($"Level {_currentLevel}, Score {score}");
            databaseManager.SaveLevelData(_currentLevel, score); 

            //If we have finished the level for the first time
            // Check for next level availability
            if(_currentLevel.Equals(lastUnlockedLevel) 
                && lastUnlockedLevel.Equals(MaxLevel)) 
            {
                Debug.Log("Last level");
            }

            OnLevelCompleted?.Invoke(_currentLevel, score);
        }
    }
}
