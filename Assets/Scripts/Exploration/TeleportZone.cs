using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TeleportZone : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private GameObject boutonTeleport;
    private Transform player;
    private LevelLoader levelLoader;

    [Header("TELEPORT PARAMETERS")]
    [SerializeField] private float detectionRadius = 7;


    void Start()
    {
        player = GameObject.Find("Player").transform;
        levelLoader = GameObject.Find("GameManagerZone").GetComponent<LevelLoader>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < detectionRadius)
        {
            if (!boutonTeleport.activeSelf)
            {
                boutonTeleport.SetActive(true);
            }
        } else if (boutonTeleport.activeSelf)
        {
            boutonTeleport.SetActive(false);
        }
    }

    public void Teleport()
    {
        if (SaveSystem.currentSave.actualScene == "Zone1")
        {
            levelLoader.LoadZone("Zone2");
        } else
        {
            levelLoader.LoadZone("Zone1");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
