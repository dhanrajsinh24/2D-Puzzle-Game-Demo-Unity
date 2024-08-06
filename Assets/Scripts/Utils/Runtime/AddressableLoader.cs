using System;
using System.Collections.Generic;
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

        // Method to load multiple ScriptableObjects with label with a callback
        public void LoadScriptableObjectsByLabel<T>(string label, Action<IList<T>> onLoaded) where T : ScriptableObject
        {
            // Load all ScriptableObjects with a specific label
            Addressables.LoadAssetsAsync<T>(label, null).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    IList<T> loadedObjects = handle.Result;
                    onLoaded?.Invoke(loadedObjects);
                }
                else
                {
                    Debug.LogError("Failed to load ScriptableObjects: " + handle.Status);
                    onLoaded?.Invoke(null);
                }
            };
        }
    }
}


