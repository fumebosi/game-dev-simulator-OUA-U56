using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/SkillList", fileName = "SkillHolder")]
public class SkillHolder : ScriptableObject
{
   public List<SkillPreset> SkillList;

}
