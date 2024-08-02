using UnityEngine;
using UnityEngine.EventSystems;

namespace IG.NodeSystem 
{
    public class ComputerNode : Node, IResultNode
    {
        protected override void UpdateVisualFeedback()
        {
            // Update the node's visual state based on connection status
            if (isConnected)
            {
                // Code to enable glow effect
            }
            else
            {
                // Code to disable glow effect
            }
        }

        protected override void NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
        }
    }
}