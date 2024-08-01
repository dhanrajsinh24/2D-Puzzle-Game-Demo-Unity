using IG.Level;
using IG.NodeSystem;
using UnityEngine;
using UnityEngine.UI;

namespace IG.Controller 
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup; // Grid parent
        [SerializeField] private LevelConfig levelConfig; // Level data will be used from this config
        private Node[,] _nodeGrid; // All nodes in the grid are stored here

        private void Start()
        {
            // Auto Initialize
            Initialize();
        }

        private void Initialize()
        {
            levelConfig.Initialize(gridLayoutGroup);
            GenerateGrid();
        }

        // Generate the required grid with all the required nodes at the required place
        private void GenerateGrid()
        {
            // Initialize nodeGrid with the required size for the current level
            _nodeGrid = new Node[levelConfig.Rows, levelConfig.Columns];

            for (int i = 0; i < levelConfig.Rows; i++)
            {
                for (int j = 0; j < levelConfig.Columns; j++)
                {
                    LevelConfig.NodeData nodeData = levelConfig.GetGridElement(i, j);
                    if (nodeData != null && nodeData.nodePrefab != null)
                    {
                        var nodeObj = Instantiate(nodeData.nodePrefab, gridLayoutGroup.transform);
                        var node = nodeObj.GetComponent<Node>();
                        _nodeGrid[i, j] = node;
                        
                        // Initialize the node with its specific data from level config
                        node.Initialize(i, j, nodeData.connectableSides);
                    }
                    else
                    {
                        Debug.LogError($"Node prefab is missing at position ({i}, {j})");
                    }
                }
            }
        }

        public Node GetNodeAt(int row, int column)
        {
            if (row >= 0 && row < levelConfig.Rows && column >= 0 && column < levelConfig.Columns)
            {
                return _nodeGrid[row, column];
            }
            else
            {
                Debug.LogWarning($"Requested node at ({row}, {column}) is out of bounds.");
                return null;
            }
        }
    }
}