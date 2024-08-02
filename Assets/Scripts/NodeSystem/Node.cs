using IG.Controller;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Core script to manage the Node
    /// </summary>
    public abstract class Node : MonoBehaviour, IConnectable, IPointerClickHandler
    {
        public int Row { get; private set; } // row index of current node (to know its place)
        public int Column { get; private set; } // column index same as row
        public bool[] ConnectableSides { get; private set; } // Sides that can connect (size 4 for square, 6 for hexagonal)
        protected bool isConnected; //True if this node is connected to at least one node
        public RectTransform RectTransform { get; private set; }

        public virtual void Initialize(int row, int column, bool[] initialConnectableSides)
        {
            Row = row;
            Column = column;
            RectTransform = GetComponent<RectTransform>();
            ConnectableSides = initialConnectableSides;
        }

        public void SetConnectionStatus(bool status)
        {
            isConnected = status;
            UpdateVisualFeedback();
        }

        protected abstract void UpdateVisualFeedback(); // Update the node's visual state

        public abstract void NodeClicked();

        public void OnPointerClick(PointerEventData eventData)
        {
            NodeClicked();
        }
    }
}
