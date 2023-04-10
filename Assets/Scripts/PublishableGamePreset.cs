using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Games/GameBase", fileName = "PublishableGamePreset")]
public class PublishableGamePreset : ScriptableObject
{
    public Sprite GameSprite;
    public string GameName;
    public float GameProfit;
    public float GameCooldown;
}
