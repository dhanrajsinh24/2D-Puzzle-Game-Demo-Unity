using System;
using System.Collections.Generic;
using IG.Level;
using IG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace IG.UI 
{
    public class LevelScrollView : MonoBehaviour, IFadeScreen
    {

        [SerializeField] private Button levelButtonPrefab;
        [SerializeField] private Transform contentParent;
        [SerializeField] private CanvasGroup canvasGroup;
        private AddressableLoader _addressableLoader;

        private void Awake() 
        {
            _addressableLoader = new AddressableLoader();
        }

        private void Start() 
        {
            LoadLevels();
        }

        private void LoadLevels() 
        {
            //Load level config addressable with address
            //It would take some time!
            var label = "Level"; 
            _addressableLoader.LoadScriptableObjectsByLabel<LevelConfig>(label, OnLevelsLoaded);
        }

        public void FadeIn() 
        {
            canvasGroup.blocksRaycasts = false;
        }

        public void FadeOut() 
        {
            canvasGroup.blocksRaycasts = true;
        }

        // Handle multiple objects loading
        private void OnLevelsLoaded(IList<LevelConfig> levelList)
        {
            if (levelList != null)
            {
                for (int i = 0; i < levelList.Count; i++) 
                {
                    LevelConfig level = levelList[i];
                    Debug.Log($"{level.name}");
                    var button = Instantiate(levelButtonPrefab, contentParent);
                    int number = i + 1;
                    button.onClick.AddListener(() => LevelButtonClicked(number));
                }
            }
            else
            {
                Debug.LogError("Failed to load LevelList.");
            }
        }

        private void LevelButtonClicked(int level) 
        {
            Debug.Log($"Level button {level}");
            FadeOut();

            // TODO load this level
        }
    }
}
