using UnityEngine;

namespace IG.Controller 
{
    public class CircuitValidation : MonoBehaviour
    {
        private int _connectedComputersCount = 0;
        private int _totalComputers = 0;

        public void UpdateConnectedComputersCount(bool isConnected)
        {
            if(isConnected) 
            {
                _connectedComputersCount++;

                // Check if level is complete
                if (_connectedComputersCount == _totalComputers)
                {
                    OnCircuitComplete();
                }
            }
            else _connectedComputersCount--;
        }

        private void OnCircuitComplete()
        {
            Debug.Log("Level Complete! All computers are connected.");
            // Additional logic for level completion
        }
    }
}
