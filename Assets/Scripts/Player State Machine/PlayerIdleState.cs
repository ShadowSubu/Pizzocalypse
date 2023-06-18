using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) {
        IsRootState = true;
    }


    public override void EnterState()
    {
        Debug.Log("Enter State from Idle");
        Ctx.AppliedMovementX = 0f;
        Ctx.AppliedMovementZ = 0f;
    }
    public override void UpdateState()
    {
        Debug.Log("Update State from Idle");
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit State from Idle");
    }
    public override void CheckSwitchState()
    {
        if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState()
    {
    }

}
