using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MineTrap : Ability
{
    [SerializeField] public float explosionRange = 5f;
    [SerializeField] private int maxDamage;

    public float ExplosionRange { get { return explosionRange; } }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere representing the explosion range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    public void ExplodeMine()
    {
        Destroy(gameObject);
        // play explosion VFX
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision detected with:" + other);
        // Check if the triggering object has the Health script or is tagged as "Enemy"
        if (other.gameObject.TryGetComponent(out Health healthComponent) && other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("collision detected");
            // Apply damage directly to the triggering object
            if (healthComponent != null) healthComponent.TakeDamage(maxDamage);

            // Optionally, you can also apply damage to other nearby colliders using OverlapSphere
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);

            foreach (var hit in hits)
            {
                if (hit.gameObject != other.gameObject && hit.gameObject.TryGetComponent(out Health healthColliders))
                {
                    healthColliders.TakeDamage(maxDamage);
                }
            }

            ExplodeMine();
        }
    }
}
