using IG.Factory;
using IG.Level;
using IG.NodeSystem;
using UnityEngine;
namespace IG.Controller 
{
    public class SpriteGridManager : MonoBehaviour, IGrid
    {
        [SerializeField] private Grid gridGroup; // Grid parent
        [SerializeField] private LevelConfig levelConfig; // Level data will be used from this config
        private Node[,] _nodeGrid; // All nodes in the grid are stored here

        private void Start()
        {
            levelConfig.Initialize(gridGroup);
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            var gridType = levelConfig.gridType;
            float nodeSize = levelConfig.nodeSize;
            float spacing = levelConfig.spacing;
            int rows = levelConfig.rows;
            int columns = levelConfig.columns;

            // Initialize nodeGrid with the required size for the current level
            _nodeGrid = new Node[rows, columns];

             // Calculate the center offset to position the grid in the center of the screen
            Vector3 gridOffset = new (
                -(columns - 1) * nodeSize * 0.5f, 
                (rows - 1) * nodeSize * 0.5f, 
                0f
            );

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var nodeData = levelConfig.GetGridElement(row, column);
                    if (nodeData != null)
                    {
                        // Initialize the node with its specific data from level config
                        _nodeGrid[row, column] = NodeFactory.CreateNode(nodeData, gridGroup.transform,
                        row, column, levelConfig.gridType, this);

                        if(_nodeGrid[row, column] == null) continue; //It is empty node and should be ignored 

                        // Calculate the cell position by using offset
                        var cellPosition = new Vector3Int(column, -row, 0);
                        var nodePosition = gridGroup.CellToWorld(cellPosition) + gridOffset;

                        // Position the node
                        _nodeGrid[row, column].transform.position = nodePosition;
                    }
                }
            }
        }

        public Node GetNodeAt(int row, int column)
        {
            if (row >= 0 && row < levelConfig.rows && column >= 0 && column < levelConfig.columns)
            {
                //Debug.Log($"Requested node ({row}, {column}).");
                return _nodeGrid[row, column];
            }
            else
            {
                //Debug.LogWarning($"Requested node at ({row}, {column}) is out of bounds.");
                return null;
            }
        }
    }
}