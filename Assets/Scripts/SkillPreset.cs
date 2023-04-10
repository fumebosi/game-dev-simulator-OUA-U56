using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SkillPreset", fileName = "SkillPreset")]
public class SkillPreset : ScriptableObject
{
    public Sprite SkillSprite;
    public string SkillName;
    public float SkillCooldown;
    public float[] LevelDifficulties; 
    public int[] Milestones;
}
