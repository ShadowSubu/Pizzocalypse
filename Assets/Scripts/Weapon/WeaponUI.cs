using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bulletCountText;

    [SerializeField] IntEventSO bulletCountEvent = default;

    private void OnEnable()
    {
        bulletCountEvent.OnEventRaised += UpdateWeaponUI;
    }

    private void UpdateWeaponUI(int ammoInGun, int maxAmmo, int totalAmmoStock)
    {
        bulletCountText.text = "Bullets: " +  ammoInGun.ToString() + "/" + maxAmmo.ToString() + " || " + totalAmmoStock.ToString();
    }
}
