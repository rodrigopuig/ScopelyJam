using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using Palmmedia.ReportGenerator.Core.Parser.Filtering;
using UnityEngine.SceneManagement;

namespace Rodrigo
{
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// parameter - round index
        /// </summary>
        /// 
        public static GameController instance;
        public static Action<int> onNextRound;
        public static Action onGameFinished;

        public GameObject PlayersPrefab;
        public GameObject Players;
        public Player player1;
        public Player player2;
        public bool player1Advantage;
        public bool player2Advantage;

        public TextMeshProUGUI txtRound;


        int currentRound;

        private void Awake()
        {
            Tutorial.onCloseTutorial += StartGame;
            instance = this;
        }

        IEnumerator Start()
        {
            yield return null;
            Time.timeScale = 0.00001f;
        }

        public void Update()
        {


            if (Input.GetKeyDown(KeyCode.Escape))
                ReloadScene();
        }

        void ReloadScene()
        {
            string _sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
        }

        void StartGame()
        {
            Time.timeScale = 1;
            PlayTxtRoundAnimation();
        }

        public void NextRound(Player loser)
        {
            Debug.Log("Next Round");
            Fader.FadeIn(()=>
            {
                // Debug.Log("FadeIn");
                if(loser == player2 && player1Advantage)
                {
                    Debug.Log("Player 1 WINS");
                    PlayerPrefs.SetInt(DataUtils.playerScore1, 1);
                    PlayerPrefs.SetInt(DataUtils.playerScore2, 0);
                    SceneManager.LoadScene("GameFinished");
                }
                else if(loser == player1 && player2Advantage)
                {
                    Debug.Log("Player 2 WINS");
                    PlayerPrefs.SetInt(DataUtils.playerScore2, 1);
                    PlayerPrefs.SetInt(DataUtils.playerScore1, 0);
                    SceneManager.LoadScene("GameFinished");
                }
                else if (loser == player2 && player2Advantage)
                {
                    player1Advantage = false;
                    player2Advantage = false;
                }
                else if (loser == player1 && player1Advantage)
                {
                    player2Advantage = false;
                    player1Advantage = false;
                }
                else if(loser == player2)
                {
                    player1Advantage = true;
                    player2Advantage = false;
                }
                else if(loser == player1)
                {
                    player2Advantage = true;
                    player1Advantage = false;
                }

                Destroy(Players);
                Players = Instantiate(PlayersPrefab);
                player1 = Players.transform.GetChild(0).GetComponent<Player>();
                player2 = Players.transform.GetChild(1).GetComponent<Player>();
                player1.won = player1Advantage;
                player2.won = player2Advantage;
                player1.lost = player2Advantage && !player1Advantage;
                player2.lost = player1Advantage && !player2Advantage;
                player1.UpdateBoxes();
                player2.UpdateBoxes();

                // Debug.Log("FadeOut1");
                Fader.FadeOut(() =>
                {
                    // Debug.Log("FadeOut2");
                    Time.timeScale = 1;
                    // Debug.Log("Next Round");
                    currentRound++;
                    txtRound.text = $"ROUND {currentRound + 1}";
                    StartCoroutine(CoroutineUtils.DoAfterFrames(1, PlayTxtRoundAnimation));

                    onNextRound?.Invoke(currentRound);
                });
            });
            
        }

        void PlayTxtRoundAnimation()
        {
            RectTransform _rtText = txtRound.GetComponent<RectTransform>();

            _rtText.anchoredPosition = new Vector2(Screen.width, _rtText.anchoredPosition.y);
            Sequence _sequence = DOTween.Sequence().SetUpdate(true)
                .Append(_rtText.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack))
                .Append(_rtText.DOAnchorPosX(-Screen.width, 0.5f).SetDelay(1f));
        }

        public static void FinishGame()
        {

        }

        private void OnDestroy()
        {
            onNextRound = null;
            onGameFinished = null;

        }
    }
}