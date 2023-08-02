using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieClass : MonoBehaviour
{
    public int HP = 100;
    public Animator animator;

    public void TakeDamage(int DamageAmount)
    {
        HP -= DamageAmount;
        if(HP <= 0)
        {
            animator.SetTrigger("IsDead");
        }
        else
        {
            animator.SetTrigger("Damage");
        }
    }
}
