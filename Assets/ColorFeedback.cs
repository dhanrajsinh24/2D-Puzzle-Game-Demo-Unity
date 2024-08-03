using UnityEngine;
using UnityEngine.UI;

namespace IG.NodeSystem 
{
    public class ColorFeedback : MonoBehaviour
    {
        [SerializeField]private Image[] images;

        public void ToggleGlow(bool enable) 
        {
            var color = enable ? Color.green : Color.white;

            foreach(Image image in images) 
            {
                image.color = color;
            }
        }
    }
}

