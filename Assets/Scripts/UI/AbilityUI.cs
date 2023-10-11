using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class AbilityUI : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    bool hasAbility;

    [SerializeField] private VoidEventSO abilityEquipEvent = default;
    [SerializeField] private VoidEventSO abilityUseEvent = default;

    private void OnEnable()
    {
        abilityEquipEvent.OnEventRaised += AbilityEquiped;
        abilityUseEvent.OnEventRaised += AbilityUsed;
    }

    private void AbilityUsed()
    {
        abilityImage.DOFade(0.2f, 1f);
    }

    private void AbilityEquiped()
    {
        abilityImage.DOFade(1f, 1f);
    }

    private void OnDisable()
    {
        abilityEquipEvent.OnEventRaised -= AbilityEquiped;
        abilityUseEvent.OnEventRaised -= AbilityEquiped;
    }
}
