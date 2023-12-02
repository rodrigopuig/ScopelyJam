using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class Crdits : MonoBehaviour
{
    public System.Action onHide;

    public Canvas canvas;

    public Image imgBg;
    public RectTransform trCharacters;
    public TextMeshProUGUI[] txts;

    public GameObject goBtnExit;

    bool inputAllowed;

    public void Show()
    {
        SetTextsVisibility(false);

        trCharacters.anchoredPosition = new Vector2(trCharacters.anchoredPosition.x, -trCharacters.sizeDelta.y * 1.2f);

        canvas.enabled = true;
        Sequence _sequence = DOTween.Sequence()
            .Insert(0, imgBg.DOFade(0.4f, 1f))
            .Insert(0, trCharacters.DOAnchorPosY(0, 0.8f).SetEase(Ease.OutBack)).OnComplete(()=>
            {
                SetTextsVisibility(true);
                goBtnExit.SetActive(true);

                inputAllowed = true;
            });
    }

    public void Hide()
    {
        if (!inputAllowed)
            return;

        goBtnExit.SetActive(false);
        SetTextsVisibility(false);


        Sequence _sequence = DOTween.Sequence()
            .Insert(0, imgBg.DOFade(0f, 1f))
            .Insert(0, trCharacters.DOAnchorPosY(-trCharacters.sizeDelta.y * 1.2f, 1).SetEase(Ease.OutBack)).OnComplete(() =>
            {
                onHide?.Invoke();
                goBtnExit.SetActive(true);
                canvas.enabled = false;
            });
    }

    void SetTextsVisibility(bool _state)
    {
        foreach (var txt in txts)
            txt.gameObject.SetActive(_state);
    }

}
