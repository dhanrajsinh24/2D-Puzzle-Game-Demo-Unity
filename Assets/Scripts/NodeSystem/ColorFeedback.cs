using UnityEngine;

namespace IG.NodeSystem 
{
    public class ColorFeedback : MonoBehaviour
    {
        [SerializeField]private SpriteRenderer[] images;

        public void ToggleGlow(bool enable) 
        {
            var color = enable ? Color.green : Color.white;

            foreach(SpriteRenderer image in images) 
            {
                image.color = color;
            }
        }
    }
}

