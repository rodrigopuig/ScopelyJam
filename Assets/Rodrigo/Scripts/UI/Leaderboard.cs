using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public LeaderboardEntry entryPrefab;
    public Transform trParent;

    List<GameObject> instancedEntries;

    private void Awake()
    {
        instancedEntries = new List<GameObject>();
    }

    void FillInfo()
    {
        DataUtils.Scores scores = null;

        if(PlayerPrefs.HasKey(DataUtils.scores))
        {
            string _scores = PlayerPrefs.GetString(DataUtils.scores);
            scores = JsonUtility.FromJson<DataUtils.Scores>(_scores);

            if (scores.scoreEntries.Length > 0)
            {
                List<DataUtils.ScoreEntry> _list = new List<DataUtils.ScoreEntry>(scores.scoreEntries);
                List<DataUtils.ScoreEntry> _orderedList = (List<DataUtils.ScoreEntry>)_list.OrderByDescending(x => x.score);
                List<DataUtils.ScoreEntry> _myEntryList = new List<DataUtils.ScoreEntry>();

                for (int i = 0; i < 8 && i < _orderedList.Count; i++)
                {
                    _myEntryList.Add(_orderedList[i]);
                }

                _myEntryList.Add(new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName1), score = PlayerPrefs.GetInt(DataUtils.playerScore1) });
                _myEntryList.Add(new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName2), score = PlayerPrefs.GetInt(DataUtils.playerScore2) });

                _orderedList.Clear();

                _myEntryList.OrderByDescending(x => x.score);

                scores.scoreEntries = _myEntryList.ToArray();
            }
        }
        else
        {
            DataUtils.ScoreEntry[] _entries = new DataUtils.ScoreEntry[2]
            {
                new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName1), score = PlayerPrefs.GetInt(DataUtils.playerScore1)},
                new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName2), score = PlayerPrefs.GetInt(DataUtils.playerScore2)}
            };

            _entries.OrderByDescending(x => x.score);

            scores = new DataUtils.Scores() { scoreEntries = _entries };
        }

        string _json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString(DataUtils.scores, _json);

        for(int i = 0; i<scores.scoreEntries.Length; i++)
        {
            LeaderboardEntry _entry = Instantiate(entryPrefab, trParent);

        }
    }
}
