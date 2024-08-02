using UnityEngine;
using System.Collections.Generic;
using static IG.Level.LevelConfig;
using IG.Controller;
using System;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Script enables the node to Rotate on click
    /// </summary>
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

            // Get connection pairs based on grid type
            var connectionPairs = GetConnectionPairs();

            // Find the corresponding connection index pair based on position difference
            foreach (var (thisSide, otherSide) in connectionPairs)
            {
                if (IsMatchingConnection(rowDiff, colDiff, thisSide))
                {
                    // Check if this node's side can connect with the other node's corresponding side
                    return ConnectableSides[thisSide] && other.ConnectableSides[otherSide];
                }
            }

            return false; // No valid connection found
        }

        // Helper method to get connection pairs
        private (int thisSide, int otherSide)[] GetConnectionPairs()
        {
            if (_gridType == GridType.Square)
            {
                return new (int, int)[]
                {
                    (0, 2), // Up (0) to Down (2)
                    (2, 0), // Down (2) to Up (0)
                    (1, 3), // Right (1) to Left (3)
                    (3, 1)  // Left (3) to Right (1)
                };
            }
            else if (_gridType == GridType.Hexagonal)
            {
                return new (int, int)[]
                {
                    (0, 3), // Up (0) to Down (3)
                    (1, 4), // Top-Right (1) to Bottom-Left (4)
                    (2, 5), // Bottom-Right (2) to Top-Left (5)
                    (3, 0), // Down (3) to Up (0)
                    (4, 1), // Bottom-Left (4) to Top-Right (1)
                    (5, 2)  // Top-Left (5) to Bottom-Right (2)
                };
            }

            return Array.Empty<(int, int)>(); // Return an empty array for unsupported grid types
        }

        private bool IsMatchingConnection(int rowDiff, int colDiff, int sideIndex)
        {
            return sideIndex switch
            {
                0 => rowDiff == -1 && colDiff == 0,  // Up
                1 => (_gridType == GridType.Square) ? rowDiff == 0 && colDiff == 1 : rowDiff == -1 && colDiff == 1, // Right / Top-Right
                2 => (_gridType == GridType.Square) ? rowDiff == 1 && colDiff == 0 : rowDiff == 0 && colDiff == 1,  // Down / Bottom-Right
                3 => (_gridType == GridType.Square) ? rowDiff == 0 && colDiff == -1 : rowDiff == 1 && colDiff == 0, // Left / Down
                4 => _gridType == GridType.Hexagonal && rowDiff == 1 && colDiff == -1, // Bottom-Left
                5 => _gridType == GridType.Hexagonal && rowDiff == 0 && colDiff == -1, // Top-Left
                _ => false  // Default case for unsupported or invalid side index
            };
        }


        public void CheckConnections()
        {
            foreach (var neighbor in GetNeighbors())
            {
                if (neighbor != null && !_connectionCache.TryGetValue(neighbor, out bool isConnected))
                {
                    Debug.Log($"{neighbor.gameObject.name}");
                    continue;
                    //if(isConnected) break;
                    bool newConnection = CanConnectWith(neighbor);
                    _connectionCache[neighbor] = newConnection;
                    //neighbor.UpdateConnection(this, newConnection);
                }
                else 
                {
                    Debug.Log($" {neighbor} null ");
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
            // Loop through possible connections based on the current type
            for (int i = 0; i < ConnectableSides.Length; i++)
            {
                if (ConnectableSides[i])  // Only consider sides that are connectable
                {
                    (int rowOffset, int colOffset) = GetOffsetByIndex(i);
                    int neighborRow = Row + rowOffset;
                    int neighborColumn = Column + colOffset;
                    var node = _gridManager.GetNodeAt(neighborRow, neighborColumn);
                    yield return node;
                }
            }
        }

        // Helper function to get offsets based on grid type and connection index
        private (int rowOffset, int colOffset) GetOffsetByIndex(int index)
        {
            if (_gridType == GridType.Square)
            {
                return index switch
                {
                    0 => (-1, 0),  // Up
                    1 => (0, 1),   // Right
                    2 => (1, 0),   // Down
                    3 => (0, -1),  // Left
                    _ => (0, 0),   // Default case (shouldn't be hit)
                };
            }
            
            if (_gridType == GridType.Hexagonal)
            {
                return index switch
                {
                    0 => (-1, 0),  // Up
                    1 => (-1, 1),  // Top-Right
                    2 => (0, 1),   // Bottom-Right
                    3 => (1, 0),   // Down
                    4 => (1, -1),  // Bottom-Left
                    5 => (0, -1),  // Top-Left
                    _ => (0, 0),   // Default case (shouldn't be hit)
                };
            }

            // Default case for unsupported grid types
            return (0, 0);
        }

    }
}
