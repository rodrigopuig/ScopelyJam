using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUtils
{
    public const string playerName1 = "player1_name";
    public const string playerName2 = "player2_name";

    public const string playerScore1 = "player1_name";
    public const string playerScore2 = "player2_name";

    public const string scores = "scores"; //array serializado

    [System.Serializable]
    public class Scores
    {
        public ScoreEntry[] scoreEntries;
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;
    }
}
