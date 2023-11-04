using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform player;

    void Start()
    {
        SavedData savedData = SaveSystem.currentSave;
        if (savedData != null)
        {
            if (savedData.previousScene != "Combat") player.position = new Vector3(10, 2.7f, 10);
            else player.position = new Vector3(savedData.playerPosx, savedData.playerPosy, savedData.playerPosz);
        }
        else
        {
            SaveSystem.instance.SaveData("", player.position, "", 0, SaveSystem.instance.getAllEnemies());
        }

        StartCoroutine(autoSave());
    }

    IEnumerator autoSave()
    {
        while (true)
        {
            SaveSystem.instance.SaveData(SaveSystem.currentSave.previousScene, player.position, "", 0, SaveSystem.instance.getAllEnemies());
            yield return new WaitForSeconds(30f);
        }
    }
}
