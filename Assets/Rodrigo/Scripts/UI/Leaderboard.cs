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

        PlayerPrefs.SetString(DataUtils.playerName1, "Javi");
        PlayerPrefs.SetInt(DataUtils.playerScore1, 12);

        PlayerPrefs.SetString(DataUtils.playerName2, "Dani");
        PlayerPrefs.SetInt(DataUtils.playerScore2, 5);

        DataUtils.Scores scores = new DataUtils.Scores()
        {
            scoreEntries = new DataUtils.ScoreEntry[10]
             {
                 new DataUtils.ScoreEntry() { playerName = "Pablo", score = 10},
                 new DataUtils.ScoreEntry() { playerName = "Carmen", score = 6},
                 new DataUtils.ScoreEntry() { playerName = "Fer", score = 8},
                 new DataUtils.ScoreEntry() { playerName = "Paz", score = 20},
                 new DataUtils.ScoreEntry() { playerName = "Julio", score = 15},
                 new DataUtils.ScoreEntry() { playerName = "Ale", score = 9},
                 new DataUtils.ScoreEntry() { playerName = "Jesus", score = 11},
                 new DataUtils.ScoreEntry() { playerName = "Polo", score = 4},
                 new DataUtils.ScoreEntry() { playerName = "Erik", score = 2},
                 new DataUtils.ScoreEntry() { playerName = "Alvaro", score = 3},
             }
        };

        PlayerPrefs.SetString(DataUtils.scores, JsonUtility.ToJson(scores));

        FillInfo();
    }



    void FillInfo()
    {
        DataUtils.Scores scores = null;

        foreach (var go in instancedEntries)
            Destroy(go);

        instancedEntries.Clear();

        if (PlayerPrefs.HasKey(DataUtils.scores))
        {
            string _scores = PlayerPrefs.GetString(DataUtils.scores);
            scores = JsonUtility.FromJson<DataUtils.Scores>(_scores);

            if (scores.scoreEntries.Length > 0)
            {
                List<DataUtils.ScoreEntry> _list = new List<DataUtils.ScoreEntry>(scores.scoreEntries);
                List<DataUtils.ScoreEntry> _orderedList = new  List<DataUtils.ScoreEntry>( _list.OrderByDescending(x => x.score));
                List<DataUtils.ScoreEntry> _myEntryList = new List<DataUtils.ScoreEntry>();

                DataUtils.ScoreEntry _player1 = new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName1), score = PlayerPrefs.GetInt(DataUtils.playerScore1) };
                DataUtils.ScoreEntry _player2 = new DataUtils.ScoreEntry() { playerName = PlayerPrefs.GetString(DataUtils.playerName2), score = PlayerPrefs.GetInt(DataUtils.playerScore2) };

                int _numberOfElements = 8;

                if (!IsThere(_player1, _list))
                    _numberOfElements -= 1;

                if (!IsThere(_player2, _list))
                    _numberOfElements -= 1;

                for (int i = 0; i < _numberOfElements && i < _orderedList.Count; i++)
                {
                    _myEntryList.Add(_orderedList[i]);
                }

                if(!FindAndSubstitute(_player1, ref _myEntryList))
                    _myEntryList.Add(_player1);

                if (!FindAndSubstitute(_player2, ref _myEntryList))
                    _myEntryList.Add(_player2);

                _orderedList.Clear();

               _myEntryList =new List<DataUtils.ScoreEntry>(_myEntryList.OrderByDescending(x => x.score));

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

           _entries = _entries.OrderByDescending(x => x.score).ToArray();

            scores = new DataUtils.Scores() { scoreEntries = _entries };
        }

        string _json = JsonUtility.ToJson(scores);
        PlayerPrefs.SetString(DataUtils.scores, _json);

        for (int i = 0; i < scores.scoreEntries.Length; i++)
        {
            LeaderboardEntry _entry = Instantiate(entryPrefab, trParent);
            _entry.txtPosition.text = $"{i+1}";
            _entry.txtPlayerName.text = scores.scoreEntries[i].playerName;
            _entry.txtScore.text = scores.scoreEntries[i].score.ToString();

            instancedEntries.Add(_entry.gameObject);
        }
    }

    bool FindAndSubstitute(DataUtils.ScoreEntry _entry, ref List<DataUtils.ScoreEntry> _allEntries)
    {
        bool _found = false;
        for(int i = 0; i<_allEntries.Count && !_found; i++)
        {
            if (_entry.playerName == _allEntries[i].playerName)
            {
                _found = true;

                if (_entry.score > _allEntries[i].score)
                    _allEntries[i] = _entry;
            }
        }

        return _found;
    }

    bool IsThere(DataUtils.ScoreEntry _entry, List<DataUtils.ScoreEntry> _allEntries)
    {
        bool _found = false;
        for (int i = 0; i < _allEntries.Count && !_found; i++)
        {
            if (_entry.playerName == _allEntries[i].playerName)
            {
                _found = true;
            }
        }

        return _found;
    }
}
