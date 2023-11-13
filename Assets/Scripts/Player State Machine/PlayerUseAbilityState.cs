using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class PlayerUseAbilityState : PlayerBaseState
{
    public PlayerUseAbilityState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
     : base(currentContext, playerStateFactory) { }

    bool isAbilityUsed;

    public override void EnterState()
    {
        Debug.Log("Enter UseAbilityState");
        isAbilityUsed = false;
        ExecuteAbility();
    }

    public override void UpdateState()
    {
        Debug.Log("Update from UseAbilityState");
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Ctx.IsAbilityTrigerred = false;
        Debug.Log("Exit UseAbilityState");
    }

    public override void CheckSwitchState()
    {
        if(isAbilityUsed && Ctx.ActiveGun == null )
        {
            SwitchState(Factory.None());
        }
        else if(Ctx.ActiveGun != null && !Ctx.IsShooting)
        {
            SwitchState(Factory.GunEquip());
        }

        else if (Ctx.ActiveGun != null && !Ctx.IsGunToggled && Ctx.IsShooting)
        {
            SwitchState(Factory.GunFire());
        }
    }

    public override void InitializeSubState(){}

    private void ExecuteAbility()
    {
        switch (Ctx.AbilityTrigerred.AbilityType)
        {
            case AbilityType.NoReload:
                ExecuteNoReload();
                break;

            case AbilityType.Vent:
                ExecuteVent();
                break;

            case AbilityType.GunSwitch:
                ExecuteGunSwitch();
                break;

            default:
                break;
        }            
    }

    private void ExecuteNoReload()
    {
        Ctx.NoReload.UseAbility();
        isAbilityUsed = true;
    }

    private void ExecuteVent()
    { 
        Ctx.CurrentVent.UseAbility();
        isAbilityUsed = true;   
    }

    private void ExecuteGunSwitch()
    {
        Ctx.GunSwitch.UseAbility();
        isAbilityUsed = true;   
    }
}
