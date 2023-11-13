using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Vent : Abilities
{
    [SerializeField] Transform _startLocation;
    [SerializeField] Transform _destinationLocation;
    [SerializeField] Vector3 _playerSizeOnVent;
    [SerializeField] float _ventingDuration;
    [SerializeField] public int _ventingCooldown;
    [SerializeField] float _playerScalingDuration;
    [SerializeField] GameObject playerGameObject;
    Transform player;
    Vector3 _playerOriginalScale;
    
    async public override void UseAbility()
    {
        //Start venting
        playerGameObject.GetComponent<CharacterController>().enabled = false;
        await player.DOScale(_playerSizeOnVent, _playerScalingDuration).AsyncWaitForCompletion();
        await player.DOMove(_destinationLocation.position, _ventingDuration).AsyncWaitForCompletion();
        
        //Venting Completed
        await player.DOScale(_playerOriginalScale, _playerScalingDuration).AsyncWaitForCompletion();
        playerGameObject.GetComponent<CharacterController>().enabled = true;
       
        //Set Vent Cooldown
        DisableColliders();
        await Task.Delay(_ventingCooldown * 1000);
        EnableColliders();
        
    }
    public void SetPlayer(Transform player)
    {
        this.player = player;
        _playerOriginalScale = player.localScale;
    }
    
    public void DisableColliders()
    {
        _startLocation.gameObject.GetComponent<Collider>().enabled = false;
        _destinationLocation.gameObject.GetComponent<Collider>().enabled = false;
    }

    public void EnableColliders()
    {
        _startLocation.gameObject.GetComponent<Collider>().enabled = true;
        _destinationLocation.gameObject.GetComponent<Collider>().enabled = true;
    }
}
