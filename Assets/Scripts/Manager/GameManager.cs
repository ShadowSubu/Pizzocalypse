using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameState state;
    public static event Action<GameState> OnGameStateChanged;
    [SerializeField] PlayerStateMachine playerCharacter;
    private void Awake()
    {
        instance = this;
        playerCharacter = FindAnyObjectByType<PlayerStateMachine>();
        playerCharacter.transform.localScale = Vector3.zero;
    }
    private void Start()
    {
        UpdateGameState(GameState.EnterGame);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.EnterGame:
                HandleEnterGame();
                break;
            case GameState.ShowPlayerMonologue:
                HandleShowPlayerMonologue();
                break;
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.Deliver:
                HandleDeliver();
                break;
            case GameState.ShowScore:
                HandleShowScore();
                break;
            case GameState.EndGame:
                HandleEndGame();
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }

    async void HandleEnterGame()
    {
        if (AudioManager.Instance)
        {
            
            AudioManager.Instance.Play("Pizzocalypse-Spawn"); //PLAY AUDIO
        }

        // SPAWN PLAYER
        if (playerCharacter)
        {
            await playerCharacter.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InBounce).AsyncWaitForCompletion();
            UpdateGameState(GameState.ShowPlayerMonologue);
        }
        else
        {
            Debug.LogWarning("Missing Player Prefab!!");
        }
    }

    void HandleShowPlayerMonologue()
    {

    }

    void HandleStartGame()
    {
        if (AudioManager.Instance) AudioManager.Instance.Play("Rock_The_Party"); //PLAY BGM
    }

    void HandleDeliver()
    {

    }

    void HandleShowScore()
    {

    }

    void HandleEndGame()
    {

    }
}

public enum GameState
{
    EnterGame,
    ShowPlayerMonologue,
    StartGame,
    Deliver,
    ShowScore,
    EndGame
}
