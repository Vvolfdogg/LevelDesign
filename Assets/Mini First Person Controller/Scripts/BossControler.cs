using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class BossControler : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] Transform player;

    [SerializeField] LayerMask whatIsGround, whatIsPlayer;
    [SerializeField] float timeBetweenAttacks;
    private bool alreadyAttacked;

    [SerializeField] float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;

    public int health = 300;
    public float speed = 3f;
    [SerializeField] FirstPersonMovement playerTakeDamage;
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
        playerTakeDamage = GameObject.Find("First Person Controller").GetComponent<FirstPersonMovement>();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInSightRange && !playerInAttackRange) Chasing();
        else if (playerInSightRange && playerInAttackRange) Attacking();


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
            playerTakeDamage.PlayerTakeDamage(10f);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}