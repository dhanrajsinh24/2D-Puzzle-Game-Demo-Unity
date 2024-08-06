using System;
using IG.Level;
using IG.Utils;
using UnityEngine;

namespace IG.Controller 
{
    public class LevelManager : MonoBehaviour
    {
        private const int MaxLevel = 4;

        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private UIManager uiManager;

        private LevelConfig _currentLevelConfig; 
        [SerializeField] private SpriteNodeGrid gridParentPrefab; // Grid parent to initialize on level load
        
        private int _currentLevel;
        private SpriteNodeGrid _currentGridParent;
        private ScoreManager _scoreManager;
        private AddressableLoader _addressableLoader;

        public static Action<int, int> OnLevelLoaded; //Called When the level loaded with level number, top score
        public static Action<int, int> OnLevelCompleted; //Called when a level is completed with level number, current score

        private void Awake()
        {
            //Auto initialization of this main manager script of the game
            Initialize();
        }

        private void Initialize() 
        {
            Application.targetFrameRate = 60;

            if (databaseManager == null)
            {
                databaseManager = FindObjectOfType<DatabaseManager>();
            }
            _currentLevel = databaseManager.Initialize(this);

            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
            }
            uiManager.Initialize(this, databaseManager);

            _scoreManager = new ScoreManager();
            var nodeClickManager = gameObject.AddComponent<NodeClickManager>();
            nodeClickManager.Initialize(_scoreManager);

            _addressableLoader = new AddressableLoader();
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
                // TODO need to disable next level button or show UI for 
                // All levels are completed
            }
            else 
            {
                // TODO need to load next level
                int levelToLoad = _currentLevel + 1;
                //LoadLevel(levelToLoad);
            }

            OnLevelCompleted?.Invoke(_currentLevel, score);
        }
    }
}
