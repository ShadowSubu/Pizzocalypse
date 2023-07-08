using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerGunFireState : PlayerBaseState
{
    public PlayerGunFireState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    bool isShootingOver;
    public override void EnterState()
    {
        Debug.Log("Enter State from GunFire");
        StartShootingDuration();
        AnimateGun();
        ShootBullet();
    }

    public override void UpdateState()
    {
        Debug.Log("Update State from GunFire");
        CheckSwitchState();
    }

    public override void ExitState()
    {
        Debug.Log("Exit State from GunFire");
        Ctx.IsShooting = false;
        Ctx.RequireNewGunToggle = false;
    }

    public override void CheckSwitchState()
    {
        if (isShootingOver)
        {
            SwitchState(Factory.GunEquip());
        }
    }

    public override void InitializeSubState()
    {
    }

    async void StartShootingDuration()
    {
        isShootingOver = false;
        await Task.Delay((int)Ctx.ActiveGun.ShootingDuration);
        Debug.Log("Duration: " + (int)Ctx.ActiveGun.ShootingDuration);
        SetAnimationFalse();
        isShootingOver = true;
    }

    void AnimateGun()
    {
        switch (Ctx.ActiveGun.GunType)
        {
            case GunType.Pistol:
                Ctx.Animator.SetBool(Ctx.IsPistolFireHash, true);
                break;
            case GunType.Shotgun:
                Ctx.Animator.SetBool(Ctx.IsShotgunFireHash, true);
                break;
            case GunType.Rifle:
                Ctx.Animator.SetBool(Ctx.IsRifleFireHash, true);
                break;
            default:
                break;
        }
    }

    void SetAnimationFalse()
    {
        Ctx.Animator.SetBool(Ctx.IsPistolFireHash, false);
        Ctx.Animator.SetBool(Ctx.IsShotgunFireHash, false);
        Ctx.Animator.SetBool(Ctx.IsRifleFireHash, false);
    }

    void ShootBullet()
    {
        if (Ctx.ActiveGun != null && Ctx.ActiveGun.ShootingPoint != null)
        {
            Bullet bullet = Object.Instantiate(Ctx.BulletPrefab, Ctx.ActiveGun.ShootingPoint.position, Ctx.transform.rotation);
            
        }
    }
}
