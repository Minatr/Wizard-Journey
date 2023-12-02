using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform player;
    [SerializeField] private GestionSpell gestionSpell;

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
            SaveSystem.instance.SaveData(
                "",
                player.position,
                3,
                new SpellSave("lightning", "shield", new Dictionary<string, SpellType> 
                    { 
                        {"lightning", new SpellType(1, 3, 60)},
                        {"fire", new SpellType(0, 2, 40)},
                        {"ice", new SpellType(0, 2, 30)},
                        {"shield", new SpellType(1, 1, 30)},
                        {"heal", new SpellType(0, 2, 60)},
                        {"buffAttack", new SpellType(0, 1, 50)}
                    }
                ),
                "",
                0,
                SaveSystem.instance.getAllEnemies()
            );
            // On initialise les sorts pour la scène depuis cet endroit s'il n'y avait pas de sauvegarde existante
            gestionSpell.InitSpells();
        }

        StartCoroutine(autoSave());
    }

    IEnumerator autoSave()
    {
        while (true)
        {
            SaveSystem.instance.SaveData(SaveSystem.currentSave.previousScene, player.position, 3, SaveSystem.currentSave.spellSave, "", 0, SaveSystem.instance.getAllEnemies());
            yield return new WaitForSeconds(30f);
        }
    }
}
