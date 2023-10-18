using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Health : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    // Subscribe to this event to get notified when this unit dies
    public UnityEvent OnDeath;
    public UnityEvent OnHit;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth > 0)
        {
            OnHit?.Invoke();
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath?.Invoke();
            
        }
    }
}
