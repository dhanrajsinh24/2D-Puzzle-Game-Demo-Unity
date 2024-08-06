using IG.Controller;
using TMPro;
using UnityEngine;

namespace IG.UI 
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI topScoreText;

        private void OnEnable() 
        {
            LevelManager.OnLevelLoaded += EnableButton;
        }

        private void OnDisable() 
        {
            LevelManager.OnLevelLoaded -= EnableButton;
        }


        public void EnableButton(int _, int topScore) 
        {
            UpdateTopScore(topScore);
        }

        private void UpdateTopScore(int score) 
        {
            topScoreText.text = $"Top Score: {score}";
        }
    }
}

