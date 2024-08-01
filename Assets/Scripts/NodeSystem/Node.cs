using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Core script to manage the Node
    /// </summary>
    public abstract class Node : MonoBehaviour, INode
    {
        public int Row { get; private set; } // row index of current node (to know its place)
        public int Column { get; private set; } // column index same as row
        protected bool isConnected; //True if this node is connected to at least one node
        public RectTransform RectTransform { get; private set; }

        public virtual void Initialize(int row, int column)
        {
            Row = row;
            Column = column;
            RectTransform = GetComponent<RectTransform>();
        }

        public abstract void CheckConnections(); // Must be implemented by derived Node classes

        protected void SetConnectionStatus(bool status)
        {
            isConnected = status;
            UpdateVisualFeedback();
        }

        protected abstract void UpdateVisualFeedback(); // Update the node's visual state
    }
}
