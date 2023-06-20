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
        Debug.Log("Enter State from GunEquip");
        if (Ctx.IsGunSelected)
        {
            ExitState();
            InitializeSubState();
        }
    }

    public override void ExitState()
    {
        Debug.Log("Enter State from GunEquip");
        Ctx.Animator.SetLayerWeight(1, 0f);
    }

    public override void CheckSwitchState()
    {
    }

    public override void InitializeSubState()
    {
        
    }
}
