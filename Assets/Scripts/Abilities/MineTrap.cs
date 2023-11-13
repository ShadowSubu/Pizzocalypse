using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTrap : MonoBehaviour
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
        //play explosion VFX
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Health _))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Health healthColliders))
                {
                    healthColliders.TakeDamage(maxDamage);
                }
            }
            ExplodeMine();
        }
    }

}
