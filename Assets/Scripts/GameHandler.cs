using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance = null;
    
    [HideInInspector] public int TimeAvailable;
    [HideInInspector] public float Money;

    public TextMeshProUGUI TimeAvailableText;
    public TextMeshProUGUI MoneyText;

 
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameHandler found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        TimeAvailable = 6;
        Money = 0;
    }

    public void EarnProfitFromGame(float profitFromGame)
    {
        Money += profitFromGame;
        UpdateMoney();
    }

    public void UpdateMoney()
    {
        MoneyText.text = Math.Round(Money, 0).ToString() + "$";
    }

    public void SpendMoney(float amount)
    {
        Money -= amount;
        UpdateMoney();
    }

    public void UpdateTimeButtons(List<Skill> allSkills)
    {
        if (TimeAvailable >= 1)
        {
            foreach (Skill skill in allSkills)
            {
                skill.AddTimeButton.SetActive(true);
            }
        }
       
        foreach (Skill skill in allSkills)
        {
            if (skill.TimeGiven > 0)
                skill.RemoveTimeButton.SetActive(true);
            else
                skill.AddTimeButton.SetActive(false);
        }
    }
}
