using System.Collections;
using System.Linq;
using UnityEngine;

namespace IG.NodeSystem 
{
    /// <summary>
    /// Extends Node to add rotation functionality. 
    /// Handles updating connections when the node is rotated.
    /// </summary>
    public class RotatableNode : Node, IRotatable
    {
        private Coroutine _rotateCoroutine;
        private float _lastTargetZRotation;

        public override IEnumerator NodeClicked()
        {
            Debug.Log($"{gameObject.name} clicked");
            yield return Rotate();
        }

        public IEnumerator Rotate()
        {
            yield return RotateBy90();

            ShiftConnectibleSides();

            CheckConnections();
        }

        /// <summary>
        /// Checks the connections of the current node to determine if it's connected to a WiFi node.
        /// </summary>
        public void CheckConnections()
        {
            // Get all connectable neighbors
            var connectableNeighbors = GetConnectableNeighbors().ToList();

            // Check if the node is connected to WiFi through any of its neighbors
            bool isConnected = connectableNeighbors.Any(neighbor => neighbor is WiFiNode || neighbor.IsConnectedToWifi);

            // If the node was not previously connected but now is, find the corresponding WiFi node
            if (isConnected && ConnectedWiFiNode == null)
            {
                ConnectedWiFiNode = connectableNeighbors.OfType<WiFiNode>().FirstOrDefault();

                if (ConnectedWiFiNode == null)
                {
                    // Attempt to find the WiFi node through other connected nodes
                    var connectedNeighbor = connectableNeighbors.FirstOrDefault(neighbor => neighbor.IsConnectedToWifi);
                    if (connectedNeighbor != null)
                    {
                        ConnectedWiFiNode = connectedNeighbor.ConnectedWiFiNode;
                    }
                }

                // Log if the WiFi node is found or not
                if (ConnectedWiFiNode == null)
                {
                    Debug.LogError("WiFi node not found");
                }
                else
                {
                    Debug.Log($"Connected to WiFi: {ConnectedWiFiNode.name}");
                }
            }

            // If the node is connected to a WiFi node, revalidate all connections
            if (ConnectedWiFiNode != null)
            {
                ConnectedWiFiNode.RevalidateConnections();
            }
        }

        private IEnumerator RotateOverTime(float targetZRotation)
        {
            float elapsedTime = 0f;
            float duration = 0.1f; // Adjust the duration for smoothness
            Vector3 startingEulerAngles = rotateTransform.eulerAngles;
            _lastTargetZRotation = targetZRotation;
            Debug.Log($"{gameObject.name}, targetZRotation {targetZRotation}");

            while (elapsedTime < duration)
            {
                float newZRotation = Mathf.Lerp(startingEulerAngles.z, _lastTargetZRotation, elapsedTime / duration);
                rotateTransform.eulerAngles = new Vector3(startingEulerAngles.x, startingEulerAngles.y, newZRotation);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rotateTransform.eulerAngles = new Vector3(startingEulerAngles.x, startingEulerAngles.y, _lastTargetZRotation);
        }

        // Rotate the Node by 90 degrees clockwise
        private IEnumerator RotateBy90()
        {
            if (_rotateCoroutine != null) 
            {
                StopCoroutine(_rotateCoroutine);
            }

            float currentZRotation = rotateTransform.eulerAngles.z;
            
            // Rotate counterclockwise by 90 degrees
            float newZRotation = currentZRotation - 90f; 
            yield return RotateOverTime(newZRotation);
        }
    }
}
