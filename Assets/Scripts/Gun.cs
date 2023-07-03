using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunType _gunType;
    [SerializeField] float _shootingDuration;

    //GETTERS AND SETTERS
    public GunType GunType { get { return _gunType; } }
    public float ShootingDuration { get { return _shootingDuration * 1000; } } // converted to milli seconds

}
