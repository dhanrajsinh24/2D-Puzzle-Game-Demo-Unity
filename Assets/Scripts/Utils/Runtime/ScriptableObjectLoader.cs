using UnityEngine;

public class ScriptableObjectLoader
{
    // Load a ScriptableObject by its name from the Resources folder
    public T LoadScriptableObject<T>(string folderPath, string objectName) where T : ScriptableObject
    {
        var path = folderPath + "/" + objectName;
        var loadedObject = Resources.Load<T>(path);

        if (!loadedObject)
        {
            Debug.LogError($"ScriptableObject '{objectName}' not found at '{path}'");
            return null;
        }

        return loadedObject;
    }
}
