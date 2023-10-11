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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    public override void TakeDamage(int amount)
    {
        Debug.Log("Pizza Damaged - 1");
        pizzaDamageEvent.RaiseEvent();
    }

    public void OnPLayerDie()
    {
        // Code Related to Death goes here
    }
}
