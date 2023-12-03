using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentPickup : AbilityPickup
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.TryGetComponent(out PlayerStateMachine player))
                player.EquipAbility(AbilityType.Vent);
        }
    }
}
