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
            CableNode
        }

        [System.Serializable]
        public class NodeData
        {
            //public int row;
            //public int column;
            public GameObject nodePrefab; // Prefab for the node
            public NodeType nodeType;
            
            // TODO Length should be validated to either 4(square) or 6(Hexagonal)
            public bool[] connectableSides; //Side bool array to specify which side is connectable (True is so)
        }

        public GridType gridType = GridType.Square;
        public float nodeSize = 100f;
        public float spacing = 5f;
        public NodeData[] grid;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private void OnValidate()
        {
            ValidateGridSize();
            CalculateRowsAndColumns();

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
            ValidateGridSize();
            CalculateRowsAndColumns();

            nodeParentGrid.cellSize = new Vector2(nodeSize, nodeSize);
            nodeParentGrid.spacing = new Vector2(spacing, spacing);
            nodeParentGrid.constraintCount = gridType == GridType.Square ? Columns : Rows;
        }

        private void ValidateGridSize()
        {
            var gridSize = grid.Length;
            if (gridSize <= 0)
            {
                Debug.LogError("Grid size must be greater than 0.");
            }
            else if (gridSize % 2 != 0)
            {
                Debug.LogError("Grid size must be divisible by 2.");
            }
        }

        private void CalculateRowsAndColumns()
        {
            var gridSize = grid.Length;
            if (gridSize > 0 && gridSize % 2 == 0)
            {
                Rows = Columns = gridSize / 2;
            }
            else
            {
                Debug.LogError("Grid size is not valid for calculation.");
            }
        }

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

        public NodeData GetGridElement(int row, int column)
        {
            if (grid.Length < 2 || row < 0 || row >= Rows || column < 0 || column >= Columns)
            {
                Debug.LogWarning("Grid position out of bounds");
                return null;
            }

            int index = row * Columns + column;
            return grid[index];
        }
    }
}