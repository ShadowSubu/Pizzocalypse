using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunEquipState : PlayerBaseState
{
    public PlayerGunEquipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("Enter State from GunEquip");
        Ctx.Animator.SetLayerWeight(1, 1f);
    }

    public override void UpdateState()
    {
        Debug.Log("Update State from GunEquip");
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit State from GunEquip");
        Ctx.RequireNewGunToggle = true;
        Ctx.Animator.SetLayerWeight(1, 0f);
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsGunToggled && !Ctx.RequireNewGunToggle && Ctx.ActiveGun == null)
        {
            SwitchState(Factory.None());
        }
    }

    public override void InitializeSubState()
    {
        
    }
}
