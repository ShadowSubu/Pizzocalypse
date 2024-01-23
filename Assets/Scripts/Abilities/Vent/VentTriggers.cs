using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VentTriggers : MonoBehaviour
{
    private Vent vent;
    public Vent Vent => vent;
    public bool isInTrigger;
    //public UnityEvent<bool> 

    public void Initialize(Vent vent)
    {
        this.vent = vent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStateMachine player))
        {
            //Debug.Log("Ready to vent");
            isInTrigger = true;
            player.SetVent(Vent);
        }
        // SHOW PLAYER THAT VENT IS ENABLED
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStateMachine player))
        {
            isInTrigger = false;
            player.SetVent(null);
        }
        // SHOW PLAYER THAT VENT IS DISABLED
    }
}
