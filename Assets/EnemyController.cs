using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookingRadius = 10f;
    Transform target;
    public Transform player;
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public Vector3 walkingPoint;
    bool walkingpointSet;
    public float walkingRange;
    public float betweenTimeAttack;
    bool alreadyAttack;
    public float enemyHealth;
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInattackRange;
    Animator animm;
    public GameObject projectTile;
    Collider thisCol;





    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animm = GetComponent<Animator>();
        thisCol = GetComponent<Collider>();
    }
    void Start()
    {
        target = PlayerManager.instance.player.transform;


    }


    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInattackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInattackRange)
        {
            animm.SetBool("LoafAround", true);
            Patrolling();
        }
        if (playerInSightRange && !playerInattackRange)
        {
            animm.SetBool("Walking", true);
            ChasePlayer();
        }
        if (playerInattackRange && playerInSightRange)
        {
            animm.SetTrigger("Attack");
            AttackPlayer();
        }

    }




    private void Patrolling()
    {
        if (!walkingpointSet)
        {
            SearchWalkingPoint();
        }
        if (walkingpointSet)
        {

            agent.SetDestination(walkingPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkingPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkingpointSet = false;
        }
    }
    private void SearchWalkingPoint()
    {
        float randomZ = Random.Range(-walkingRange, walkingRange);
        float randomX = Random.Range(-walkingRange, walkingRange);
        walkingPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkingPoint, -transform.up, 2f, whatIsGround))
        {
            walkingpointSet = true;
        }

    }

    private void ChasePlayer()
    {

        agent.SetDestination(player.position);
        Debug.Log(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttack)
        {

            Rigidbody rigid = Instantiate(projectTile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigid.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rigid.AddForce(transform.up * 8f, ForceMode.Impulse);
            alreadyAttack = true;
            Invoke(nameof(ResetAttack), betweenTimeAttack);

        }
    }
    private void ResetAttack()
    {
        alreadyAttack = false;
    }
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;





        if (enemyHealth <= 0)
        {
            animm.SetTrigger("Die");
            animm.SetBool("AttackingDestroy", false);
            //animm.GetComponent<Animator>().enabled = false;
            Invoke(nameof(DestroyEnemy), 2f);
        }
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {

        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
