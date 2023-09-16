using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoneState : PlayerBaseState
{
    public PlayerNoneState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        Debug.Log("Enter State from NoneState");
    }

    public override void UpdateState()
    {
        Debug.Log("Update State from NoneState");
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit State from NoneState");
        Ctx.RequireNewGunToggle = true;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsGunToggled && !Ctx.RequireNewGunToggle)
        {
            SwitchState(Factory.GunEquip());
            Ctx.EquipAnimation(0, 1);
        }
        if(Ctx.IsAbilityTrigerred)
        {
            SwitchState(Factory.UseAbility());
        }
    }

    public override void InitializeSubState()
    {
    }

}
