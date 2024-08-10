using System;
using UnityEngine;

namespace IG.Controller 
{
    /// <summary>
    /// Handles level complete validation
    /// Attached to Grid which is initialized for every level
    /// </summary>
    public class CircuitValidation : MonoBehaviour
    {
        private int _connectedComputersCount = 0;
        private int _totalComputers;
        public static Action OnValidated;

        //Needs to be initialized on every level load
        public void Initialize(int totalComputers) 
        {
            _totalComputers = totalComputers;
        }

        public void UpdateConnectedComputersCount(bool isConnected)
        {
            if(isConnected) 
            {
                _connectedComputersCount++;

                // Check if the all computers are connected to wifi
                if (_connectedComputersCount.Equals(_totalComputers))
                {
                    // all Computers are connected
                    CompleteValidation();
                }
            }
            else _connectedComputersCount--;
        }

        private void CompleteValidation()
        {
            Debug.Log("All computers are connected.");

            OnValidated?.Invoke();
        }
    }
}
