using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] Transform player;
    private Animator animator;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    private Vector3 walkPoint;

    private bool walkPointSet;
    [SerializeField] float walkPointRange;
    [SerializeField] float timeBetweenAttacks;
    private bool alreadyAttacked;

    [SerializeField] float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    [SerializeField] int health = 3;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenNewDestinations;
    private bool waitingBeforeWalking = false;
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health == 0)
        {
            Die();
        }
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        else if (playerInSightRange && !playerInAttackRange) Chasing();
        else if (playerInSightRange && playerInAttackRange) Attacking();


    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else if (walkPointSet && !waitingBeforeWalking)
        {
            waitingBeforeWalking = true;
            Invoke("WalkingToDestination", timeBetweenNewDestinations);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 0.1f)
        {
            waitingBeforeWalking = false;
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
    }

    private void Chasing()
    {
        agent.SetDestination(player.position);
    }

    private void Attacking()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void WalkingToDestination()
    {
        agent.SetDestination(walkPoint);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}