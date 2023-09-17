using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void SaveData()
    {
        SavedData savedData = new SavedData
        {
            playerLevel = 1,
        };

        string jsonData = JsonUtility.ToJson(savedData);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public SavedData LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);
            return savedData;
        }
        else
        {
            return null;
        }
    }
}

public class SavedData
{
    public int playerLevel;
}