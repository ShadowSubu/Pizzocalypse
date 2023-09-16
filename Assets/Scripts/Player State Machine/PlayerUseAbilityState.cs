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
    bool isVenting = false;

    public override void EnterState()
    {
        Debug.Log("Enter UseAbilityState");
        //Ctx.IsAbilityTrigerred = false;
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
        if(isAbilityUsed && Ctx.ActiveGun == null && !isVenting)
        {
            SwitchState(Factory.None());
        }
        else if(Ctx.ActiveGun != null && !isVenting && !Ctx.IsShooting)
        {
            SwitchState(Factory.GunEquip());
        }
        else if(Ctx.ActiveGun != null && !isVenting && Ctx.IsShooting)
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

            default:
                break;
        }            
    }

    
    async private void ExecuteNoReload()
    {
        Debug.Log("Executing NoReload");
        int temp = Ctx.ActiveGun.MagSize;

        // Set the currentMagSize to infinity
        Ctx.ActiveGun.CurrentMagSize = int.MaxValue;

        await Task.Delay(10000);

        isAbilityUsed = true;

        // Reset the currentMagSize to its original value
        Ctx.ActiveGun.CurrentMagSize = temp;
    }

    void ExecuteVent()
    {
        //Delay(1500);
        
        if(!isVenting && !Ctx.IsShooting)
        {
            isVenting = true;
            Debug.Log("Executing Vent Ability");
            Ctx.CurrentVent.UseAbility();
        }
        
        // Wait for 10 seconds
        

        // Mark the ability as used
        isAbilityUsed = true;
        isVenting = false;
    }

   
}
