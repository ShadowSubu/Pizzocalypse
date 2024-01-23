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
        if (other.gameObject.TryGetComponent(out PlayerStateMachine _))
        {
            //Debug.Log("Ready to vent");
            isInTrigger = true;
        }
        // SHOW PLAYER THAT VENT IS ENABLED
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStateMachine _))
        {
            isInTrigger = false;
        }
        // SHOW PLAYER THAT VENT IS DISABLED
    }
}
