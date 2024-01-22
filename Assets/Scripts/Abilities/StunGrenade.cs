using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class StunGrenade : Ability
{
    [SerializeField] private float explosionDelay;
    [SerializeField] private float thowingForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float stunDuration;

    private Vector3 direction;

    private void Start()
    {
        

    }

    public void Initialize(Vector3 direction)
    {
        this.direction = direction;
        Throw();
    }

    private void Throw()
    {
        GetComponent<Rigidbody>().AddForce(direction * thowingForce, ForceMode.Impulse);
        StartCountdown();
    }

    private async void StartCountdown()
    {
        await Task.Delay((int)(1000 * explosionDelay));

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var hit in hits)
        {
            if ((hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("RangedEnemy")) && hit.gameObject.TryGetComponent(out ZombieAI zombie))
            {
                zombie.Stun(stunDuration);
                Debug.LogWarning("Stunned Zombies: " + zombie.ToString());
            }
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere representing the explosion range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
