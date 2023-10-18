using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    public SelectionState SelectionState { get { return selectionState; } }

    private void Awake()
    {
        
    }
    private void Start()
    {
        selectionState = SelectionState.gun;
        SelectWeapon(0);
        gunSelectionButton.onClick.AddListener(EnableGunSelection);
        levelSelectionButton.onClick.AddListener(EnabelLevelSelection);
        playButton.onClick.AddListener(Play);
        nextButton.onClick.AddListener(OnNextButton);
        prevButton.onClick.AddListener(OnPrevButton);
        AssignThisToSlices();

        playButton.interactable = false;

        if (AudioManager.Instance) AudioManager.Instance.Play("Rough_-_Tune"); //PLAY BGM FOR MENU
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
        gunSelectorCamera.gameObject.SetActive(true);
        selectionState = SelectionState.gun;
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
    }

    private void EnabelLevelSelection()
    {
        gunSelectorCamera.gameObject.SetActive(false);
        levelSelectorCamera.gameObject.SetActive(true);
        selectionState = SelectionState.level;
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 1"); //PLAY AUDIO
    }

    private void Play()
    {
        if (AudioManager.Instance) AudioManager.Instance.Play("Pizzocalypse-Button Click 3"); //PLAY AUDIO
        if (selectedLevel)
        {
            if (SceneLoader.Instance && selectedLevel.isUnlocked)
            {
                SceneLoader.Instance.LoadScene(selectedLevel.levelName);
            }
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
    }

    public void OnNextButton()
    {

    }

    public void OnPrevButton()
    {

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
