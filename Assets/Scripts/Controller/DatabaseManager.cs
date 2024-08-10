using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IG.Controller
{
    /// <summary>
    /// Handles Json database management for level progress and score.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        private const string JsonFileName = "Level Data";
        private string _path;

        [Serializable]
        public class LevelData
        {
            public string name;
            public int level;
            public int topScore;
        }

        [Serializable]
        public class StoredData 
        {
            [HideInInspector] public int lastPlayedLevel = 1;
            public List<LevelData> levelDataList = new ();
        }

        [SerializeField]private StoredData storedData;
        private LevelManager _levelManager;

        public int LastUnlockedLevel
        {
            get 
            { 
                // last unlocked level is the level which is not finished 
                // so it's data will not be in the database
                return storedData.levelDataList.Count + 1;
            }
        }

        //Get the last played level from stored data
        public int LastPlayedLevel
        {
            get 
            { 
                return storedData.lastPlayedLevel;
            }
            set 
            {
                storedData.lastPlayedLevel = value;
                WriteData();
            }
        }

        private void OnEnable() 
        {
            LevelManager.OnLevelLoaded += UpdateLastPlayedLevel;
        }

        private void OnDisable() 
        {
            LevelManager.OnLevelLoaded -= UpdateLastPlayedLevel;
        }

        /// <summary>
        /// Initialize all data and returns last played level
        /// </summary>
        /// <param name="levelManager"></param>
        /// <returns></returns>
        public int Initialize(LevelManager levelManager)
        {
            _levelManager = levelManager;

            _path = Path.Combine(Application.persistentDataPath, JsonFileName);
            
            // If required json file for database exists at the persistentDataPath then load the data
            // otherwise create the file
            if(File.Exists(_path)) LoadData();
            else WriteData();

            return storedData.lastPlayedLevel;
        }

        public void SaveLevelData(int level, int score)
        {
            Debug.Log($"Saving data: Level {level}, Score {score}");

            var levelData = GetLevelData(level);

            if(levelData != null) 
            {
                if(levelData.topScore < score)
                    levelData.topScore = score;
            }
            else 
            {
                storedData.levelDataList.Add(new LevelData() 
                {
                    name = "Level " + level,
                    level = level,
                    topScore = score
                });
            }
            
            WriteData();
        }

        // Writes data to json file. If file does not exists, it creates
        private void WriteData()
        {
            Debug.Log("Writing Data..");
            var dataJson = JsonUtility.ToJson(storedData);
            File.WriteAllText(_path, dataJson);
        }

        // Loads the data from json file
        private void LoadData()
        {
            Debug.Log("Loading Data..");
            var jsonData = File.ReadAllText(_path);
            storedData = JsonUtility.FromJson<StoredData>(jsonData);
        }

        public LevelData GetLevelData(int level)
        {
            if(storedData.levelDataList == null) return null;
            return storedData.levelDataList.Find(ld => ld.level == level);
        }

        private void UpdateLastPlayedLevel(int level, int _) 
        {
            //Update last played level
            LastPlayedLevel = level;
        }
    }
}
