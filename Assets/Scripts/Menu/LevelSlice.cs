using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSlice : MonoBehaviour
{
    int sliceNumber;
    public MenuSelector menuSelector;
    [SerializeField] public string levelName;
    public bool isUnlocked = false;

    private void OnMouseDown()
    {
        if (menuSelector.SelectionState == SelectionState.level)
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
