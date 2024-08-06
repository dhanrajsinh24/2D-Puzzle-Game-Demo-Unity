using IG.Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IG.UI 
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI topScoreText;
        [SerializeField] private Button backgroundClickButton;

        private void OnEnable() 
        {
            CircuitValidation.OnValidated += DisableButton;
            LevelManager.OnLevelLoaded += EnableButton;
        }

        private void OnDisable() 
        {
            CircuitValidation.OnValidated -= DisableButton;
            LevelManager.OnLevelLoaded -= EnableButton;
        }

        private void ToggleButton(bool enable) 
        {
            backgroundClickButton.interactable = enable;
        }

        public void DisableButton() 
        {
            ToggleButton(false);
        }

        public void EnableButton(int _, int topScore) 
        {
            ToggleButton(true);
            UpdateTopScore(topScore);
        }

        private void UpdateTopScore(int score) 
        {
            topScoreText.text = $"Top Score: {score}";
        }
    }
}

