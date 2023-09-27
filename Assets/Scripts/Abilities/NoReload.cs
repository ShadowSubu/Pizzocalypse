using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NoReload : Abilities
{
    
    async public override void UseAbility()
    {
        Debug.Log("Executing NoReload");
        int temp = Context.ActiveGun.MagSize;

        // Set the currentMagSize to infinity
        Context.ActiveGun.CurrentMagSize = int.MaxValue;

        await Task.Delay(10000);

        // Reset the currentMagSize to its original value
        Context.ActiveGun.CurrentMagSize = temp;
    }
}
