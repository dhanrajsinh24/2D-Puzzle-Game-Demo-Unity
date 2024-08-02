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
            // Instantiate the prefab
            var nodeObj = Object.Instantiate(nodeData.nodePrefab, parent);
            var node = nodeObj.GetComponent<Node>();

            // Initialize the node with its data
            node.Initialize(row, column, nodeData.connectableSides);

            // Add rotation ability to all nodes except Wifi
            if (nodeData.nodeType != LevelConfig.NodeType.WiFiNode)
            {
                var rotatableNode = nodeObj.GetComponent<RotatableNode>();
                rotatableNode.Initialize(gridType, gridManager);
            }

            // Add other node types and their specific components here if needed

            return node;
        }
    }
}