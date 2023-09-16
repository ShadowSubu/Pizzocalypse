using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [SerializeField] AbilityType _abilityType;

    public AbilityType AbilityType { get { return _abilityType; } }

    public virtual void UseAbility()
    { }
    
}
