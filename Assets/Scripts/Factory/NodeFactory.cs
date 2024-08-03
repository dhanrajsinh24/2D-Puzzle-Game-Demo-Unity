using IG.Controller;
using IG.Level;
using IG.NodeSystem;
using UnityEngine;

namespace IG.Factory
{
    public static class NodeFactory
    {
        public static Node CreateNode(LevelConfig.NodeData nodeData, Transform parent, 
        int row, int column, LevelConfig.GridType gridType, GridManager gridManager)
        {
            if(nodeData.nodeType.Equals(LevelConfig.NodeType.EmptyNode))
            {
                Debug.Log($"Empty at position ({row}, {column})");
                
                // Instantiate the Empty node to fill the array space
                Object.Instantiate(nodeData.nodePrefab, parent);

                return null;
            }
            
            // Instantiate the Node
            var nodeObj = Object.Instantiate(nodeData.nodePrefab, parent);
            var node = nodeObj.GetComponent<Node>();

            // We have to take the clone of array so it does not change the base nodeData of Level
            var connectableSides = (bool[])nodeData.connectableSides.Clone();

            // Initialize the node with its data
            node.Initialize(row, column, connectableSides);

            // Add rotation ability to all nodes except Wifi
            if (nodeData.nodeType != LevelConfig.NodeType.WiFiNode)
            {
                var rotatableNode = nodeObj.GetComponent<RotatableNode>();
                rotatableNode.Initialize(gridType, gridManager);
            }

            return node;
        }
    }
}