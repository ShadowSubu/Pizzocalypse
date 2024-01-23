using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Vent : Ability, IInteractable
{
    [SerializeField] Transform _startLocation;
    [SerializeField] Transform _destinationLocation;
    [SerializeField] Vector3 _playerSizeOnVent;
    [SerializeField] float _playerScalingDuration;
    [SerializeField] GameObject playerGameObject;
    Transform player;
    Vector3 _playerOriginalScale;

    // NEW
    [SerializeField] VentTriggers location1;
    [SerializeField] VentTriggers location2;
    [Range(0.1f,1f)]
    [SerializeField] float playerShrinkValue;
    [SerializeField] float shrinkDuration;
    [SerializeField] float _ventingDuration;
    [SerializeField] float _ventingCooldown;
    bool readyToUse;
    [SerializeField] VoidEventSO abilityUseEvent;

    public void Start()
    {
        readyToUse = true;
        location1.Initialize(this);

    }

    async public override void UseAbility(PlayerStateMachine player)
    {
        /*
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
        EnableColliders(); */

        if (!readyToUse)
        {
            return;
        }

        if (location1.isInTrigger)
        {
            //Debug.Log("Ready to vent from loc1");
            await Teleport(player, location2.transform.position);
            // Set Cooldown
            SetCooldown();
            location1.isInTrigger = false;
        }
        else if (location2.isInTrigger)
        {
            //Debug.Log("Ready to vent from loc2");
            await Teleport(player, location1.transform.position);
            // Set Cooldown
            SetCooldown();
            location2.isInTrigger = false;
        }

    }

    async Task Teleport(PlayerStateMachine player, Vector3 position)
    {
        // Start Venting
        player.GetComponent<CharacterController>().enabled = false;
        await player.transform.DOScale(playerShrinkValue, shrinkDuration).AsyncWaitForCompletion();
        await player.transform.DOMove(position, _ventingDuration).AsyncWaitForCompletion();

        // Scaling the Player Up
        await player.transform.DOScale(1, shrinkDuration).AsyncWaitForCompletion();
        player.GetComponent<CharacterController>().enabled = true;
    }

    private async void SetCooldown()
    {
        readyToUse = false;
        float cooldownTimer = 0;
        while (cooldownTimer < _ventingCooldown)
        {
            await Task.Delay(1000);
            cooldownTimer += 1f;
        }
        readyToUse = true;
    }

    public void Interact()
    {
        
    }
    

    /*public void SetPlayer(Transform player)
    {
        this.player = player;
        _playerOriginalScale = player.localScale;
    }

    private void DisableColliders()
    {
        _startLocation.gameObject.GetComponent<Collider>().enabled = false;
        _destinationLocation.gameObject.GetComponent<Collider>().enabled = false;
    }

    private void EnableColliders()
    {
        _startLocation.gameObject.GetComponent<Collider>().enabled = true;
        _destinationLocation.gameObject.GetComponent<Collider>().enabled = true;
    }*/

    #region



    #endregion
}
