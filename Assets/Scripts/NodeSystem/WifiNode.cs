using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Manages connections for all nodes linked to the WiFi node.
    /// </summary>
    public class WiFiNode : Node
    {
        private List<Node> _connectedNodes = new();
        [SerializeField] private Animation errorFeedback;

        private void Awake() 
        {
            SetConnectionStatus(this);
        }

        public override IEnumerator NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            if(errorFeedback != null) 
            {
                errorFeedback.Play();
            }

            // Wait for the error animation to be played
            float seconds = errorFeedback.clip.length;
            yield return new WaitForSeconds(seconds);
        }

        // Method to add a connected node
        public void AddConnectedNode(Node node)
        {
            if (node != null && node != this && !_connectedNodes.Contains(node))
            {
                Debug.Log($"Adding {node.gameObject}");
                _connectedNodes.Add(node);
                node.SetConnectionStatus(this);
            }
        }

        // Method to remove a connected node
        public void RemoveConnectedNode(Node node)
        {
            if (node != null && _connectedNodes.Contains(node))
            {
                Debug.Log($"Removing {node.gameObject}");
                _connectedNodes.Remove(node);
                node.SetConnectionStatus(null);
            }
        }

        // Check all nodes connected to this WiFi node
        public void RevalidateConnections()
        {
            Debug.Log($"RevalidateConnections");
            var visited = new HashSet<Node>();
            var toRemove = new HashSet<Node>(_connectedNodes);

            CheckValidConnections(this, visited, toRemove);

            // Remove nodes that are no longer connected
            foreach (var node in toRemove)
            {
                RemoveConnectedNode(node);
            }
        }

        // Recursive method to check all connections starting from a given node
        private void CheckValidConnections(Node node, HashSet<Node> visited, HashSet<Node> toRemove)
        {
            if (node == null || visited.Contains(node))
                return;

            Debug.Log($"Checking {node.gameObject}");

            visited.Add(node);

            if(node != this) 
            {
                AddConnectedNode(node); // Ensure the node is in the connected set
            }

            // Get all neighbors and check their connections
            foreach (var neighbor in node.GetConnectableNeighbors())
            {
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    CheckValidConnections(neighbor, visited, toRemove);
                }

                // If this neighbor is in toRemove, it's still connected, so we shouldn't remove it
                toRemove.Remove(neighbor);
            }
        }
    }
}
