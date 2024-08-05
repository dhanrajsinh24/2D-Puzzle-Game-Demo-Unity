using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace IG.Utils 
{
    public class AddressableLoader
    {
        // Method to load a ScriptableObject by its address with a callback
        public void LoadScriptableObject<T>(string address, Action<T> onLoaded) where T : ScriptableObject
        {
            Addressables.LoadAssetAsync<T>(address).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    T loadedObject = handle.Result;
                    onLoaded?.Invoke(loadedObject);
                }
                else
                {
                    Debug.LogError("Failed to load ScriptableObject: " + handle.Status);
                    onLoaded?.Invoke(null);
                }
            };
        }
    }
}

