using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera gunSelectorCamera;
    [SerializeField] CinemachineVirtualCamera levelSelectorCamera;
    [SerializeField] bool isGunSelectorMode;
    [SerializeField] Gun[] _guns = new Gun[] { };
    [SerializeField] LevelSlice[] levelSlices = new LevelSlice[] { };
    [SerializeField] Button gunSelectionButton;
    [SerializeField] Button levelSelectionButton;

    public bool IsGunSelectorMode { get { return isGunSelectorMode; } }
    private void Start()
    {
        isGunSelectorMode = true;
        _guns[0].gameObject.SetActive(true);
        gunSelectionButton.onClick.AddListener(EnableGunSelection);
        levelSelectionButton.onClick.AddListener(EnabelLevelSelection);
        AssignThisToSlices();
    }
    private void Update()
    {
        if (isGunSelectorMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                DisableGuns();
                _guns[0].gameObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                DisableGuns();
                _guns[1].gameObject.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                DisableGuns();
                _guns[2].gameObject.SetActive(true);
            }
        }
    }

    private void DisableGuns()
    {
        foreach (Gun gun in _guns)
        {
            gun.gameObject.SetActive(false);
        }
    }

    private void EnableGunSelection()
    {
        levelSelectorCamera.gameObject.SetActive(false); 
        gunSelectorCamera.gameObject.SetActive(true);
        isGunSelectorMode = true;
    }

    private void EnabelLevelSelection()
    {
        gunSelectorCamera.gameObject.SetActive(false);
        levelSelectorCamera.gameObject.SetActive(true);
        isGunSelectorMode = false;
    }

    private void AssignThisToSlices()
    {
        foreach (LevelSlice item in levelSlices)
        {
            item.menuSelector = this;
        }
    }

    public void SelectLevel(int num, LevelSlice level)
    {
        level.Highlight(levelSlices);
    }
}
