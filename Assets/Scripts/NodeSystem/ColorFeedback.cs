using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Handles visual feedback for the nodes
    /// </summary>
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

