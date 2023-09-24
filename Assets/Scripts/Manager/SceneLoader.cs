using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [SerializeField] CanvasGroup loaderCanvas;
    [SerializeField] Image progressBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        loaderCanvas.alpha = 1.0f;
        loaderCanvas.gameObject.SetActive(true);
        progressBar.fillAmount = 0f;
        FadeOut();
    }

    public async void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        // IMPLEMENT LOADING SCENE
    }

    async Task FadeIn()
    {
        loaderCanvas.gameObject.SetActive(true);
        await loaderCanvas.DOFade(1f, 1f).AsyncWaitForCompletion();
    }

    async Task FadeOut()
    {
        await loaderCanvas.DOFade(0f, 1f).AsyncWaitForCompletion();
        loaderCanvas.gameObject.SetActive(false);
    }
}
