using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using IG.Controller;
using static IG.Level.LevelConfig;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Core script to manage the Node
    /// </summary>
    public abstract class Node : MonoBehaviour, IConnectable
    {
        public int Row { get; private set; } // row index of current node (to know its place)
        public int Column { get; private set; } // column index same as row
        public bool[] ConnectableSides { get; private set; } // Sides that can connect (size 4 for square, 6 for hexagonal)
        public bool IsConnectedToWifi {get; set;} //True if this node is connected to at least one node
        public RectTransform RectTransform { get; private set; }
        private IGrid _gridManager;
        protected GridType gridType;
        [SerializeField] private ColorFeedback _colorFeedback;
        [SerializeField] protected Transform rotateTransform;
        public WiFiNode ConnectedWiFiNode {get; private set;} // Reference to the connected WiFiNode

        public virtual void Initialize(int row, int column, bool[] initialConnectableSides, 
        IGrid gridManager, GridType gridType)
        {
            Row = row;
            Column = column;
            RectTransform = GetComponent<RectTransform>();
            _gridManager = gridManager;
            this.gridType = gridType;
            ConnectableSides = initialConnectableSides;
        }

        // Function to Apply initial rotation to the node
        public void ApplyInitialRotation(int initialRotation)
        {
            float angle = 0f;

            if (gridType == GridType.Square)
            {
                angle = initialRotation * -90f; // 4 possible rotations
            }
            else if (gridType == GridType.Hexagonal)
            {
                angle = initialRotation * -60f; // 6 possible rotations
            }

            rotateTransform.Rotate(Vector3.forward, angle); // Rotate around the Z-axis

            //Update connectible sides by shifting it by rotation multiplier times
            for(int i = 0; i < initialRotation; i++) 
            {
                ShiftConnectibleSides();
            }
        }

        // Shift the connectableSides array based on grid type
        public void ShiftConnectibleSides()
        {
            var size = (int)gridType;
            bool lastSide = ConnectableSides[size - 1];
            for (int i = size - 1; i > 0; i--)
            {
                ConnectableSides[i] = ConnectableSides[i - 1];
            }
            ConnectableSides[0] = lastSide;
        }

        public void CheckConnections()
        {
            // Get all connectable neighbors
            var connectableNeighbors = GetConnectableNeighbors().ToList();

            // Check if the node is connected to WiFi through any of its neighbors
            bool isConnected = connectableNeighbors.Any(neighbor => neighbor is WiFiNode || neighbor.IsConnectedToWifi);

            // If the node was connected to WiFi or is now connected, trigger WiFi node to revalidate connections
            if (isConnected && !ConnectedWiFiNode)
            {
                var neighborWifi = connectableNeighbors.OfType<WiFiNode>().FirstOrDefault();
                if(neighborWifi == null) 
                {
                    neighborWifi = connectableNeighbors.First(neighbor => neighbor.IsConnectedToWifi).ConnectedWiFiNode;
                }

                if(neighborWifi == null) Debug.LogError($"wifi not found");
                else Debug.Log($"Wifi {neighborWifi.name}");
                
                ConnectedWiFiNode = neighborWifi;
            }

            if(ConnectedWiFiNode != null) 
            {
                ConnectedWiFiNode.RevalidateConnections(); 
            }
        }

        // Get all nodes that are neighbors and connectable with the current node
        public IEnumerable<Node> GetConnectableNeighbors()
        {
            // Loop through possible connections based on the current type
            for (int i = 0; i < ConnectableSides.Length; i++)
            {
                if (ConnectableSides[i]) // Only consider sides that are connectable
                {
                    (int rowOffset, int colOffset) = GetOffsetByIndex(i);
                    int neighborRow = Row + rowOffset;
                    int neighborColumn = Column + colOffset;
                    var neighborNode = _gridManager.GetNodeAt(neighborRow, neighborColumn);

                    // Check if the neighbor node exists and its corresponding side is also connectable
                    if (neighborNode != null)
                    {
                        if (CanConnectWith(neighborNode, i))
                        {
                            yield return neighborNode;
                        }
                    }
                }
            }
        }

        //Checks if the node can connect with the other node which is already connected or is wifinode
        public bool CanConnectWith(Node other, int connectibleSideIndex) 
        {
            // Get the opposite side index of the neighbor (e.g., if this node is connecting to the right, 
            // the neighbor should be connectable on the left)
            int oppositeSideIndex = GetOppositeSideIndex(connectibleSideIndex);
            // Check if the neighbor node exists and its corresponding side is also connectable
            return other.ConnectableSides[oppositeSideIndex];
        }

        // Helper function to get offsets based on grid type and connection index
        private (int rowOffset, int colOffset) GetOffsetByIndex(int index)
        {
            if (gridType == GridType.Square)
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
            
            if (gridType == GridType.Hexagonal)
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

        // Helper function to get the opposite side index based on the current side index
        private int GetOppositeSideIndex(int sideIndex)
        {
            if (gridType == GridType.Square)
            {
                return sideIndex switch
                {
                    0 => 2,  // Up (0) -> Down (2)
                    1 => 3,  // Right (1) -> Left (3)
                    2 => 0,  // Down (2) -> Up (0)
                    3 => 1,  // Left (3) -> Right (1)
                    _ => -1, // Default (shouldn't be hit)
                };
            }
            else if (gridType == GridType.Hexagonal)
            {
                return sideIndex switch
                {
                    0 => 3,  // Up (0) -> Down (3)
                    1 => 4,  // Top-Right (1) -> Bottom-Left (4)
                    2 => 5,  // Bottom-Right (2) -> Top-Left (5)
                    3 => 0,  // Down (3) -> Up (0)
                    4 => 1,  // Bottom-Left (4) -> Top-Right (1)
                    5 => 2,  // Top-Left (5) -> Bottom-Right (2)
                    _ => -1, // Default (shouldn't be hit)
                };
            }

            return -1; // Default case for unsupported grid types
        }

        // Sets connection status and wifi asignment
        public void SetConnectionStatus(WiFiNode wifi)
        {
            IsConnectedToWifi = wifi != null;
            Debug.Log($"SetConnectionStatus {IsConnectedToWifi} {gameObject.name}");
            if(wifi == null) ConnectedWiFiNode = null;
            else ConnectedWiFiNode = wifi;
            UpdateVisualFeedback();
        }

        // Update the node's visual state
        protected virtual void UpdateVisualFeedback() 
        {
           // Update the node's visual state based on connection status
            _colorFeedback.ToggleGlow(IsConnectedToWifi);
        }

        public abstract void NodeClicked();
    }
}
