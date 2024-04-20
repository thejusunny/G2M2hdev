using System;
using System.IO;
using UnityEngine;

public static class JsonHandler
{
    public static String FilePath => Application.persistentDataPath;
    // Generic method to save data to a file in JSON format
    public static void SaveToJson<T>(T data, string fileName)
    {
        try
        {
            // Serialize the data to a JSON string
            string json = JsonUtility.ToJson(data);

            // Write the JSON string to a file
            File.WriteAllText(Path.Combine(FilePath, fileName), json);

            Debug.Log("Data saved successfully.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to save data: {ex.Message}");
        }
    }

    // Generic method to load data from a file in JSON format
    public static T LoadFromJson<T>(string fileName) where T : new()
    {
        string fullPath = Path.Combine(FilePath, fileName);
        try
        {
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log("Data loaded successfully.");
                return data;
            }
            else
            {
                // File doesn't exist, create a new file with default T values
                T defaultData = new T();
                SaveToJson(defaultData, fileName); // Save the default data to a new file
                Debug.Log($"File not found. A new file with default values has been created: {fileName}");
                return defaultData;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load data: {ex.Message}");
            return default(T);
        }
    }
    public static void DeleteSaveFile(string fileName)
    {
        string fullPath = Path.Combine(FilePath, fileName);
        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                Debug.Log($"File deleted successfully: {fileName}");
            }
            else
            {
                Debug.LogWarning($"File not found: {fileName}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to delete file: {ex.Message}");
        }
    }
    public static T LoadFromJsonNull<T>(string fileName)
    {
        string fullPath = Path.Combine(FilePath, fileName);
        try
        {
            if (File.Exists(fullPath))
            {
                string json = File.ReadAllText(fullPath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log("Data loaded successfully.");
                return data;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load data: {ex.Message}");
        }
        return default;
    }
}
