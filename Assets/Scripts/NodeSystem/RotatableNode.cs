using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Script enables the node to Rotate on click
    /// </summary>
    public class RotatableNode : Node, IRotatable
    {
        public override void NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            Rotate();
        }

        public void Rotate()
        {
            // TODO Rotate animation
            // Rotate the Node by 90 degrees clockwise
            transform.Rotate(0, 0, -90);

            ShiftConnectibleSides();

            CheckConnections();
        }
    }
}
