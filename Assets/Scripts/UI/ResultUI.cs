using System.Collections;
using IG.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IG.UI 
{
    /// <summary>
    /// Handles Result screen UI
    /// </summary>
    public class ResultUI : FadeScreen
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private Button confirmButton;
        private UIManager _uiManager;

        private void OnEnable() 
        {
            LevelManager.OnLevelCompleted += UpdateUI;
        }

        private void OnDisable() 
        {
            LevelManager.OnLevelCompleted -= UpdateUI;
        }
        
        public void Initialize(UIManager uiManager) 
        {
            _uiManager = uiManager;
        }

        protected override void AssignAnimationState()
        {
            animationState = screenAnimation["ScreenFade"]; // Get the animation state
        }

        private void UpdateUI(int level, int score) 
        {
            currentScoreText.text = $"Score: {score}";
            if(level.Equals(LevelManager.MaxLevel)) 
            {
                levelText.text = $"Last Level Cleared!";
                confirmButton.gameObject.SetActive(false);
            }
            else 
            {
                levelText.text = $"Level {level} Cleared!";
                confirmButton.interactable = true;
            }

            StartCoroutine(ShowResultScreen());
        }

        private IEnumerator ShowResultScreen() 
        {
            ToggleRingAnimation(false); 
            yield return new WaitForSeconds(3f);
            FadeIn();
        }

        public override void FadeOut()
        {
           base.FadeOut();

           ToggleRingAnimation(true);  
        }

        public void OnConfirmClicked() 
        {
            confirmButton.interactable = false;
            FadeOut();

            StartCoroutine(NextLevel());
        }

        private IEnumerator NextLevel() 
        {
            yield return new WaitForSeconds(0.5f);
            _uiManager.LevelManager.LoadNextLevel();
        }
    }
}

