using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HotnessMeter : MonoBehaviour
{
    [SerializeField] Image fillBar;

    [SerializeField] float currentHotness;
    [SerializeField] float maxHotness;
    [SerializeField] float coolRate;

    bool isPaused;
    private void Awake()
    {
        currentHotness = maxHotness;
    }
    private void OnEnable()
    {
        ResumeHotnessBar();
    }

    private void OnDisable()
    {
        StopHotnessBar();
    }

    private async void ResumeHotnessBar()
    {
        isPaused = false;
        while (currentHotness >= 0 && !isPaused)
        {
            currentHotness -= coolRate;
            fillBar.fillAmount = currentHotness/maxHotness;
            await Task.Delay(10);
        }
    }

    private void StopHotnessBar()
    {
        isPaused = true;
    }
}
