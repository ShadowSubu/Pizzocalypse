using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent ZombieAgent;
    Transform Player;
    public float StopChaseDis;
    public float AttackRange;
    public float ChaseSpeed;
    float BeforeSpeed;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ZombieAgent = animator.GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        BeforeSpeed = ZombieAgent.speed;
        ZombieAgent.speed = ChaseSpeed;
    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ZombieAgent.SetDestination(Player.position);
        float distance = Vector3.Distance(Player.position, animator.transform.position);
        if (distance > StopChaseDis)
        {
            animator.SetBool("IsChasing", false);
        }
        if (distance < AttackRange)
        {
            animator.SetBool("IsAttacking", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ZombieAgent.SetDestination(animator.transform.position);
        ZombieAgent.speed = BeforeSpeed;
    }

   
}
