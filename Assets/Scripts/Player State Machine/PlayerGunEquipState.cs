using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerGunEquipState : PlayerBaseState
{
    //Animation Lerp
    float lerpDuration = 0.3f;
    float valueToLerp;

    public PlayerGunEquipState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("Enter State from GunEquip");
        EquipAnimation(0,1);
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
        EquipAnimation(1,0);
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

    private async void EquipAnimation(float startValue, float endValue)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed/lerpDuration);
            timeElapsed += Time.deltaTime;
            Debug.Log("Lerp: " + startValue);
            Ctx.Animator.SetLayerWeight(1, valueToLerp);
            await Task.Yield();
        }
    }
}
