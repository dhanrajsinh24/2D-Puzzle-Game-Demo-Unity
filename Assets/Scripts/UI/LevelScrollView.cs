using System;
using System.Collections;
using System.Collections.Generic;
using IG.Controller;
using IG.Level;
using IG.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace IG.UI 
{
    /// <summary>
    /// Level scroll which handles all level lock / unlock visuals
    /// </summary>
    public class LevelScrollView : FadeScreen
    {
        [SerializeField] private Button levelScrollButton;
        [SerializeField] private Button levelButtonPrefab;
        [SerializeField] private Transform contentParent;
        private AddressableLoader _addressableLoader;
        private LevelButton[] _levelButtons;
        private int _lastUnlockedLevel;
        private UIManager _uiManager;
        private bool _isOn;

        private void Awake() 
        {
            _addressableLoader = new AddressableLoader();
        }

        private void OnEnable() 
        {
            LevelManager.OnLevelLoaded += UpdateUnlockedLevelButton;
            LevelManager.OnLevelCompleted += DisableLevelScroll;
        }

        private void OnDisable() 
        {
            LevelManager.OnLevelLoaded -= UpdateUnlockedLevelButton;
            LevelManager.OnLevelCompleted -= DisableLevelScroll;
        }

        protected override void AssignAnimationState()
        {
            animationState = screenAnimation["LevelScroll"]; // Get the animation state
        }

        public void Initialize(UIManager uiManager, int lastUnlockedLevel) 
        {
            _uiManager = uiManager;
            _lastUnlockedLevel = lastUnlockedLevel;
            LoadLevels();
        }

        private void LoadLevels()
        {
            //Load level config addressable with address
            //It would take some time!
            var label = "Level"; 
            _addressableLoader.LoadScriptableObjectsByLabel<LevelConfig>(label, OnLevelsLoaded);
        }

        // Handle multiple objects loading
        private void OnLevelsLoaded(IList<LevelConfig> levelList)
        {
            if (levelList != null)
            {
                _levelButtons = new LevelButton[levelList.Count];
                for (int i = 0; i < levelList.Count; i++) 
                {
                    //Instantiate level button and assign data
                    LevelConfig level = levelList[i];
                    var button = Instantiate(levelButtonPrefab, contentParent);
                    int number = i + 1;
                    button.onClick.AddListener(() => LevelButtonClicked(number));
                    _levelButtons[i] = button.GetComponent<LevelButton>();
                    _levelButtons[i].UpdateLevelText(number);
                    
                    // Assign lock / unlock
                    if(number <= _lastUnlockedLevel) 
                    {
                        _levelButtons[i].UnlockLevel(true);
                    }
                    else 
                    {
                        _levelButtons[i].UnlockLevel(false);
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to load LevelList.");
            }
        }

        private void UpdateUnlockedLevelButton(int level, int _)
        {
            // Level scroll button should be enabled now due to new level load
            levelScrollButton.interactable = true;

            //If the level scroll is not ready, we dont need to update it
            if(_levelButtons == null) return;

            _lastUnlockedLevel = level;
            _levelButtons[level - 1].UnlockLevel(true);
        }

        private void DisableLevelScroll(int _, int __)
        {
            // Level scroll button should be disabled now due to level completion screen
            levelScrollButton.interactable = false;
        } 

        private void LevelButtonClicked(int level) 
        {
            Debug.Log($"Level button {level}");
            ToggleFade();

            StartCoroutine(LoadLevel(level));
        }

        private IEnumerator LoadLevel(int level) 
        {
            _uiManager.LevelManager.DestroyCurrentLevel();
            
            yield return new WaitForSeconds(0.5f);

            _uiManager.LevelManager.LoadLevel(level);
        }

        public void ToggleFade() 
        {
            _isOn = !_isOn;

            if(_isOn) FadeIn();
            else FadeOut();
        }
    }
}
