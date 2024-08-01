using UnityEngine;
using UnityEngine.UI;

namespace IG.Level
{
    /// <summary>
    /// Level configuration to be used for the particular level
    /// </summary>
    [CreateAssetMenu(fileName = "Level", menuName = "IG/Level")]
    public class LevelConfig : ScriptableObject
    {
        public enum GridType
        {
            Square,
            Hexagonal
        }

        public enum NodeType
        {
            WiFiNode,
            ComputerNode,
            CableNode
        }

        /// <summary>
        /// Responsible to hold all information about the node at the specific position
        /// </summary>
        [System.Serializable]
        public class NodeData
        {
            public int row;
            public int column;
            public NodeType nodeType;
        }

        public GridType gridType = GridType.Square; // Type of the grid
        public int GridSize { get; private set; } = 2; // Total grid size, must be divisible by 2 and not zero
        public float nodeSize = 100f; // Size of each node
        public float spacing = 5f; // Spacing between nodes
        public NodeData[] grid; // Aarray to hold all nodes row by row to make a grid

        // Public properties for rows and columns
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private void OnValidate()
        {
            // Ensure the grid size is valid
            ValidateGridSize();
            
            // Initialize grid with default values
            CalculateRowsAndColumns();

            if (grid == null || grid.Length != Rows * Columns)
            {
                grid = new NodeData[Rows * Columns];
            }
            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == null)
                {
                    Debug.Log($"{i}th element is null");
                }
            }
        }

        public void Initialize(GridLayoutGroup nodeParentGrid)
        {
            // Validate and calculate grid size before initializing
            ValidateGridSize();
            CalculateRowsAndColumns();

            // Set the Grid layout group as required with the node size, spacing, etc.
            nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
            nodeParentGrid.spacing = new Vector2(spacing, spacing);
            nodeParentGrid.constraintCount = gridType == GridType.Square ? Columns : Rows;
        }

        // Validate grid size to ensure it's not zero and divisible by 2
        private void ValidateGridSize()
        {
            if (GridSize <= 0)
            {
                Debug.LogError("Grid size must be greater than 0.");
                //TODO Default to safe value
            }
            else if (GridSize % 2 != 0)
            {
                Debug.LogError("Grid size must be divisible by 2.");
                //TODO Default to safe value
            }
        }

        // Calculate rows and columns based on the grid size
        private void CalculateRowsAndColumns()
        {
            if (GridSize > 0 && GridSize % 2 == 0)
            {
                Rows = Columns = GridSize / 2;
            }
            else
            {
                Debug.LogError("Grid size is not valid for calculation.");
                //TODO Default to safe value
            }
        }

        /// <summary>
        /// Responsible to fill the grid elements programatically if needed
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="element"></param>
        public void SetGridElement(int row, int column, NodeData element)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                int index = row * Columns + column;
                grid[index] = element;
            }
            else
            {
                Debug.LogWarning("Grid position out of bounds");
            }
        }

        /// <summary>
        /// Get the element at the specified position in the grid
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public NodeData GetGridElement(int row, int column)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                int index = row * Columns + column;
                return grid[index];
            }
            else
            {
                Debug.LogWarning("Grid position out of bounds");
                return null;
            }
        }
    }
}