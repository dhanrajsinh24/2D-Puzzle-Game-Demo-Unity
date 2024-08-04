using UnityEngine;

namespace IG.Controller 
{
    public class LevelManager : MonoBehaviour
    {
        private int _currentLevel;
        private int _lastUnlockedLevel = 1;
        private const int maxLevel = 5;

        [SerializeField] private DatabaseManager databaseManager;
        [SerializeField] private ScoreManager scoreManager;

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

            databaseManager.Initialize(this);

            LoadLevel(_lastUnlockedLevel);
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
            // Making sure the level being loaded is within the valid range
            if (level > 0 && level <= maxLevel)
            {
                _currentLevel = level;

                Debug.Log($"Loading Level {_currentLevel}");
            }
            else
            {
                Debug.LogError("Level to load is not valid.");
            }
        }

        private void CompleteLevel()
        {
            //If we have finished the level for the first time
            if(_currentLevel.Equals(_lastUnlockedLevel)) 
            {
                if (_lastUnlockedLevel < maxLevel)
                {
                    _lastUnlockedLevel++;
                    LoadLevel(_lastUnlockedLevel);
                }
                else
                {
                    Debug.Log("All levels completed!");
                }
            }

            //Save level unlocking and score to database
            databaseManager.SaveLevelData(_lastUnlockedLevel, scoreManager.CurrentScore);
        }
    }
}
