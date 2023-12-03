using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AutoLabel : MonoBehaviour
{
    public enum PlayerId { Player_1, Player_2}

    public PlayerId playerID;
    public TextMeshPro text;

    private void Awake()
    {
        if (playerID == PlayerId.Player_1)
            text.text = PlayerPrefs.GetString(DataUtils.playerName1);
        else
            text.text = PlayerPrefs.GetString(DataUtils.playerName2);
    }
}
