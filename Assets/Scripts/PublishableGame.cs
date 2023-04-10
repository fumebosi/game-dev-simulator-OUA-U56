using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublishableGame 
{
    public float Profit;
    public float Cooldown;

    public string GameName;
    public float CurrentProgress;

    public PublishableGame(float profit, float cooldown, string gameName)
    {
        Profit = profit;
        Cooldown = cooldown;
        GameName = gameName;
        CurrentProgress = 0;
    }

    public Image Mask;
}
