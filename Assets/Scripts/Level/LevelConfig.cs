using UnityEngine;
using UnityEngine.UI;

namespace IG.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "IG/Level")]
    public class LevelConfig : ScriptableObject
    {
        public enum GridType
        {
            Square = 4,
            Hexagonal = 6
        }

        public enum NodeType 
        {
            WiFiNode,
            ComputerNode,
            CableNode,
            EmptyNode
        }

        [System.Serializable]
        public class NodeData
        {
            public GameObject nodePrefab; // Prefab for the node
            public NodeType nodeType;
            
            // Length should be validated to either 4(square) or 6(Hexagonal)
            public bool[] connectableSides; // Side bool array to specify which side is connectable (True is so)

            [Range(0, 5)] // Range is 0-5 to support 4 rotations for Square and 6 for Hexagonal
            public int initialRotation; // Number of rotations (0 for no rotation)
        }

        public GridType gridType = GridType.Square;
        public float nodeSize = 100f;
        public float spacing = 5f;
        public int rows;
        public int columns;
        public NodeData[] grid;

        private void OnValidate()
        {
            ValidateGridSize();

            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i] == null)
                {
                    Debug.Log($"{i}th element is null");
                }
            }
        }

        public void Initialize(Grid nodeParentGrid)
        {
            ValidateGridSize();

            nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
            nodeParentGrid.cellGap = new Vector2(spacing, spacing);
        }

        private void ValidateGridSize()
        {
            var gridSize = grid.Length;
            if (gridSize <= 0)
            {
                Debug.LogError("Grid size must be greater than 0.");
            }
            
            if (gridSize % 2 != 0)
            {
                Debug.LogError("Grid size must be divisible by 2.");
            }

            if(rows == 0 || columns == 0 || !gridSize.Equals(rows * columns)) 
            {
                Debug.LogError("Grid does not have valid rows or colums or Grid size is not valid");
            }
        }

        public void SetGridElement(int row, int column, NodeData element)
        {
            if (row >= 0 && row < rows && column >= 0 && column < columns)
            {
                int index = row * columns + column;
                grid[index] = element;
            }
            else
            {
                Debug.LogWarning("Grid position out of bounds");
            }
        }

        public NodeData GetGridElement(int row, int column)
        {
            if (grid.Length < 2 || row < 0 || row >= rows || column < 0 || column >= columns)
            {
                Debug.LogWarning("Grid position out of bounds");
                return null;
            }

            int index = row * columns + column;
            return grid[index];
        }
    }
}