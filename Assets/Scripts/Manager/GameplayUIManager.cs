using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEditor;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameplayUIManager : MonoBehaviour
{
    [Header("Dialogue Panel")]
    [SerializeField] RectTransform dialoguePanel;
    [SerializeField] Button nextDialogueButton;
    [SerializeField] DialogueStatus dialogueStatus;

    [Header("Start Game Panel")]
    [SerializeField] RectTransform startGamePanel;

    [Header("Bar Panel")]
    [SerializeField] RectTransform barPanel;

    [Header("Score Panel")]
    [SerializeField] RectTransform scorePanel;
    [SerializeField] Button replayButton;
    [SerializeField] Button menuButton;
    [SerializeField] Button nextLevelButton;

    private void Awake()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        AssignButtons();
        ResetPanels();
    }

    private void Start()
    {
    }

    void AssignButtons()
    {
        nextDialogueButton.onClick.AddListener(PlayNextDialogue);
        replayButton.onClick.AddListener(ReplayLevel);
        menuButton.onClick.AddListener(GoToMenu);
        nextLevelButton.onClick.AddListener(PlayNextLevel);
    }

    void ResetPanels()
    {
        ResetDialoguePanel();
        ResetStartPanel();
        ResetBarPanel();
        ResetScorePanel();
    }

    void OnGameStateChanged(GameState state)
    {
        Debug.Log("Chnaged state");
        switch (state)
        {
            case GameState.EnterGame:
                break;
            case GameState.ShowPlayerMonologue:
                OpenDialoguePanel(DialogueStatus.opening);
                break;
            case GameState.StartGame:
                OpenStartGamePanel();
                break;
            case GameState.Deliver:
                AnimateBarPanelReverse();
                OpenDialoguePanel(DialogueStatus.closing);
                break;
            case GameState.ShowScore:
                OpenScorePanel();
                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }
    }

    #region Start Game Panel

    void ResetStartPanel()
    {
        startGamePanel.GetComponent<CanvasGroup>().alpha = 0f;
        startGamePanel.gameObject.SetActive(false);
    }

    async void OpenStartGamePanel()
    {
        startGamePanel.gameObject.SetActive(true);
        await startGamePanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).AsyncWaitForCompletion();
        await Task.Delay(1000);
        CloseStartGamePanel();
    }

    async void CloseStartGamePanel()
    {
        await startGamePanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).AsyncWaitForCompletion();
        startGamePanel.gameObject.SetActive(false);
        AnimateBarPanel();
    }

    #endregion

    #region Dialogue Panel

    void ResetDialoguePanel()
    {
        nextDialogueButton.interactable = false;
        dialoguePanel.GetComponent<CanvasGroup>().alpha = 0f;
        dialoguePanel.gameObject.SetActive(false);
    }

    async void OpenDialoguePanel(DialogueStatus status)
    {
        dialogueStatus = status;
        dialoguePanel.gameObject.SetActive(true);
        await dialoguePanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).AsyncWaitForCompletion();
        nextDialogueButton.interactable = true;
    }

    void PlayNextDialogue()
    {
        CloseDialoguePanel();
    }

    async void CloseDialoguePanel()
    {
        nextDialogueButton.interactable = false;
        await dialoguePanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).AsyncWaitForCompletion();
        ResetDialoguePanel();

        switch (dialogueStatus)
        {
            case DialogueStatus.opening:
                if (GameManager.instance) GameManager.instance.UpdateGameState(GameState.StartGame);
                break;
            case DialogueStatus.midGame:
                break;
            case DialogueStatus.closing:
                if (GameManager.instance) GameManager.instance.UpdateGameState(GameState.ShowScore);
                break;
            default:
                break;
        }
    }
    #endregion

    #region Bar Panel

    void ResetBarPanel()
    {
        barPanel.GetComponent<CanvasGroup>().alpha = 0f;
        barPanel.gameObject.SetActive(false);
    }

    async void AnimateBarPanel()
    {
        barPanel.gameObject.SetActive(true);
        await barPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).AsyncWaitForCompletion();
    }

    async void AnimateBarPanelReverse()
    {
        await barPanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).AsyncWaitForCompletion();
        barPanel.gameObject.SetActive(false);
    }
    #endregion

    #region Score Panel

    void ResetScorePanel()
    {
        scorePanel.GetComponent<CanvasGroup>().alpha = 0f;
        scorePanel.gameObject.SetActive(false);
    }

    async void OpenScorePanel()
    {
        scorePanel.gameObject.SetActive(true);
        await scorePanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).AsyncWaitForCompletion();
        AnimateScoreElements();
    }

    async void AnimateScoreElements()
    {
        
    }

    async Task CloseScorePanel()
    {
        await scorePanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).AsyncWaitForCompletion();
        scorePanel.gameObject.SetActive(false);
    }
    #endregion

    async void ReplayLevel()
    {
        await CloseScorePanel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    async void GoToMenu()
    {
        await CloseScorePanel();
    }

    async void PlayNextLevel()
    {
        await CloseScorePanel();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}

public enum DialogueStatus{
    opening,
    midGame,
    closing
}
