using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSwitch : Abilities
{
    private bool canSwitchGun = true; // Flag to allow or prevent gun switching
    

    async public override void UseAbility()
    {
        int num = UnityEngine.Random.Range(0, Context.Guns.Length);
        Debug.Log("Using Gun Switch");
        if (!canSwitchGun)
        {
            // The player can't switch guns right now, so return early.
            return;
        }

        
        ToggleGun(num);
        

        // Wait for 10 seconds
        await Task.Delay(10000);

        // After 10 seconds, prevent further gun switching
        canSwitchGun = false;


        /* Context.PlayerInput.CharacterControls.EquipGun.started += OnEquipPistol;
         Context.PlayerInput.CharacterControls.EquipGun.canceled += OnEquipPistol;
         Context.PlayerInput.CharacterControls.EquipShotgun.started += OnEquipShotgun;
         Context.PlayerInput.CharacterControls.EquipShotgun.canceled += OnEquipShotgun;
         Context.PlayerInput.CharacterControls.EquipRifle.started += OnEquipRifle;
         Context.PlayerInput.CharacterControls.EquipRifle.canceled += OnEquipRifle;*/

    }

    /*    void OnEquipPistol(InputAction.CallbackContext context)
        {
            if (!canSwitchGun)
            {
                // The player can't switch guns right now, so return early.
                return;
            }

            Context.IsGunToggled = context.ReadValueAsButton();
            if (Context.IsGunToggled)
            {
                ToggleGun(0);
            }
        }

        void OnEquipShotgun(InputAction.CallbackContext context)
        {
            if (!canSwitchGun)
            {
                // The player can't switch guns right now, so return early.
                return;
            }

            Context.IsGunToggled = context.ReadValueAsButton();
            if (Context.IsGunToggled)
            {
                ToggleGun(1);
            }
        }

        void OnEquipRifle(InputAction.CallbackContext context)
        {
            if (!canSwitchGun)
            {
                // The player can't switch guns right now, so return early.
                return;
            }

            Context.IsGunToggled = context.ReadValueAsButton();
            if (Context.IsGunToggled)
            {
                ToggleGun(2);
            }
        }*/

    private void ToggleGun(int num)
    {
        Debug.Log("Changing Gun");
        if (Context.ActiveGun == null || Context.ActiveGun != Context.Guns[num])
        {
            if (Context.ActiveGun != null)
            {
                Context.ActiveGun.gameObject.SetActive(false);
            }
            Context.ActiveGun = Context.Guns[num];
            Context.ActiveGun.gameObject.SetActive(true);
        }
        else if (Context.ActiveGun != null)
        {
            Context.ActiveGun.gameObject.SetActive(false);
            Context.ActiveGun = null;
        }
    }
}
