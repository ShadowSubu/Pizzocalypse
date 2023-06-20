using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFireState : PlayerBaseState
{
    public PlayerGunFireState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Enter State from GunFire");
    }

    public override void UpdateState()
    {
        Debug.Log("Enter State from GunFire");
    }

    public override void ExitState()
    {
        Debug.Log("Enter State from GunFire");
    }

    public override void CheckSwitchState()
    {
    }

    public override void InitializeSubState()
    {
    }

}
