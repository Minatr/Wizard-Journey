using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator enemyANI;
    [SerializeField] private SpriteRenderer sr;
    private Transform player;
    private Rigidbody playerRB;
    private Animator playerANI;
    private Rigidbody enemyRB;
    private NavMeshAgent enemyNMA;
    private LevelLoader levelLoader;

    [Header("STATS")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float combatRadius;
    private bool hasDestination;

    [Header("WANDERING PARAMETERS")]
    [SerializeField] private float wanderingWaitTimeMin;
    [SerializeField] private float wanderingWaitTimeMax;
    [SerializeField] private float wanderingDistanceMin;
    [SerializeField] private float wanderingDistanceMax;


    void Start()
    {
        // On récupère les différents éléments de la scène
        player = GameObject.Find("Player").transform;
        playerRB = player.GetComponent<Rigidbody>();
        playerANI = player.GetComponentInChildren<Animator>();
        enemyRB = transform.GetComponent<Rigidbody>();
        enemyNMA = transform.GetComponent<NavMeshAgent>();
        levelLoader = GameObject.Find("GameManager").GetComponent<LevelLoader>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < combatRadius)
        {
            // On freeze le joueur
            playerANI.enabled = false;
            playerRB.constraints = RigidbodyConstraints.FreezeAll;

            // On freeze l'ennemi
            enemyANI.enabled = false;
            enemyRB.constraints = RigidbodyConstraints.FreezeAll;
            enemyNMA.enabled = false;    // Pas trop compris pourquoi, mais le NavMeshAgent (NMA) fait glisser l'ennemi même en le stoppant intégralement,
            enemyNMA.enabled = true;     // La seule solution potable reste donc de désactiver et réactiver le NMA pour ne pas avoir d'erreurs.

            // Lancement du combat
            levelLoader.LoadCombat();
        }

        if (Vector3.Distance(player.position, transform.position) < detectionRadius)
        {
            agent.speed = chaseSpeed;

            agent.SetDestination(player.position);
        }
        else
        {
            agent.speed = walkSpeed;

            if (agent.remainingDistance < 2f && !hasDestination)
            {
                StartCoroutine(GetNewDestination());
            }
        }

        enemyANI.SetFloat("Speed", agent.velocity.magnitude);

        // Gère l'orientation du Sprite lors de son déplacement
        setOrientation();
    }

    private void setOrientation()
    {
        Vector3 movementDirection = agent.velocity.normalized;

        if (movementDirection.x > 0 && sr.flipX)
        {
            sr.flipX = false;
        }
        else if (movementDirection.x < 0 && !sr.flipX)
        {
            sr.flipX = true;
        }
    }

    IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) * new Vector3(Random.Range(-1f, 1), 0f, Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        hasDestination = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combatRadius);
    }
}
