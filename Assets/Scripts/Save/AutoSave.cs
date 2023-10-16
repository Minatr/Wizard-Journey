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
            player.position = new Vector3(savedData.playerPosx, savedData.playerPosy, savedData.playerPosz);
        }
        else
        {
            SaveSystem.instance.SaveData(-1, player.position, "", 0, SaveSystem.instance.getAllEnemies());
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
