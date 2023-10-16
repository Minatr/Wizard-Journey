using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Zone[] zones;

    [Header("ENEMIES PREFABS")]
    [SerializeField] private GameObject werewolfPrefab;

    private float raycastDistance = 10.0f;


    private void Awake()
    {
        SavedData savedData = SaveSystem.currentSave;

        // On va regarder si on vient d'une autre zone ou non : si ce n'est pas le cas, on cr�� de nouveaux ennemis, sinon �a veut dire qu'on �tait en combat ou d�connect�, dans ces cas l� on r�cup�re ceux d�j� cr��s auparavant
        if (savedData != null && savedData.previousScene == 1)
        {
            foreach (SavedEnemyData savedEnemyData in savedData.enemyDataList)
            {
                if (!savedEnemyData.attacking)
                {
                    GameObject enemyPrefab = new GameObject();
                    if (savedEnemyData.name == "Werewolf") enemyPrefab = werewolfPrefab;

                    GameObject enemy = Instantiate(enemyPrefab, new Vector3(savedEnemyData.posx, savedEnemyData.posy, savedEnemyData.posz), Quaternion.identity);
                    enemy.GetComponent<EnemyAI>().ChangeLevel(savedEnemyData.level);
                    enemy.GetComponent<EnemyAI>().enemyName = savedEnemyData.name;
                }
            }
        }
        else
        {
            foreach (Zone zone in zones)
            {
                SpawnEnemies(zone);
            }
        }
    }

    private void SpawnEnemies(Zone zone)
    {
        for (int i = 0; i < zone.numberOfEnemiesToSpawn; i++)
        {
            // On cr�� une position al�atoire
            Vector3 randomPosition = new Vector3(Random.Range(zone.minX, zone.maxX), 0, Random.Range(zone.minZ, zone.maxZ));

            // On r�cup�re la hauteur du terrain en cette position al�atoire
            Ray ray = new Ray(randomPosition + Vector3.up * raycastDistance, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance + 1))
            {
                randomPosition = hit.point;
            }
            else
            {
                continue;
            }

            Instantiate(zone.enemyPrefab[0], randomPosition, Quaternion.identity);
        }
    }
}

[System.Serializable]
public class Zone
{
    public GameObject[] enemyPrefab;
    public int numberOfEnemiesToSpawn;
    public int minX;
    public int maxX;
    public int minZ;
    public int maxZ;
}