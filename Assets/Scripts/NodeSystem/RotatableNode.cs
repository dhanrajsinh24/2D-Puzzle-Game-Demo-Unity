using UnityEngine;
using System.Collections.Generic;
using static IG.Level.LevelConfig;
using IG.Controller;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Script enables the node to Rotate on click
    /// </summary>
    [RequireComponent(typeof(Node))]
    public class RotatableNode : Node, IRotatable
    {
        private GridType _gridType; 
        private readonly Dictionary<Node, bool> _connectionCache = new();
        private GridManager _gridManager;

        public void Initialize(GridType type, GridManager gridManager) 
        {
            _gridType = type;
            _gridManager = gridManager;
        }

        public override void NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            Rotate();
        }

        public void Rotate()
        {
            // TODO Rotate animation
            // Rotate the Node by 90 degrees clockwise
            transform.Rotate(0, 0, -90);

            ShiftConnectibleSides();

            CheckConnections();
        }

        protected override void UpdateVisualFeedback()
        {
            // Update the node's visual state based on connection status
            if (isConnected)
            {
                // Code to enable glow effect
            }
            else
            {
                // Code to disable glow effect
            }
        }

        // Shift the connectableSides array based on grid type
        private void ShiftConnectibleSides()
        {
            var size = (int)_gridType;
            bool lastSide = ConnectableSides[size - 1];
            for (int i = size - 1; i > 0; i--)
            {
                ConnectableSides[i] = ConnectableSides[i - 1];
            }
            ConnectableSides[0] = lastSide;
        }

        public bool CanConnectWith(Node other)
        {
            // Calculate row and column difference
            int rowDiff = other.Row - Row;
            int colDiff = other.Column - Column;

            // Define a set of connection pairs based on the node's shape
            int[,] connectionPairs;
            if (_gridType == GridType.Square)
            {
                // Connection pairs for square grid (4 sides)
                // Each pair: { thisNodeSide, otherNodeSide }
                connectionPairs = new int[,]
                {
                    { 0, 2 }, // Up (0) to Down (2)
                    { 2, 0 }, // Down (2) to Up (0)
                    { 1, 3 }, // Right (1) to Left (3)
                    { 3, 1 }  // Left (3) to Right (1)
                };
            }
            else if (_gridType == GridType.Hexagonal)
            {
                // Connection pairs for hexagonal grid (6 sides)
                // Each pair: { thisNodeSide, otherNodeSide }
                connectionPairs = new int[,]
                {
                    { 0, 3 }, // Up (0) to Down (3)
                    { 1, 4 }, // Top-Right (1) to Bottom-Left (4)
                    { 2, 5 }, // Bottom-Right (2) to Top-Left (5)
                    { 3, 0 }, // Down (3) to Up (0)
                    { 4, 1 }, // Bottom-Left (4) to Top-Right (1)
                    { 5, 2 }  // Top-Left (5) to Bottom-Right (2)
                };
            }
            else
            {
                return false; // Unsupported type
            }

            // Find the corresponding connection index pair based on position difference
            for (int i = 0; i < connectionPairs.GetLength(0); i++)
            {
                if (IsMatchingConnection(rowDiff, colDiff, connectionPairs[i, 0]))
                {
                    // Check if this node's side can connect with the other node's corresponding side
                    return ConnectableSides[connectionPairs[i, 0]] && other.ConnectableSides[connectionPairs[i, 1]];
                }
            }

            return false; // No valid connection found
        }

        private bool IsMatchingConnection(int rowDiff, int colDiff, int sideIndex)
        {
            if (_gridType == GridType.Square)
            {
                return (sideIndex == 0 && rowDiff == -1 && colDiff == 0) ||  // Up
                    (sideIndex == 2 && rowDiff == 1 && colDiff == 0) ||   // Down
                    (sideIndex == 1 && rowDiff == 0 && colDiff == 1) ||   // Right
                    (sideIndex == 3 && rowDiff == 0 && colDiff == -1);    // Left
            }
            else if (_gridType == GridType.Hexagonal)
            {
                return (sideIndex == 0 && rowDiff == -1 && colDiff == 0) ||  // Up
                    (sideIndex == 1 && rowDiff == -1 && colDiff == 1) ||  // Top-Right
                    (sideIndex == 2 && rowDiff == 0 && colDiff == 1) ||   // Bottom-Right
                    (sideIndex == 3 && rowDiff == 1 && colDiff == 0) ||   // Down
                    (sideIndex == 4 && rowDiff == 1 && colDiff == -1) ||  // Bottom-Left
                    (sideIndex == 5 && rowDiff == 0 && colDiff == -1);    // Top-Left
            }

            return false; // No matching connection found
        }

        public void CheckConnections()
        {
            foreach (var neighbor in GetNeighbors())
            {
                if (neighbor != null && !_connectionCache.TryGetValue(neighbor, out bool isConnected))
                {
                    if(isConnected) break;
                    bool newConnection = CanConnectWith(neighbor);
                    _connectionCache[neighbor] = newConnection;
                    //neighbor.UpdateConnection(this, newConnection);
                }
            } 
        }

        public void UpdateConnection(Node other, bool isConnected)
        {
            _connectionCache[other] = isConnected;

            //SetConnectionStatus(isConnected);
            // Apply visual feedback if necessary
        }

        private IEnumerable<Node> GetNeighbors()
        {
            // Precompute neighboring offsets for both square and hexagonal grids
            var offsets = _gridType == GridType.Square
                ? new (int, int)[]
                {
                    (-1, 0),  // Up
                    (0, 1),   // Right
                    (1, 0),   // Down
                    (0, -1)   // Left
                }
                : new (int, int)[]
                {
                    (-1, 0),  // Up
                    (-1, 1),  // Top-Right
                    (0, 1),   // Bottom-Right
                    (1, 0),   // Down
                    (1, -1),  // Bottom-Left
                    (0, -1)   // Top-Left
                };

            // Loop through possible connections based on the current type
            for (int i = 0; i < ConnectableSides.Length; i++)
            {
                if (ConnectableSides[i])  // Only consider sides that are connectable
                {
                    int neighborRow = Row + offsets[i].Item1;
                    int neighborColumn = Column + offsets[i].Item2;

                    yield return _gridManager.GetNodeAt(neighborRow, neighborColumn);
                }
            }
        }
    }
}
