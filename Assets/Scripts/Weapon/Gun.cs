using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GunType _gunType;
    [SerializeField] float _shootingDuration;
    [SerializeField] Transform _shootingPoint;
    [SerializeField] int _ammoAmount;
    [SerializeField] int _ammoReduceAmount;
    [SerializeField] int _magSize;
    [SerializeField] int _currentMagSize;

    //GETTERS AND SETTERS
    public GunType GunType { get { return _gunType; } }
    public float ShootingDuration { get { return _shootingDuration * 1000; } } // converted to milli seconds
    public Transform ShootingPoint { get { return _shootingPoint; } }
    public int AmmoAmount {  get { return _ammoAmount; } set { _ammoAmount = value; } }
    public int AmmoReduceAmount { get { return _ammoReduceAmount; } }
    public int MagSize { get { return _magSize; }  }
    public int CurrentMagSize {  get { return _currentMagSize; } set { _currentMagSize = value; } }

}
