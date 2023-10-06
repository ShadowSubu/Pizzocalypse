using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 1.5f;
    public float runSpeed = 4f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private int currentPatrolPointIndex = 0;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float nextAttackTime = 0f;

    private enum ZombieState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }

    private ZombieState currentState = ZombieState.Idle;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start in the Idle state
        SetState(ZombieState.Idle);
    }

    private void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                // Implement Idle behavior here
                break;

            case ZombieState.Patrol:
                Patrol();
                break;

            case ZombieState.Chase:
                // Implement Run behavior here
                break;

            case ZombieState.Attack:
                Attack();
                break;
        }
    }

    private void SetState(ZombieState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        // Implement state-specific setup or behavior changes here

        // Example: Handle animation transitions
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("IsWalking", false);
                break;

            case ZombieState.Patrol:
                animator.SetBool("IsWalking", true);
                break;

            case ZombieState.Chase:
                // Set run animations and behavior
                break;

            case ZombieState.Attack:
                animator.SetTrigger("Attack");
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        // Calculate distance to the current patrol point
        float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position);

        // Check if we are close to the patrol point
        if (distanceToPatrolPoint < 0.1f)
        {
            // Move to the next patrol point
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            navMeshAgent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        }

        // Check for the player and switch to Run state if close
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            SetState(ZombieState.Chase);
            navMeshAgent.speed = runSpeed;
            navMeshAgent.SetDestination(player.position);
        }
    }

    private void Attack()
    {
        // Check if it's time to attack again
        if (Time.time >= nextAttackTime)
        {
            // Implement attack logic here
            // You can use raycasting or other methods to detect if the player is within attack range

            // If the player is within range, deal damage or trigger attack animation

            // Set the next attack time based on attack cooldown
            nextAttackTime = Time.time + attackCooldown;
        }

        // If the player moves away, switch back to Patrol
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            SetState(ZombieState.Patrol);
            navMeshAgent.speed = patrolSpeed;
        }
    }
}
        
    

