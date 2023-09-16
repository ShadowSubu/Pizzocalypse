using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class Vent : Abilities
{
    [SerializeField] Transform _startLocation;
    [SerializeField] Transform _destinationLocation;
    Transform player;
    [SerializeField] GameObject playerGameObject;
    async public override void UseAbility()
    {
        Debug.Log("UseAbility being used");
        playerGameObject.GetComponent<CharacterController>().enabled = false;
        await player.DOMove(_destinationLocation.position, .75f).AsyncWaitForCompletion();
        
        Debug.Log("Venting Completed");
        playerGameObject.GetComponent<CharacterController>().enabled = true;
       
        await Task.Delay(10000);
        //_startLocation.gameObject.GetComponent<MeshCollider>().enabled = false;
        //_destinationLocation.gameObject.GetComponent<MeshCollider>().enabled = false;
    }
    public void SetPlayer(Transform player)
    {
        this.player = player;
        //Debug.Log($"Player transform {this.player.transform.position}");
    }
}
