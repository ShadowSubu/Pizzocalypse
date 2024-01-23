using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.ProBuilder.MeshOperations;

[RequireComponent(typeof(Collider))]
public class AbilityPickup : MonoBehaviour
{
    [SerializeField] AbilityType type;
    //PlayerStateMachine playerStateMachine;
    public AbilityType Type => type;

    public float rotationDuration = 2f;
    public float upDownAmplitude = 0.5f;

    void Start()
    {
        RotateUpDown();
        //playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void RotateUpDown()
    {
        // Rotate 360 degrees around the Y-axis indefinitely
        transform.DORotate(new Vector3(0f, 360f, 0f), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);

        // Move up and down using DOJump
        transform.DOJump(transform.position, upDownAmplitude, 1, rotationDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerStateMachine player) && player.SetAbility(type))
        {
            if (player.AbilityCount.ContainsKey(type))
            {
                player.AbilityCount[type]++;
                //Debug.Log("Incremented to dict:" + player.AbilityCount[type]);
            }
            else
            {
                player.AbilityCount[type] = 1;
                //Debug.Log("Added Ability to dict:" + player.AbilityCount[type]);
            }
            Destroy(gameObject);
            /*if (player.SetAbility(type))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Has Ability");
            }*/
        }
    }
}
