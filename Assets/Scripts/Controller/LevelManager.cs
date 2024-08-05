using UnityEngine;

namespace IG.Controller 
{
    public class LevelManager : MonoBehaviour
    {
        private int _currentLevel;
        private int _lastUnlockedLevel;
        private const int MaxLevel = 5;

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

            (_lastUnlockedLevel, _currentLevel) = databaseManager.Initialize(this);

            // We will play the level at start which is the last played (Not the last unlocked)
            LoadLevel(databaseManager.GetLastPlayedLevel());
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
            if (level > 0 && level <= MaxLevel)
            {
                _currentLevel = level;

                Debug.Log($"Loading Level {_currentLevel}");
            }
            else
            {
                Debug.LogError("Level to load is not valid.");
            }
        }

        public void CompleteLevel()
        {
            //If we have finished the level for the first time
            if(_currentLevel.Equals(_lastUnlockedLevel)) 
            {
                if (_lastUnlockedLevel < MaxLevel)
                {
                    _lastUnlockedLevel++;
                }
                else
                {
                    Debug.Log("All levels completed!");
                }
            }

            //Save level unlocking and score to database
            databaseManager.SaveLevelData(_currentLevel, scoreManager.CurrentScore);
        }
    }
}
