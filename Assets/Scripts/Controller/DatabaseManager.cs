using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IG.Controller
{
    public class DatabaseManager : MonoBehaviour
    {
        private const string JsonFileName = "Level Data";
        private string _path;

        [Serializable]
        public class LevelData
        {
            public string name;
            public int level;
            public int score;
        }

        [Serializable]
        public class StoredData 
        {
            [HideInInspector] public int lastPlayedLevel = 1;
            public List<LevelData> levelDataList = new ();
        }

        [SerializeField]private StoredData storedData;
        private LevelManager _levelManager;

        public void Initialize(LevelManager levelManager)
        {
            _levelManager = levelManager;

            _path = Path.Combine(Application.persistentDataPath, JsonFileName);
            
            // If required json file for database exists at the persistentDataPath then load the data
            // otherwise create the file
            if(File.Exists(_path)) LoadData();
            else WriteData();
        }

        public void SaveLevelData(int level, int score)
        {
            Debug.Log($"Saving data: Level {level}, Score {score}");

            storedData.levelDataList.Add(new LevelData() 
            {
                name = "Level " + level,
                level = level,
                score = score
            });
            
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

            _levelManager.LoadLevel(storedData.lastPlayedLevel);
        }

        public LevelData GetLevelData(int level)
        {
            return storedData.levelDataList.Find(ld => ld.level == level);
        }

        public int GetLevelDataLength()
        {
            return storedData.levelDataList.Count;
        }

        public void UpdateLastPlayedLevel(int level) 
        {
            storedData.lastPlayedLevel = level;
        }
    }
}
