using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill 
{
    public string SkillName;
    public int SkillLevel;
    public float LearningCooldown;
    public float CurrentLevelDifficulty;
    public bool PlayerIsThinkingAboutSkill;
    public int TimeGiven;
    public float AttentionTimeLeft;
    public float[] LevelDifficulties;
    
    public float LearningProgress;
    public Skill(string skillName, float learningCooldown, float[] levelDifficulties)
    {
        SkillName = skillName;
        LearningCooldown = learningCooldown;
        LevelDifficulties = levelDifficulties;

        SkillLevel = 0;
        CurrentLevelDifficulty = levelDifficulties[SkillLevel];
        PlayerIsThinkingAboutSkill = false;
        AttentionTimeLeft = 0;
        TimeGiven = 0;
        LearningProgress = 0;
    }

    public TextMeshProUGUI CurrentLevelText;

    public GameObject AttentionButton;
    public GameObject ThinkingButton;

    public TextMeshProUGUI TimeGivenText;
    public TextMeshProUGUI AttentionTimeLeftText;

    public GameObject AddTimeButton;
    public GameObject RemoveTimeButton;

    public Image Mask;
}
