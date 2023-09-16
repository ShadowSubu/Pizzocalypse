using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGunFireState : PlayerBaseState
{
    public PlayerGunFireState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    bool isShootingOver;

    public override void EnterState()
    {
        Ctx.IsShooting = false;
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
        Ctx.IsRotating = false;
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
        //Debug.Log("Duration: " + (int)Ctx.ActiveGun.ShootingDuration);
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
            if (Ctx.ActiveGun.AmmoAmount >= 0 && Ctx.ActiveGun.CurrentMagSize > 0 && !Ctx.IsReloading)
            {
                switch (Ctx.ActiveGun.GunType)
                {
                    case GunType.Pistol:
                        PistolShoot();
                        break;
                    case GunType.Shotgun:
                        break;
                    case GunType.Rifle:
                        RifleShooting();
                        break;
                    default:
                        break;
                }

            }
        }           
    }

    void PistolShoot()
    {
        Bullet bullet = Object.Instantiate(Ctx.BulletPrefab, Ctx.ActiveGun.ShootingPoint.position, Ctx.transform.rotation);
        AudioManager.Instance.Play("Pizzocalypse-Pistol");
    }

    void ShotgunShooting()
    {

    }

    async void RifleShooting()
    {
        for (int i = 0; i < 3; i++)
        {
            Bullet bullet = Object.Instantiate(Ctx.BulletPrefab, Ctx.ActiveGun.ShootingPoint.position, Ctx.transform.rotation);
            AudioManager.Instance.Play("Pizzocalypse-Rifle");
            await Task.Delay(100);
        }
    }
}
