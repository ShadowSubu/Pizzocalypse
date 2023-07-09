using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlice : MonoBehaviour
{
    int sliceNumber;
    public MenuSelector menuSelector;

    private void OnMouseDown()
    {
        if (!menuSelector.IsGunSelectorMode)
        {
            Debug.Log(gameObject.name);
            if (menuSelector != null)
            {
                menuSelector.SelectLevel(sliceNumber, this);
            }
        }
    }

    public void Highlight(LevelSlice[] levels)
    {
        foreach (LevelSlice item in levels)
        {
            item.transform.localScale = Vector3.one;
        }
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }
}
