using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }


    public override void EnterState()
    {
        //Debug.Log("Enter State from Run");
        InitializeSubState();
        Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
    }
    public override void UpdateState()
    {
        //Debug.Log("Update State from Run");
        Run();
        CheckSwitchState();
    }
    private void Run()
    {
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
        //Debug.Log("movement:" + Ctx.CurrentMovementInput);
        AudioManager.Instance.PlayWalkSFX();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit State from Run");
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
    }
    public override void CheckSwitchState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsGunToggled && Ctx.ActiveGun == null)
        {
            SetSubState(Factory.None());
        }
        else if (Ctx.ActiveGun != null)
        {
            SetSubState(Factory.GunEquip());
        }
        else if (Ctx.IsAbilityTrigerred)
        {
            SetSubState(Factory.UseAbility());
        }
    }

}
