using System.Collections;
using IG.Controller;
using TMPro;
using UnityEngine;

namespace IG.UI 
{
    public class ResultUI : FadeScreen
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI currentScoreText;

        private void OnEnable() 
        {
            LevelManager.OnLevelCompleted += UpdateUI;
        }

        private void OnDisable() 
        {
            LevelManager.OnLevelCompleted -= UpdateUI;
        }

        protected override void AssignAnimationState()
        {
            animationState = screenAnimation["LevelScroll"]; // Get the animation state
        }

        private void UpdateUI(int level, int score) 
        {
            levelText.text = $"Level {level} Cleared!";
            currentScoreText.text = $"Score: {score}";

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
            FadeOut();
        }
    }
}

