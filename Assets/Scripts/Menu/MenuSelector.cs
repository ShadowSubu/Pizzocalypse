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
    [SerializeField] Gun selectedGun;
    [SerializeField] LevelSlice selectedLevel;
    [SerializeField] Button gunSelectionButton;
    [SerializeField] Button levelSelectionButton;

    public bool IsGunSelectorMode { get { return isGunSelectorMode; } }

    private void Awake()
    {
        if (AudioManager.Instance) AudioManager.Instance.Play(""); //PLAY BGM FOR MENU
    }
    private void Start()
    {
        isGunSelectorMode = true;
        SelectWeapon(0);
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
                SelectWeapon(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SelectWeapon(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SelectWeapon(2);
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
        if (AudioManager.Instance) AudioManager.Instance.Play(""); //PLAY AUDIO
    }

    private void EnabelLevelSelection()
    {
        gunSelectorCamera.gameObject.SetActive(false);
        levelSelectorCamera.gameObject.SetActive(true);
        isGunSelectorMode = false;
        if (AudioManager.Instance) AudioManager.Instance.Play(""); //PLAY AUDIO
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
        selectedLevel = level;
        if (AudioManager.Instance) AudioManager.Instance.Play(""); //PLAY AUDIO
    }

   public void SelectWeapon(int gunNumber)
    {
        DisableGuns();
        _guns[gunNumber].gameObject.SetActive(true);
        selectedGun = _guns[gunNumber];
        if (AudioManager.Instance) AudioManager.Instance.Play(""); //PLAY AUDIO
    }

    public void LoadLevel()
    {
        if (selectedLevel != null)
        {
            //LOAD LEVEL HERE
        }
    }
}
