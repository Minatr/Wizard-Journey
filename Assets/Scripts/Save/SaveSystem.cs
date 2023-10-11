using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public void SaveData(Vector3 player, string enemyName, int enemyLevel)
    {
        SavedData savedData = new SavedData
        {
            actualScene = SceneManager.GetActiveScene().buildIndex,
            playerPosx = player.x,
            playerPosy = player.y,
            playerPosz = player.z,
            enemyName = enemyName,
            enemyLevel = enemyLevel
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
    // Scene informations
    public int actualScene;

    // Player informations
    public float playerPosx;
    public float playerPosy;
    public float playerPosz;

    // Enemy informations
    public string enemyName;
    public int enemyLevel;
}