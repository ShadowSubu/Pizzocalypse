using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    public Transform[] patrolPoints;

    [Header("StateSpeeds")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 4f;

    [Header("StateTimings")]
    public float IdleTime = 5f;
    public float Patroltime = 8f;

    [Header("AttackConditions")]
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float TouchDistance = 1.5f;


    [Space(5)]
    public float stopChasingDis = 8f;

    float distance;



    private int currentPatrolPointIndex;
    private Transform player;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private float nextAttackTime;
    float BeforeSpeed;


    float patrolRunTime;
    float RunTime;

    [HideInInspector]
    public enum ZombieState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Dead,
        GotHit
    }
    [HideInInspector]
    public ZombieState currentState = ZombieState.Idle;

    public void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Start in the Idle state
        SetState(ZombieState.Idle);
        if(currentState == ZombieState.Patrol)
        {
            navMeshAgent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].position);
        }
  
    }

    public void Update()
    {
        switch (currentState)
        {
            case ZombieState.Idle:
                Idle();
                break;

            case ZombieState.Patrol:
                Patrol();
                break;

            case ZombieState.Chase:
                Chase();
                break;

            case ZombieState.Attack:
                Attack();
                break;
            
            case ZombieState.Dead:
                Death();
                break;
            case ZombieState.GotHit:
                GotHit();
                break;



        }
    }

    public void SetState(ZombieState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;
        // Implement state-specific setup or behavior changes here

        // Example: Handle animation transitions
        switch (currentState)
        {
            case ZombieState.Idle:
                animator.SetBool("IsPatrolling", false);
                RunTime = 0;
                navMeshAgent.SetDestination(transform.position);
                break;

            case ZombieState.Patrol:
                animator.SetBool("IsPatrolling", true);
                animator.SetBool("IsChasing", false);
                patrolRunTime = 0;
                break;

            case ZombieState.Chase:
                animator.SetBool("IsChasing", true);
                animator.SetBool("IsAttacking", false);
                BeforeSpeed = navMeshAgent.speed;
                navMeshAgent.speed = chaseSpeed;
                break;

            case ZombieState.Attack:
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsChasing", false);
                break;

            case ZombieState.Dead:
                animator.SetTrigger("IsDead");
                animator.SetBool("IsPatrolling", false);
                animator.SetBool("IsChasing", false);
                animator.SetBool("IsAttacking", false);
                break;

            case ZombieState.GotHit:
               
                break;
        }
    }

    private void Idle()
    {
        RunTime += Time.deltaTime;
        if (RunTime > IdleTime)
        {
            SetState(ZombieState.Patrol);
        }
         distance = Vector3.Distance(player.position, transform.position);
        if (distance < TouchDistance)
        {
            SetState(ZombieState.Chase);
        }
    }
    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;
    
       // float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Length)].position);
        }
        patrolRunTime += Time.deltaTime;
        if (patrolRunTime > Patroltime)
        {
            SetState(ZombieState.Idle);   
        }

        // Check for the player and switch to Run state if close
         distance = Vector3.Distance(player.position, transform.position);
       if (distance < TouchDistance)
        {
            SetState(ZombieState.Chase);
            navMeshAgent.speed = chaseSpeed;
            navMeshAgent.SetDestination(player.position);
        }
    }

    public void Chase()
    {
        navMeshAgent.SetDestination(player.position);
        distance = Vector3.Distance(player.position, transform.position);
        if (distance > stopChasingDis)
        {
            SetState(ZombieState.Patrol);
            navMeshAgent.speed = BeforeSpeed;
        }
        if (distance < attackRange)
        {
            SetState(ZombieState.Attack);
            navMeshAgent.SetDestination(transform.position);
            navMeshAgent.speed = BeforeSpeed;
        }
    }

    public void Attack()
    {

        animator.transform.LookAt(player);
        distance = Vector3.Distance(player.position, transform.position);
        // Check if it's time to attack again
        if (Time.time >= nextAttackTime)
        {
            /* if (distance < 1.5)
            {
                animator.SetBool("NormalAttack", true);
            }*/
            // Implement attack logic here
            // You can use raycasting or other methods to detect if the player is within attack range

            // If the player is within range, deal damage or trigger attack animation

            // Set the next attack time based on attack cooldown
            nextAttackTime = Time.time + attackCooldown;
        }

        // If the player moves away, switch back to Patrol
        if (distance > attackRange)
        {
            SetState(ZombieState.Chase);
            navMeshAgent.speed = chaseSpeed;
        }
    }

    public void Death()
    {
        SetState(ZombieState.Dead);
    }
    public void GotHit()
    {
        animator.SetTrigger("Damage");
    }
}
        
    

