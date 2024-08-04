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

                // Check if level is complete
                if (_connectedComputersCount == TotalComputers)
                {
                    OnCircuitComplete();
                }
            }
            else _connectedComputersCount--;
        }

        private void OnCircuitComplete()
        {
            Debug.Log("Level Complete! All computers are connected.");
            
            OnValidated?.Invoke();
        }
    }
}
