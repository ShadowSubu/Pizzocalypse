using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform Player;
    public float AttackRange;
    float AttackTime;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
       
      
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.LookAt(Player);
        float distance = Vector3.Distance(Player.position, animator.transform.position);
        if (distance > AttackRange)
        {
            animator.SetBool("IsAttacking", false);
        }

        if (distance < 1.5)
        {
            animator.SetBool("NormalAttack", true);
        }
  
       
    }

  


}
