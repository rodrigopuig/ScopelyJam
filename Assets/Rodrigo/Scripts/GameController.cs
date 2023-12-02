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
        public Action<int> onNextRound;
        public Action onGameFinished;

        public TextMeshProUGUI txtRound;


        int currentRound;

        private void Awake()
        {
            Tutorial.onCloseTutorial += StartGame;
        }

        IEnumerator Start()
        {
            yield return null;
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
            PlayTxtRoundAnimation();
        }

        public void NextRound()
        {
            currentRound++;
            txtRound.text = $"ROUND {currentRound + 1}";
            StartCoroutine(CoroutineUtils.DoAfterFrames(1, PlayTxtRoundAnimation));

            onNextRound?.Invoke(currentRound);
        }

        void PlayTxtRoundAnimation()
        {
            RectTransform _rtText = txtRound.GetComponent<RectTransform>();
            float _txtWidth = _rtText.sizeDelta.x;

            _rtText.anchoredPosition = new Vector2(Screen.width, _rtText.anchoredPosition.y);
            Sequence _sequence = DOTween.Sequence()
                .Append(_rtText.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack))
                .Append(_rtText.DOAnchorPosX(-Screen.width, 0.5f).SetDelay(1f));

        }
    }
}