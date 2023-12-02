using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class Menu : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform rtPlayButton, rtCreditsButton, rtExitButton;

    [Header("References")]
    public Crdits credits;
    public PlayerNamesPopUp popUp;

    bool inputAllowed;

    private void Awake()
    {
        credits.onHide += ()=> Show();
        popUp.onHide += () => Show();

        rtPlayButton.anchoredPosition = new Vector2(305f, rtPlayButton.anchoredPosition.y);
        rtCreditsButton.anchoredPosition = new Vector2(-224, rtCreditsButton.anchoredPosition.y);
        rtExitButton.localScale = Vector3.zero;

        inputAllowed = true;

        Show();
    }

    public void ShowCredits()
    {
        if (inputAllowed)
        {
            inputAllowed = false;
            Hide(credits.Show);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        if (inputAllowed)
        {
            inputAllowed = false;
            Hide(popUp.Show);
        }
    }

    public void Show(System.Action _onComplete = null)
    {
        rtExitButton.localScale = Vector3.zero;
        //canvas.enabled = true;
        Sequence _sequence = DOTween.Sequence()
            .Insert(0, rtPlayButton.DOAnchorPosX(-305, 0.5f).SetEase(Ease.OutBack))
            .Insert(0, rtCreditsButton.DOAnchorPosX(224f, 0.5f).SetEase(Ease.OutBack))
            .Insert(0.5f, rtExitButton.DOScale(1, 0.5f).SetEase(Ease.OutBack)).OnComplete(() => { _onComplete?.Invoke(); inputAllowed = true; });
    }

    public void Hide(System.Action _onComplete = null)
    {
        Sequence _sequence = DOTween.Sequence()
           .Insert(0.5f, rtPlayButton.DOAnchorPosX(305, 0.5f).SetEase(Ease.OutBack))
           .Insert(0.5f, rtCreditsButton.DOAnchorPosX(-224f, 0.5f).SetEase(Ease.OutBack))
           .Insert(0, rtExitButton.DOScale(0, 0.5f).SetEase(Ease.OutBack)).OnComplete(() => {
              // canvas.enabled = false;
               _onComplete?.Invoke();
           });
    }
}
