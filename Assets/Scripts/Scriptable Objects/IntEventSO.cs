using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Int Event Channel")]
public class IntEventSO : ScriptableObject
{
    public UnityAction<int, int, int> OnEventRaised;

    public void RaiseEvent(int ammoInGun, int maxAmmo, int totalAmmoStock)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(ammoInGun, maxAmmo, totalAmmoStock);
    }
}
