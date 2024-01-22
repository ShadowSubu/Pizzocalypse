using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    int _ammoAmountCache;
    int _ammoReduceAmountCache;
    int _magSizeCache;
    int _currentMagSizeCache;

    private void Start()
    {
        _ammoAmountCache = _ammoAmount;
        _ammoReduceAmountCache = _ammoReduceAmount;
        _magSizeCache = _magSize;
        _currentMagSizeCache = _currentMagSize;
    }

    public async void InitiateNoReload(float duration)
    {
        // TODO: CACHE ALL THE VALUES RELATED TO AMMO
        //       DONT REDUCE THE AMMO WHEN SHOOTING
        //       START A DELAY FUNCTION FOOR THE ABILITY DURATION
        //       AFTER THE DELAY RESET ALL THE VALUES RELATED TO AMMO

        int ammoAmount = _ammoAmount;
        int ammoReduceAmount = _ammoReduceAmount;
        int magSize = _magSize;
        int currentMagSize = _currentMagSize;

        _ammoAmount = 1;
        _ammoReduceAmount = 0;
        _magSize = 1;
        _currentMagSize = 1;

        await Task.Delay((int)(duration * 1000));

        ResetGun();

        /*_ammoAmount = ammoAmount;
        _ammoReduceAmount = ammoReduceAmount;
        _magSize = magSize;
        _currentMagSize = currentMagSize;*/
    }

    public void ResetGun()
    {
        _ammoAmount = _ammoAmountCache;
        _ammoReduceAmount = _ammoReduceAmountCache;
        _magSize = _magSizeCache;
        _currentMagSize = _currentMagSizeCache;
    }

}
