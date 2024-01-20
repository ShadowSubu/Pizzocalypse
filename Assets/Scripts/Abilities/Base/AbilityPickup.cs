using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class AbilityPickup : MonoBehaviour
{
    [SerializeField] AbilityType type;
    public AbilityType Type => type;

    public float rotationDuration = 2f;
    public float upDownAmplitude = 0.5f;

    void Start()
    {
        RotateUpDown();
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
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerStateMachine>().SetAbility(type))
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Has Ability");
            }
        }
    }
}
