using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Fader : MonoBehaviour
{
    public static Fader Instance;

    public Material mat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Instance.mat.SetFloat("_Offset", 1);

            StartCoroutine(CoroutineUtils.DoAfterDelay(0.5f, () =>
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += (_scene, mode) =>
                {
                    FadeOut();
                    Debug.Log("Faded Out");
                };
            }));
            
        }
        else
            Destroy(gameObject);
    }

    public static void FadeIn(Action _onComplete = null)
    {
        float _value = 1;
        DOTween.To(() => _value, x => _value = x, 0, 0.8f).OnUpdate(() => { Instance.mat.SetFloat("_Offset", _value); }).OnComplete(() => { _onComplete?.Invoke(); });
    }

    public static void FadeOut(Action _onComplete = null)
    {
        float _value = 0;
        DOTween.To(() => _value, x => _value = x, 1, 0.8f).OnUpdate(() => { Instance.mat.SetFloat("_Offset", _value); }).OnComplete(() => { _onComplete?.Invoke(); }); ;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,100,100), "FadeIn")) { FadeIn(); }
        if (GUI.Button(new Rect(100, 0, 100, 100),"FadeOut")) { FadeOut(); }
    }
#endif
}
