using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameProfitHandler : MonoBehaviour
{
    public static GameProfitHandler Instance = null;
    public GameList GL;
    public GameObject[] GamePrefabs;
    public GameObject[] GameDevelopPrefabs;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameProfitHandler found. Destroying " + gameObject.name);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializeGames();
        InitializeGameVisuals();
    }
    private void InitializeGames()
    {
        foreach (PublishableGamePreset GP in GL.GamesList)
        {
            PublishableGame game = new PublishableGame(GP.GameProfit, GP.GameCooldown, GP.GameName);
            gamesToBeInitialized.Add(game);
        }
    }

    List<PublishableGame> allGames = new List<PublishableGame>();
    List<PublishableGame> gamesToBeInitialized = new List<PublishableGame>();
    private void InitializeGameVisuals()
    {
        foreach (GameObject gamePrefab in GamePrefabs)
        {
            foreach (PublishableGame game in gamesToBeInitialized)
            {
                if (gamePrefab.name == game.GameName)
                {
                    game.Mask = gamePrefab.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>();
                    gamePrefab.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().sprite = FindNeededSprite(game.GameName);
                    
                    allGames.Add(game);
                    gamesToBeInitialized.Remove(game);
                    break;
                }
            }
        }
    }
    Sprite FindNeededSprite(string nameOfGame)
    {
        foreach (PublishableGamePreset game in GL.GamesList)
        {
            if (nameOfGame == game.GameName)
            {
                return game.GameSprite;
            }
        }
        return null;
    }

    List<PublishableGame> publishableGames = new List<PublishableGame>();
    private void FixedUpdate()
    {
        if (publishableGames.Count > 0)
        {
            for (int i = 0; i < publishableGames.Count; i++)
            {
                HandleSaleProgress(publishableGames[i]);
            }
        }
    }
    private void Update()
    {
        HandleClicks();
    }
    private void HandleSaleProgress(PublishableGame game)
    {
        game.CurrentProgress += Time.deltaTime;
        float progress = game.CurrentProgress / game.Cooldown;
        game.Mask.fillAmount = progress;

        if (progress >= 1)
        {
            GameHandler.Instance.EarnProfitFromGame(game.Profit + (float)(game.Profit / Random.Range(2.5f, 6f)));
            if (GameHandler.Instance.Money >= 4000000)
            {
                hasGtaMoney = true;
                if (hasGtaMoney && hasGtaSkills)
                {
                    foreach (GameObject item in GameDevelopPrefabs)
                    {
                        if (item.name == "gta_purchase")
                        {
                            item.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.cyan;
                        }
                    }
                }
            }
            game.CurrentProgress = 0;
            game.Mask.fillAmount = game.CurrentProgress / game.Cooldown;
        }
    }

    [HideInInspector] public bool canDevelopTerraria = false;
    [HideInInspector] public bool canDevelopMinecraft = false;
    [HideInInspector] public bool hasGtaSkills = false;
    [HideInInspector] public bool hasGtaMoney = false;

    private RaycastHit hitInfo;
    char[] delimiterChar = { '_' };
    [SerializeField] LayerMask gamePurchaseLayer;
    private void HandleClicks()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1000, gamePurchaseLayer))
            {
                GameObject objectHit = hitInfo.transform.gameObject;
                string[] separatedWords = objectHit.name.Split(delimiterChar);

                //aþaðýdaki kýsým zaman yetmeyeceði için böyle yazýldý. 
                if (separatedWords[0] == "terraria" && canDevelopTerraria)
                {
                    Debug.Log("terraria purcahse girdi");
                    foreach (PublishableGame game in allGames)
                    {
                        if (game.GameName == separatedWords[0])
                        {
                            objectHit.gameObject.SetActive(false);
                            foreach (GameObject publishableGame in GamePrefabs)
                            {
                                if (publishableGame.name == game.GameName)
                                {
                                    publishableGame.SetActive(true);
                                    publishableGames.Add(game);
                                }
                            }
                        }
                    }
                }
                else if (separatedWords[0] == "minecraft" && canDevelopMinecraft)
                {
                    foreach (PublishableGame game in allGames)
                    {
                        if (game.GameName == separatedWords[0])
                        {
                            objectHit.gameObject.SetActive(false);
                            foreach (GameObject publishableGame in GamePrefabs)
                            {
                                if (publishableGame.name == game.GameName)
                                {
                                    publishableGame.SetActive(true);
                                    publishableGames.Add(game);
                                }
                            }
                        }
                    }
                }
                else if (separatedWords[0] == "gta" && hasGtaSkills)
                {
                    foreach (PublishableGame game in allGames)
                    {
                        if (game.GameName == separatedWords[0])
                        {
                            objectHit.gameObject.SetActive(false);
                            foreach (GameObject publishableGame in GamePrefabs)
                            {
                                if (publishableGame.name == game.GameName)
                                {
                                    publishableGame.SetActive(true);
                                    publishableGames.Add(game);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
