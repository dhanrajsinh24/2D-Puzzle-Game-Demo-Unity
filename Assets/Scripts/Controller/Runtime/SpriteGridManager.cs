using IG.Factory;
using IG.Level;
using IG.NodeSystem;
using UnityEngine;
namespace IG.Controller 
{
    /// <summary>
    /// This handles making the grid and assign appropriate nodes
    /// </summary>
    public class SpriteNodeGrid : MonoBehaviour, IGrid
    {
        [SerializeField] private Grid gridGroup; // Grid parent
        [SerializeField] private NodeFactory nodeFactory;
        private Node[,] _nodeGrid; // All nodes in the grid are stored here

        public Transform GridTransform { get; set; }
        public CircuitValidation CircuitValidation { get; set; }
        public LevelConfig LevelConfig { get; set; }

        public void Initialize(LevelConfig levelConfig)
        {
            LevelConfig = levelConfig;
            LevelConfig.Initialize(gridGroup);

            if(!CircuitValidation) CircuitValidation = GetComponent<CircuitValidation>();
            CircuitValidation.Initialize(LevelConfig.TotalComputers);

            nodeFactory.Initialize(this, GridTransform, levelConfig.gridType, CircuitValidation);
            
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            float nodeSize = LevelConfig.nodeSize;
            float spacing = LevelConfig.spacing;
            int rows = LevelConfig.rows;
            int columns = LevelConfig.columns;

            // Initialize nodeGrid with the required size for the current level
            _nodeGrid = new Node[rows, columns];

             // Calculate the total width and height of the grid (node size only)
            float totalWidth = (columns - 1) * nodeSize;
            float totalHeight = (rows - 1) * nodeSize;

            // Calculate the center offset to position the grid in the center of the screen
            Vector3 gridOffset = new (
                -totalWidth * 0.5f - ((columns - 1) * spacing * 0.5f),
                totalHeight * 0.5f - ((rows - 1) * spacing * 0.5f),
                0f
            );

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var nodeData = LevelConfig.GetGridElement(row, column);
                    if (nodeData != null)
                    {
                        // Initialize the node with its specific data from level config
                        _nodeGrid[row, column] = nodeFactory.CreateNode(nodeData, row, column);

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
            if (row >= 0 && row < LevelConfig.rows && column >= 0 && column < LevelConfig.columns)
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