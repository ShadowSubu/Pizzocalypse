using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button nextButton;
    [SerializeField] Button prevButton;
    private SelectionState selectionState;

    // Gun Selection
    [SerializeField] Gun[] _guns = new Gun[] { };
    [SerializeField] public Gun selectedGun;
    [SerializeField] Button gunSelectionButton;
    [SerializeField] CinemachineVirtualCamera gunSelectorCamera;

    // Level Selection
    [SerializeField] LevelSlice[] levelSlices = new LevelSlice[] { };
    [SerializeField] LevelSlice selectedLevel;
    [SerializeField] Button levelSelectionButton;
    [SerializeField] CinemachineVirtualCamera levelSelectorCamera;

    // Ability Selection
    [SerializeField] AbilityPickup[] _abilties = new AbilityPickup[] { };
    [SerializeField] public AbilityPickup selectedAbility;
    [SerializeField] Button abilitySelectionButton;
    [SerializeField] CinemachineVirtualCamera abilitySelectorCamera;

    [Space]
    [SerializeField] private PlayerLoadoutSO loadout;

    public SelectionState SelectionState { get { return selectionState; } }


    private void Awake()
    {
        
    }
    private void Start()
    {
        selectionState = SelectionState.gun;
        SelectWeapon(0);
        SelectAbility(0);
        gunSelectionButton.onClick.AddListener(EnableGunSelection);
        levelSelectionButton.onClick.AddListener(EnabelLevelSelection);
        abilitySelectionButton.onClick.AddListener(EnableAbilitySelect);
        playButton.onClick.AddListener(Play);
        nextButton.onClick.AddListener(OnNextButton);
        prevButton.onClick.AddListener(OnPrevButton);
        AssignThisToSlices();


        //playButton.interactable = false;

        if (AudioManager.Instance) AudioManager.Instance.Play("Rough_-_Tune"); //PLAY BGM FOR MENU
    }

    private void EnableAbilitySelect()
    {
        gunSelectorCamera.gameObject.SetActive(false);
        levelSelectorCamera.gameObject.SetActive(false);
        abilitySelectorCamera.gameObject.SetActive(true);
        selectionState = SelectionState.ability;
    }

    private void Update()
    {
        if (selectionState == SelectionState.gun)
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
        abilitySelectorCamera.gameObject.SetActive(false);
        gunSelectorCamera.gameObject.SetActive(true);
        selectionState = SelectionState.gun;
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
    }

    private void EnabelLevelSelection()
    {
        gunSelectorCamera.gameObject.SetActive(false);
        abilitySelectorCamera.gameObject.SetActive(false);
        levelSelectorCamera.gameObject.SetActive(true);
        selectionState = SelectionState.level;
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
    }

    private void Play()
    {
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 3"); //PLAY AUDIO
        if (loadout.AbilityType != AbilityType.None)
        {
            SceneManager.LoadScene(1);
        }
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
        if (level.isUnlocked)
        {
            playButton.interactable = true;
        }
        else
        {
            playButton.interactable = false;
        }
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
    }

   public void SelectWeapon(int gunNumber)
    {
        DisableGuns();
        _guns[gunNumber].gameObject.SetActive(true);
        selectedGun = _guns[gunNumber];
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
        loadout.GunType = selectedGun.GunType;
    }

    public void SelectAbility(int num)
    {
        DisableAbilities();
        _abilties[num].gameObject.SetActive(true);
        selectedAbility = _abilties[num];
        loadout.AbilityType = selectedAbility.Type;
    }

    private void DisableAbilities()
    {
        for (int i = 0; i < _abilties.Count(); i++)
        {
            _abilties[i].gameObject.SetActive(false);
        }
    }

    public void OnNextButton()
    {
        if(selectionState == SelectionState.gun)
        {
            var index = Array.IndexOf(_guns, selectedGun);
            index++;
            if (index < _guns.Count())
            {
                SelectWeapon(index);
            }
            else
            {
                index = 0;
                SelectWeapon(index);
            }
        }
        else if (selectionState == SelectionState.ability)
        {
            var index = Array.IndexOf(_abilties, selectedAbility);
            index++;
            if (index < _abilties.Count())
            {
                SelectAbility(index);
            }
            else
            {
                index = 0;
                SelectAbility(index);
            }
        }
    }

    public void OnPrevButton()
    {
        if (selectionState == SelectionState.gun)
        {
            var index = Array.IndexOf(_guns, selectedGun);
            index--;
            if (index < 0)
            {
                index = _guns.Count() - 1;
                SelectWeapon(index);
            }
            else
            {
                SelectWeapon(index);
            }
        }
        else if (selectionState == SelectionState.ability)
        {
            var index = Array.IndexOf(_abilties, selectedAbility);
            index--;
            if (index < 0)
            {
                index = _abilties.Count() - 1;
                SelectAbility(index);
            }
            else
            {
                SelectAbility(index);
            }
        }
    }

    public void LoadLevel()
    {
        if (selectedLevel != null)
        {
            //LOAD LEVEL HERE
        }
    }

    private void OnDestroy()
    {
        gunSelectionButton.onClick.RemoveAllListeners();
        levelSelectionButton.onClick.RemoveAllListeners();
        playButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        prevButton.onClick.RemoveAllListeners();
    }
}

public enum SelectionState
{
    level,
    gun,
    ability
}
