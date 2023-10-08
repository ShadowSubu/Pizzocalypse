using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PizzaHealthUI : MonoBehaviour
{
    [SerializeField] Image pizzaImage;
    [Tooltip("Arrange the Spites in reverse order, from lowest to Highest")]
    [SerializeField] Sprite[] healthSprites;
    [SerializeField] int pizzaHealthValue = 0;

    [SerializeField] VoidEventSO pizzaDamageEvent = default;
    [SerializeField] VoidEventSO onPLayerDieEvent = default;

    private void OnEnable()
    {
        pizzaDamageEvent.OnEventRaised += ReducePizzaHealth;
    }
    private void Start()
    {
        pizzaHealthValue = healthSprites.Length;
        pizzaImage.overrideSprite = healthSprites[pizzaHealthValue];
    }

    private void ReducePizzaHealth()
    {
        if (pizzaHealthValue < 0) 
        {
            onPLayerDieEvent.RaiseEvent();
            return;
        }
        pizzaHealthValue--;
        pizzaImage.overrideSprite = healthSprites[pizzaHealthValue];
    }
}
