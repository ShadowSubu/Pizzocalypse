using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Damager : MonoBehaviour
{
    [SerializeField] private int damageAmount;
    [SerializeField] ParticleSystem hitParticlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damageAmount);
            if (hitParticlePrefab != null) hitParticlePrefab.Play();
            DestroyObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damageAmount);
            if (hitParticlePrefab != null) hitParticlePrefab.Play();
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        if (TryGetComponent<Bullet>(out Bullet bullet))
        {
            Destroy(gameObject);
        }
    }
}
