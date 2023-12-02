using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

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

        public void NextRound()
        {
            Debug.Log("Next Round");
            currentRound++;
            txtRound.text = $"ROUND {currentRound + 1}";
            StartCoroutine(CoroutineUtils.DoAfterFrames(1, PlayTxtRoundAnimation));

            onNextRound?.Invoke(currentRound);
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