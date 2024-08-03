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

        // Shift the connectableSides array based on grid type
        private void ShiftConnectibleSides()
        {
            var size = (int)gridType;
            bool lastSide = ConnectableSides[size - 1];
            for (int i = size - 1; i > 0; i--)
            {
                ConnectableSides[i] = ConnectableSides[i - 1];
            }
            ConnectableSides[0] = lastSide;
        }
    }
}
