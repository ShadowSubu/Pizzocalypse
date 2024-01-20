using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Image hotnessBar;
    public TextMeshProUGUI scoreText;
    public List<GameObject> stars = new List<GameObject>();

    public int minScore = 200;
    public int maxScore = 3000;
    private float minStarRating = 1;
    private float maxStarRating = 3;

    private void OnEnable()
    {
        FillScore();
    }

    private void FillScore()
    {
        int currentScore = Mathf.RoundToInt(Mathf.Lerp(minScore, maxScore, 1 - hotnessBar.fillAmount));
        int starRating = (int)Mathf.Clamp(Mathf.RoundToInt(Mathf.Lerp(minStarRating, maxStarRating, 1 - hotnessBar.fillAmount)), minStarRating, maxStarRating);

        foreach (var item in stars)
        {
            item.SetActive(false);
        }

        for (int i = 0; i < starRating; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
