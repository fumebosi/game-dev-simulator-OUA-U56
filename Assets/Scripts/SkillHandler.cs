using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SkillHandler : MonoBehaviour
{
    public SkillHolder SH;

    //public static SkillHandler Instance = null;

    [SerializeField] GameObject[] skillPrefabs;
    void InitializeSkills()
    {
        foreach (SkillPreset SP in SH.SkillList)
        {
            Skill skill = new Skill(SP.SkillName, SP.SkillCooldown, SP.LevelDifficulties);
            skillsToBeInitialized.Add(skill);
        }
    }
    private void Awake()
    {
        InitializeSkills();
        InitializeSkillVisuals();
    }

    private void FixedUpdate()
    {
        if (skillsThatWeGaveAttention.Count > 0)
        {
            for (int i = 0; i < skillsThatWeGaveAttention.Count; i++)
            {
                ProgressViaTimeAndAttention(skillsThatWeGaveAttention[i]);
            }
        }

        if (skillWeThinkAbout != null)
        {
            ProgressViaThinking(skillWeThinkAbout);
        }
    }
    List<Skill> allSkills = new List<Skill>();
    List<Skill> skillsToBeInitialized = new List<Skill>();
    void InitializeSkillVisuals()
    {
        foreach (GameObject skillPrefab in skillPrefabs)
        {
            foreach (Skill skill in skillsToBeInitialized)
            {
                if (skillPrefab.name == skill.SkillName)
                {
                    //skillPrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = FindNeededSprite(skill.SkillName);

                    skill.Mask = skillPrefab.transform.GetChild(0).GetChild(0).GetComponent<Image>();

                    skill.AttentionButton = skillPrefab.transform.GetChild(2).gameObject;
                    skillPrefab.transform.GetChild(2).gameObject.name = "attention_" + skill.SkillName;

                    skillPrefab.transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = FindNeededSprite(skill.SkillName);
                    //skillPrefab.transform.GetChild(2).GetChild(0).GetChild(0)
                    
                    skill.ThinkingButton = skillPrefab.transform.GetChild(3).gameObject;
                    skillPrefab.transform.GetChild(3).gameObject.name = "think_" + skill.SkillName;

                    skill.CurrentLevelText = skillPrefab.transform.GetChild(5).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                    skill.CurrentLevelText.text = skill.SkillLevel.ToString();

                    skill.TimeGivenText = skillPrefab.transform.GetChild(6).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                    skill.AddTimeButton = skillPrefab.transform.GetChild(7).gameObject;
                    skill.AddTimeButton.SetActive(true);
                    skillPrefab.transform.GetChild(7).gameObject.name = "addtime_" + skill.SkillName;
                    
                    skill.RemoveTimeButton = skillPrefab.transform.GetChild(8).gameObject;
                    skill.RemoveTimeButton.SetActive(false);
                    skillPrefab.transform.GetChild(8).gameObject.name = "removetime_" + skill.SkillName;

                    skill.AttentionTimeLeftText = skillPrefab.transform.GetChild(9).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

                    allSkills.Add(skill);
                    skillsToBeInitialized.Remove(skill);
                    break;
                }
            }
        }
    }
 

     
    Sprite FindNeededSprite(string nameOfSkill)
    {
        foreach (SkillPreset skill in SH.SkillList)
        {
            if (nameOfSkill == skill.SkillName)
            {
                return skill.SkillSprite;
            }
        }
        return null;
    }

    List<Skill> skillsThatWeGaveAttention = new List<Skill>();
    void ProgressViaTimeAndAttention(Skill skill)
    {
        skill.LearningProgress += Time.deltaTime * (float)(skill.TimeGiven * 0.95f ) * (float)(skill.AttentionTimeLeft * 0.3f)  / skill.CurrentLevelDifficulty ;
        float progress = skill.LearningProgress / skill.LearningCooldown;
        skill.Mask.fillAmount = progress;

        skill.AttentionTimeLeft -= Time.deltaTime;

        if (skill.AttentionTimeLeft <= 0)
        {
            skill.AttentionTimeLeft = 0;
        }
        skill.AttentionTimeLeftText.text = "Current Attention : " + Math.Round((double)skill.AttentionTimeLeft).ToString();
        if (progress >= 1)
        {
            skill.SkillLevel++;
            int unityLevel = 0;
            int asepriteLevel = 0;
            int blenderLevel = 0;
            int cSharpLevel = 0;

            foreach (Skill item in allSkills)
            {
                if (item.SkillName == "unity")
                {
                    unityLevel = item.SkillLevel;
                }
                else if( item.SkillName == "aseprite")
                {
                    asepriteLevel = item.SkillLevel;
                }
                else if (item.SkillName == "blender")
                {
                    blenderLevel = item.SkillLevel;
                }
                else if (item.SkillName == "csharp")
                {
                    cSharpLevel = item.SkillLevel;
                }
            }
            if (cSharpLevel >= 5 && unityLevel >= 10 && asepriteLevel >= 10)
            {
                GameProfitHandler.Instance.canDevelopTerraria = true;
                foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                {
                    if (item.name == "terraria_purchase")
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
            if (cSharpLevel >= 15 && unityLevel >= 20 && asepriteLevel >= 20)
            {
                GameProfitHandler.Instance.canDevelopMinecraft = true;
                foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                {
                    if (item.name == "minecraft_purchase")
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
            if (cSharpLevel >= 30 && unityLevel >= 30 && blenderLevel >= 30)
            {
                if (GameHandler.Instance.Money >= 4000000)
                {
                    GameProfitHandler.Instance.hasGtaSkills = true;
                    foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                    {
                        if (item.name == "gta_purchase")
                        {
                            item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                        }
                    }
                }
            }
            skill.CurrentLevelDifficulty = skill.LevelDifficulties[skill.SkillLevel];
            skill.CurrentLevelText.text = skill.SkillLevel.ToString();
            skill.LearningProgress = 0;
            skill.Mask.fillAmount = skill.LearningProgress / skill.LearningCooldown;
        }

        if (skill.AttentionTimeLeft <= 0)
        {
            skillsThatWeGaveAttention.Remove(skill);
        }
    }
    Skill skillWeThinkAbout;
    void ProgressViaThinking(Skill skill)
    {
        skill.LearningProgress += Time.deltaTime + (skill.AttentionTimeLeft * 0.001f) / skill.CurrentLevelDifficulty;
        float progress = skill.LearningProgress / skill.LearningCooldown;
        skill.Mask.fillAmount = progress;

        if (progress >= 1)
        {
            skill.SkillLevel++;
            int unityLevel = 0;
            int asepriteLevel = 0;
            int blenderLevel = 0;
            int cSharpLevel = 0;

            foreach (Skill item in allSkills)
            {
                if (item.SkillName == "unity")
                {
                    unityLevel = item.SkillLevel;
                }
                else if (item.SkillName == "aseprite")
                {
                    asepriteLevel = item.SkillLevel;
                }
                else if (item.SkillName == "blender")
                {
                    blenderLevel = item.SkillLevel;
                }
                else if (item.SkillName == "csharp")
                {
                    cSharpLevel = item.SkillLevel;
                }
            }
            if (cSharpLevel >= 5 && unityLevel >= 10 && asepriteLevel >= 10)
            {
                GameProfitHandler.Instance.canDevelopTerraria = true;
                foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                {
                    if (item.name == "terraria_purchase")
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
            if (cSharpLevel >= 15 && unityLevel >= 20 && asepriteLevel >= 20)
            {
                GameProfitHandler.Instance.canDevelopMinecraft = true;
                foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                {
                    if (item.name == "minecraft_purchase")
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
            if (cSharpLevel >= 30 && unityLevel >= 30 && blenderLevel >= 30)
            {
                GameProfitHandler.Instance.hasGtaSkills = true;
                foreach (GameObject item in GameProfitHandler.Instance.GameDevelopPrefabs)
                {
                    if (item.name == "gta_purchase" && GameProfitHandler.Instance.hasGtaMoney)
                    {
                        item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                    }
                }
            }
            skill.CurrentLevelDifficulty = skill.LevelDifficulties[skill.SkillLevel];
            skill.CurrentLevelText.text = skill.SkillLevel.ToString();
            skill.LearningProgress = 0;
            skill.Mask.fillAmount = skill.LearningProgress / skill.LearningCooldown;
        }
    }
    private void Update()
    {
        HandleClicks();
    }

    private RaycastHit hitInfo;
    char[] delimiterChar = { '_' };
    [SerializeField] LayerMask clickableLayer;
    void HandleClicks()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1000, clickableLayer))
            {
                GameObject objectHit = hitInfo.transform.gameObject;
                string[] separatedWords = objectHit.name.Split(delimiterChar);
                if (separatedWords[0] == "attention")
                {
                    foreach (Skill skill in allSkills)
                    {
                        if (skill.SkillName == separatedWords[1])
                        {
                            StartCoroutine(animateButtonScaleOne(objectHit));
                           
                            skill.AttentionTimeLeft += 5f;

                            if (skill.AttentionTimeLeft >= 17)
                            {
                                skill.AttentionTimeLeft = 17f;
                            }
                            skill.AttentionTimeLeftText.text = "Current Attention : " + Math.Round((double)skill.AttentionTimeLeft, 1).ToString();
                            bool foundItOnList = false;
                            foreach (Skill skillInAttentionList in skillsThatWeGaveAttention)
                            {
                                if (skillInAttentionList.SkillName == skill.SkillName)
                                {
                                    foundItOnList = true;
                                    break;
                                }
                            }
                            if (foundItOnList == false)
                                skillsThatWeGaveAttention.Add(skill);
                        }
                    }
                }
                else if(separatedWords[0] == "think")
                {
                    foreach(Skill skill in allSkills)
                    {
                        if (skill.SkillName == separatedWords[1])
                        {
                            foreach (Skill item in allSkills)
                            {
                                item.ThinkingButton.SetActive(true);
                            }
                            skill.ThinkingButton.SetActive(false);
                            skillWeThinkAbout = skill;
                            
                        }
                    }
                }
                else if (separatedWords[0] == "addtime")
                {
                    foreach (Skill skill in allSkills)
                    {
                        if (skill.SkillName == separatedWords[1])
                        {
                            StartCoroutine(animateButtonScaleOne(objectHit));
                            GameHandler.Instance.TimeAvailable--;
                            GameHandler.Instance.TimeAvailableText.text = "You have " + GameHandler.Instance.TimeAvailable + " hours of free time";
                            skill.RemoveTimeButton.SetActive(true);
                            if (GameHandler.Instance.TimeAvailable == 0)
                            {
                                foreach (Skill item in allSkills)
                                {
                                    item.AddTimeButton.SetActive(false);
                                }
                            }
                            skill.TimeGiven++;
                            skill.TimeGivenText.text = skill.TimeGiven.ToString() + " hours of training";
                            //GameHandler.Instance.UpdateTimeButtons(allSkills);
                        }
                    }
                    //initial olarak addtime hepsinde aktif olacak
                    //if has time >> skill.TimeGiven++
                    //bunu ekledikten sonra zamaný bir azalt. ve eðer boþ zamanýn 0 sa tüm artýrma butonlarýný deaktive et
                    //remove time butonu aktif deðilse aktif et
                    //time given textini updatele
                }
                else if (separatedWords[0] == "removetime")
                {
                    foreach (Skill skill in allSkills)
                    {
                        if (skill.SkillName == separatedWords[1])
                        {
                            StartCoroutine(animateButtonScaleOne(objectHit));
                            skill.TimeGiven--;
                            if (skill.TimeGiven == 0)
                            {
                                skill.RemoveTimeButton.SetActive(false);
                            }
                            GameHandler.Instance.TimeAvailable++;
                            GameHandler.Instance.TimeAvailableText.text = "You have " + GameHandler.Instance.TimeAvailable + " hours of free time";
                            foreach (Skill item in allSkills)
                            {
                                item.AddTimeButton.SetActive(true);
                            }
                            skill.TimeGivenText.text = skill.TimeGiven.ToString() + " hours of training";
                            
                            //GameHandler.Instance.UpdateTimeButtons(allSkills);
                        }
                    }
                    //bu buton. eðer zaten 1 zaman atanmýþsa aktif olmalý zaten. 
                    //time given textini updatele

                }
            }
        }
    }
    public static IEnumerator animateButtonScaleOne(GameObject _btn)
    {
        Vector3 startingScale = Vector3.one;      //initial scale	
        Vector3 destinationScale = startingScale * 0.9f;       //target scale

        //Scale down
        float timer1 = 0.0f;
        while (timer1 <= 1f)
        {
            timer1 += Time.deltaTime * 10f;
            _btn.transform.localScale = new Vector3(Mathf.SmoothStep(startingScale.x, destinationScale.x, timer1), Mathf.SmoothStep(startingScale.y, destinationScale.y, timer1), _btn.transform.localScale.z);

            yield return 0;
        }

        //Scale down
        float timer2 = 0.0f;
        if (_btn.transform.localScale.x >= destinationScale.x)
        {
            while (timer2 <= 1f)
            {
                timer2 += Time.deltaTime * 10f;
                _btn.transform.localScale = new Vector3(Mathf.SmoothStep(destinationScale.x, startingScale.x, timer2), Mathf.SmoothStep(destinationScale.y, startingScale.y, timer2), _btn.transform.localScale.z);

                yield return 0;
            }
        }
        _btn.transform.localScale = Vector3.one;
        yield break;
    }
}
