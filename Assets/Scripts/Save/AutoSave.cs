using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private SaveSystem save;
    [SerializeField] private Transform player;

    void Start()
    {
        SavedData savedData = save.LoadData();
        if (savedData != null)
        {
            player.position = new Vector3(savedData.playerPosx, savedData.playerPosy, savedData.playerPosz);
        }

        StartCoroutine(autoSave());
    }

    IEnumerator autoSave()
    {
        while (true)
        {
            save.SaveData(player.position, "", 0);
            yield return new WaitForSeconds(30f);
        }
    }
}
