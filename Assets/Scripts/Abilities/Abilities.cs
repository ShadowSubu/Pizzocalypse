using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities : MonoBehaviour
{
    [SerializeField] AbilityType _abilityType;
    [SerializeField] PlayerStateMachine _context;

    public AbilityType AbilityType { get { return _abilityType; } }
    public PlayerStateMachine Context { get { return _context; } }

    public abstract void UseAbility();
}
