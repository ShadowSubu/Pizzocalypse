using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : ZombieClass
{
    float Patroltime;
    List<Transform> WayPoints = new List<Transform>(); 
    NavMeshAgent ZombieAgent;

    Transform Player;
    [SerializeField] float TouchDistance;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform ;
        ZombieAgent = animator.GetComponent<NavMeshAgent>();
        Patroltime = 0;
        GameObject Points = GameObject.FindGameObjectWithTag(waypoints);
        foreach(Transform T in Points.transform)
        {
            WayPoints.Add(T);
        }

        ZombieAgent.SetDestination(WayPoints[Random.Range(0, WayPoints.Count)].position);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(ZombieAgent.remainingDistance <= ZombieAgent.stoppingDistance)
        {
            ZombieAgent.SetDestination(WayPoints[Random.Range(0, WayPoints.Count)].position);
        }
        Patroltime += Time.deltaTime;
        if (Patroltime > 15)
        {
            animator.SetBool("IsPatrolling", false);
        }

        float distance = Vector3.Distance(Player.position, animator.transform.position);
        if (distance < TouchDistance)
        {
            animator.SetBool("IsChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ZombieAgent.SetDestination(ZombieAgent.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
