using System;
using IG.Level;
using UnityEngine;

namespace IG.Controller 
{
    public class LevelManager : MonoBehaviour
    {
        private const int MaxLevel = 5;

        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private ScoreManager scoreManager;

        /// <summary>
        /// All levels (Only a few levels should be cached when there will be more)
        /// </summary>
        [SerializeField] private LevelConfig[] levelConfigs; 
        [SerializeField] private SpriteNodeGrid gridParentPrefab; // Grid parent to initialize on level load
        
        private int _currentLevel;
        
        private SpriteNodeGrid _currentGridParent;

        public static Action OnLevelLoaded;
        public static Action OnLevelCompleted;

        private void Awake()
        {
            if (databaseManager == null)
            {
                databaseManager = FindObjectOfType<DatabaseManager>();
            }

            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<ScoreManager>();
            }

            if(!levelConfigs.Length.Equals(MaxLevel)) 
            {
                Debug.LogError($"Level config must have {MaxLevel} members!");
            }

            _currentLevel = databaseManager.Initialize(this);

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
                Destroy(_currentGridParent);
            }
            
            // Making sure the level being loaded is within the valid range
            if (level > 0 && level <= MaxLevel && level <= databaseManager.LastUnlockedLevel)
            {
                _currentLevel = level;

                Debug.Log($"Loading Level {_currentLevel}");

                //Initialize grid of nodes for this level
                _currentGridParent = Instantiate(gridParentPrefab);
                _currentGridParent.Initialize(levelConfigs[_currentLevel - 1]);

                OnLevelLoaded?.Invoke();
            }
            else
            {
                Debug.LogError($"Level {level} is not valid.");
            }
        }

        public void CompleteLevel()
        {
            var lastUnlockedLevel = databaseManager.LastUnlockedLevel;
            //If we have finished the level for the first time
            if(_currentLevel.Equals(lastUnlockedLevel)) 
            {
                if (lastUnlockedLevel.Equals(MaxLevel))
                {
                    Debug.Log("Last level");
                    // TODO need to disable next level button or show UI for 
                    // All levels are completed
                }
                else 
                {
                    // TODO need to load next level
                }
            }

            //Save level data, unlocking new level, and score to database
            databaseManager.SaveLevelData(_currentLevel, scoreManager.CurrentScore);
        }
    }
}
