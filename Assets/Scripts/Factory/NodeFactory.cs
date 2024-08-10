using IG.Controller;
using IG.NodeSystem;
using UnityEngine;
using static IG.Level.LevelConfig;

namespace IG.Factory
{
    /// <summary>
    /// Handles creating Various nodes.
    /// Attached to Grid object.
    /// </summary>
    public class NodeFactory : MonoBehaviour
    {
        private IGrid _gridManager;
        private Transform _parent;
        private GridType _gridType;
        private CircuitValidation _circuitValidation;

        public void Initialize(IGrid gridManager, Transform parent, GridType gridType, CircuitValidation circuitValidation) 
        {
            _gridManager = gridManager;
            _parent = parent;
            _gridType = gridType;
            _circuitValidation = circuitValidation;
        }

        public Node CreateNode(NodeData nodeData, int row, int column)
        {
            if(nodeData.nodeType.Equals(NodeType.EmptyNode))
            {
                Debug.Log($"Empty at position ({row}, {column})");
                
                // Instantiate the Empty node to fill the array space
                Instantiate(nodeData.nodePrefab, _parent);

                return null;
            }
            
            // Instantiate the Node
            var nodeObj = Instantiate(nodeData.nodePrefab, _parent);
            var node = nodeObj.GetComponent<Node>();

            var initialConnectibleSides = nodeObj.GetComponent<InitialConnectibleSides>();
            // We have to take the clone of array so it does not change the original
            // and it creates another copy so we can now destroy the component
            var connectableSides = (bool[])initialConnectibleSides.connectibleSides.Clone();
            Object.Destroy(initialConnectibleSides);
            
            var initialRotation = nodeData.initialRotation;
            
            // Initialize the node with its data and apply the required rotation
            node.Initialize(row, column, connectableSides, _gridManager, _gridType);
            node.ApplyInitialRotation(initialRotation);

            if(node is IResultNode) (node as IResultNode).AssignAdditional(_circuitValidation);

            return node;
        }
    }
}