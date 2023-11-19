using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VentTriggers : MonoBehaviour
{
    public bool isInTrigger;
    //public UnityEvent<bool> 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInTrigger = true;
        }
        // SHOW PLAYER THAT VENT IS ENABLED
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInTrigger = false;
        }
        // SHOW PLAYER THAT VENT IS DISABLED
    }
}
