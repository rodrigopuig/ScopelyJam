using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


public class ScalableButton : MonoBehaviour
{
    public void COnMouseDown()
    {
        DOTween.Kill(GetInstanceID());
        transform.DOScale(0.8f, 0.15f).SetId(GetInstanceID());
    }

    public void COnMouseUp()
    {
        DOTween.Kill(GetInstanceID());
        transform.DOScale(1f, 0.5f).SetEase(Ease.OutBack).SetId(GetInstanceID());
    }
}
