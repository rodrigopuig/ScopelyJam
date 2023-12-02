using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class PlayerNamesPopUp : MonoBehaviour
{
    public System.Action onHide;

    public Canvas canvas;

    public Image imgBg;

    public TMP_InputField if_player1;
    public TMP_InputField if_player2;

    public RectTransform rtContinueButton;
    public RectTransform rtPopUp;

    public GameObject goBtnExit;

    bool inputAllowed;
    bool isButtonShown;

    public void Show()
    {
        rtContinueButton.gameObject.SetActive(false);

        rtPopUp.anchoredPosition = new Vector2(rtPopUp.anchoredPosition.x, -rtPopUp.sizeDelta.y * 1.2f);

        canvas.enabled = true;
        Sequence _sequence = DOTween.Sequence()
            .Insert(0, imgBg.DOFade(0.4f, 1f))
            .Insert(0, rtPopUp.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack)).OnComplete(() =>
            {
                goBtnExit.SetActive(true);
                inputAllowed = true;
            });
    }

    public void Hide()
    {
        if (!inputAllowed)
            return;

        goBtnExit.SetActive(false);


        Sequence _sequence = DOTween.Sequence()
            .Insert(0, imgBg.DOFade(0f, 1f))
            .Insert(0, rtPopUp.DOAnchorPosY(-rtPopUp.sizeDelta.y * 1.2f, 1).SetEase(Ease.OutBack)).OnComplete(() =>
            {
                onHide?.Invoke();
                goBtnExit.SetActive(true);
                canvas.enabled = false;
            });
    }

    private void Update()
    {
        if(!string.IsNullOrEmpty(if_player1.text) &&
            !string.IsNullOrEmpty(if_player2.text) && !isButtonShown)
        {
            isButtonShown = true;
            ShowButton();
        }
        else if(string.IsNullOrEmpty(if_player1.text) ||
            string.IsNullOrEmpty(if_player2.text) && isButtonShown)
        {
            isButtonShown = false;
            HideButton();
        }
    }

    void ShowButton()
    {
        rtContinueButton.gameObject.SetActive(true);
        rtContinueButton.anchoredPosition = Vector2.zero;
        rtContinueButton.DOAnchorPosX(560, .4f);
    }

    void HideButton()
    {
        rtContinueButton.gameObject.SetActive(true);
        rtContinueButton.DOAnchorPosX(0, .2f).OnComplete(()=>rtContinueButton.gameObject.SetActive(false));
    }

    public void PressButton()
    {
        if(inputAllowed)
        {
            inputAllowed = false;
            StartCoroutine(CoroutineUtils.DoAfterDelay(0.5f, () => UnityEngine.SceneManagement.SceneManager.LoadScene("Game")));
        }
    }
}
