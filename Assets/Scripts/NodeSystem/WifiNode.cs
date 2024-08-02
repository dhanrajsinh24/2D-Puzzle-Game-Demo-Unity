using UnityEngine;

namespace IG.NodeSystem 
{
    public class WiFiNode : Node
    {
        protected override void UpdateVisualFeedback()
        {
            // Wi-Fi node should always have a glow effect
            // Code to enable glow effect
        }

        protected override void NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            //TODO visual and vibrate feedback
        }
    }
}