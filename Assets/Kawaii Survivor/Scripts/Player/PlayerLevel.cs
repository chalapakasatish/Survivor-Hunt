using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Settings")]
    private int requiredXP;
    private int currentXP;
    private int level;

    [Header("Visuals")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TextMeshProUGUI levelText;
    private void Awake()
    {
        Candy.onCollected += CandyCollectedCallback;
    }
    private void OnDestroy()
    {
        Candy.onCollected -= CandyCollectedCallback;
    }
    void Start()
    {
        UpdateRequiredXp();
        UpdateVisuals();
    }
    private void UpdateRequiredXp()
    {
        requiredXP = (level + 1) * 5;
    }
    private void UpdateVisuals()
    {
        xpBar.value = (float)currentXP/requiredXP;
        levelText.text = "Level " + (level + 1);
    }
    private void CandyCollectedCallback(Candy candy)
    {
        currentXP += 1;

        if(currentXP >= requiredXP)
        {
            LevelUp();
        }
        UpdateVisuals();
    }

    private void LevelUp()
    {
        level++;
        currentXP = 0;
        UpdateRequiredXp();
    }
}
