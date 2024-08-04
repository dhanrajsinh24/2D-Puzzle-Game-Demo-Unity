using System;
using UnityEngine;

namespace IG.Controller 
{
    public class CircuitValidation : MonoBehaviour
    {
        private int _connectedComputersCount = 0;
        public int TotalComputers {get; set;}
        public static Action OnValidated;

        public void UpdateConnectedComputersCount(bool isConnected)
        {
            if(isConnected) 
            {
                _connectedComputersCount++;

                // Check if the all computers are connected to wifi
                if (_connectedComputersCount == TotalComputers)
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
            
            ClearData();

            OnValidated?.Invoke();
        }

        private void ClearData() 
        {
            _connectedComputersCount = 0;
            TotalComputers = 0;
        }
    }
}
