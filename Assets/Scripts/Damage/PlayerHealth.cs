using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] VoidEventSO pizzaDamageEvent = default;
    [SerializeField] VoidEventSO onPlayerDieEvent = default;

    private void OnEnable()
    {
        onPlayerDieEvent.OnEventRaised += OnPLayerDie;
    }

    public override void TakeDamage(int amount)
    {
        pizzaDamageEvent.RaiseEvent();
    }

    public void OnPLayerDie()
    {
        // Code Related to Death goes here
    }
}
