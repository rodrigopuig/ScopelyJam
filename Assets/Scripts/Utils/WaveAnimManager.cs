using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaveAnimManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] waves;

    const float fadeInTime = 0.5f;
    const float fadeOutTime = 1;
    const float delayMin = 0.05f;
    const float delayMax = 0.3f;



    private IEnumerator WaveAnimations()
    {
        float randDelayTime;
        SpriteRenderer wave;
        WaitForSeconds wait;
        while (true)
        {
            wave = waves[Random.Range(0, waves.Length)];
            FadeWave(wave);
            randDelayTime = Random.Range(delayMin, delayMax);
            wait = new WaitForSeconds(randDelayTime);
            yield return wait;
        }
    }

    private IEnumerator Start()
    {
        yield return WaveAnimations();
    }

    private void OnDestroy()
    {
        DOTween.Kill(GetInstanceID());
    }

    private void FadeWave(SpriteRenderer wave)
    {
        wave.DOKill();
        wave.DOFade(1, fadeInTime).OnStart(() => { wave.transform.DOScaleY(1, fadeInTime); }).OnComplete(() => { wave.DOFade(0, fadeOutTime * 2); wave.transform.DOScaleY(0.5f, fadeInTime); }).SetId(GetInstanceID()) ;
    }


}
