using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerGunEquipState : PlayerBaseState
{
    

    public PlayerGunEquipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("Enter State from GunEquip");
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
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsGunToggled && !Ctx.RequireNewGunToggle && Ctx.ActiveGun == null)
        {
            SwitchState(Factory.None());
            Ctx.EquipAnimation(1, 0);
        }
        if (Ctx.ActiveGun != null && Ctx.IsShooting)
        {
            SwitchState(Factory.GunFire());
        }
    }

    public override void InitializeSubState()
    {
        
    }

    
}
