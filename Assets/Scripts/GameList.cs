using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Games/GameList", fileName = "GameList")]
public class GameList : ScriptableObject
{
    public List<PublishableGamePreset> GamesList;
}
