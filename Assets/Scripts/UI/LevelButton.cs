using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IG.UI 
{
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button levelButton;
        
        public void UpdateLevelText(int level) 
        {
            levelText.text = $"Level {level}";
        }

        public void UnlockLevel(bool enable) 
        {
            levelButton.interactable = enable;
        }
    }
}

