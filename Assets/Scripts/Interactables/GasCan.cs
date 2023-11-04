using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCan : MonoBehaviour
{
    [SerializeField] public int hitPoints = 3;
    [SerializeField] public float explosionRange = 5f;
    [SerializeField] private int maxDamage;

    public float ExplosionRange { get { return explosionRange; } }
    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere representing the explosion range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }

    public void ExplodeGasCan()
    {
        Destroy(gameObject);
        //play explosion VFX
    }
    private void ExplodeIfInRange(GasCan gasCan)
    {
        float explosionRange = gasCan.ExplosionRange;
        Collider[] hits = Physics.OverlapSphere(gasCan.transform.position, explosionRange);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(maxDamage);
            }
        }

        gasCan.ExplodeGasCan();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Bullet>(out _))
        {
            hitPoints--;
            if (hitPoints <= 0)
            {
                ExplodeIfInRange(this);
            }
        }
    }
}
