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

    private static UIManager instance;

    private void Awake()
    {
        // singleton pattern for easy ui component access
        if (instance == null) instance = this;
    }
    
    public static void SetEnergyBar(float currentValue, float maxValue)
    {
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