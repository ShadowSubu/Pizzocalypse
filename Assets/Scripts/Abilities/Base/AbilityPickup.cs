using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AbilityPickup : MonoBehaviour
{
    [SerializeField] AbilityType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerStateMachine>().SetAbility(type))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Has Ability");
            }
        }
    }
}
