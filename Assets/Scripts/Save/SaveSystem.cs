using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public static SavedData currentSave;

    private void Awake()
    {
        instance = this;
    }

    /**
     * Effectue toute la logique liée à de nouvelles valeurs à sauvegarder sur une zone d'exploration
     */
    public void SaveData(string previousScene, Vector3 player, int playerManaMax, SpellSave spellSave, string enemyName, int enemyLevel, GameObject[] allEnemies)
    {
        // Transformation des données des ennemis pour le Json
        List<SavedEnemyData> tempList = new List<SavedEnemyData>();
        SavedEnemyData temp;
        foreach (var enemy in allEnemies)
        {
            temp = new SavedEnemyData();
            temp.name = enemy.GetComponent<EnemyAI>().enemyName;
            temp.level = enemy.GetComponent<EnemyAI>().enemyLevel;
            temp.posx = enemy.transform.position.x;
            temp.posy = enemy.transform.position.y;
            temp.posz = enemy.transform.position.z;
            temp.attacking = enemy.GetComponent<EnemyAI>().attacking;
            tempList.Add(temp);
        }

        currentSave = new SavedData
        {
            version = Application.version,
            actualScene = SceneManager.GetActiveScene().name,
            previousScene = previousScene,
            playerPosx = player.x,
            playerPosy = player.y,
            playerPosz = player.z,
            playerManaMax = playerManaMax,
            spellSave = spellSave,
            enemyName = enemyName,
            enemyLevel = enemyLevel,
            enemyDataList = tempList,
        };

        SaveGame();
    }

    /**
     * Sauvegarde la partie, on ne met à jour que l'index de scène
     */
    public void SaveGame()
    {
        currentSave.version = Application.version;
        currentSave.actualScene = SceneManager.GetActiveScene().name;
        string jsonData = JsonUtility.ToJson(currentSave);
        string filePath = Application.persistentDataPath + "/SavedData.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public void DeleteSaveFile()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }


    public SavedData LoadData()
    {
        string filePath = Application.persistentDataPath + "/SavedData.json";

        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            SavedData savedData = JsonUtility.FromJson<SavedData>(jsonData);
            currentSave = savedData;
            return savedData;
        }
        else
        {
            return null;
        }
    }

    public GameObject[] getAllEnemies()
    {
        // On récupère tous les GameObjects de la scène
        GameObject[] allWerewolves = FindObjectsOfType<GameObject>();
        List<GameObject> werewolvesWithEnemyIA = new List<GameObject>();

        foreach (GameObject werewolf in allWerewolves)
        {
            // On regarde si ce sont des ennemis
            EnemyAI enemyScript = werewolf.GetComponent<EnemyAI>();
            if (enemyScript != null)
            {
                werewolvesWithEnemyIA.Add(werewolf);
            }
        }

        // On obtient la liste de tous les ennemis présents dans le zone actuelle
        return werewolvesWithEnemyIA.ToArray();
    }
}

public class SavedData
{
    // Game informations
    public string version;

    // Scene informations
    public string actualScene;
    public string previousScene;

    // Player informations
    public float playerPosx;
    public float playerPosy;
    public float playerPosz;
    public int playerManaMax;
    public SpellSave spellSave;

    // Enemy informations
    public string enemyName;
    public int enemyLevel;
    public List<SavedEnemyData> enemyDataList;
}

[System.Serializable]
public class SavedEnemyData
{
    public string name;
    public int level;
    public float posx;
    public float posy;
    public float posz;
    public bool attacking;
}

[System.Serializable]
public class SpellSave
{
    public string currentOffensiveSpell;
    public string currentUtilitySpell;
    public Dictionary<string, SpellType> spellTypes;

    public SpellSave(string currentOffensiveSpell, string currentUtilitySpell, Dictionary<string, SpellType> spellTypes)
    {
        this.currentOffensiveSpell = currentOffensiveSpell;
        this.currentUtilitySpell = currentUtilitySpell;
        this.spellTypes = spellTypes;
    }
}

[System.Serializable]
public class SpellType
{
    public int level;
    public int cost;
    public int damage;

    public SpellType(int level, int cost, int damage)
    {
        this.level = level;
        this.cost = cost;
        this.damage = damage;
    }
}