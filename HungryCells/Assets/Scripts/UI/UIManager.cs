using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyBarText;
    [SerializeField] private Image energyBarValueTransform;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private static UIManager instance;
    private int score = 0;
    
    private void Awake()
    {
        // singleton pattern for easy ui component access
        if (instance == null) instance = this;
    }

    public static void IncreaseScore(int amount)
    {
        instance.score += amount;
        instance.scoreText.text = "Score:" +  instance.score.ToString();
    }
    
    public void OnRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void SetEnergyBar(float currentValue, float maxValue)
    {
        currentValue = Mathf.Max(0, currentValue);
        instance.energyBarText.text =  $"{currentValue:0.00}/{maxValue:0}";
        instance.energyBarValueTransform.fillAmount = Math.Clamp(currentValue / maxValue, 0, 1);
    }

    public static void SetDeathScreenVisibility(bool isVisible)
    {
        instance.deathScreen.SetActive(isVisible);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}