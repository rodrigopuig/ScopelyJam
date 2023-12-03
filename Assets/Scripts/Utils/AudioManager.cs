using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip pushSound;
    [SerializeField] AudioClip fightSound;
    [SerializeField] AudioClip clashSound;
    [SerializeField] AudioClip walkSound;
    [SerializeField] AudioSource musicSC;
    AudioSource audioSC;

    const float musicFadeTime = 4f;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(transform.gameObject);
        audioSC = GetComponent<AudioSource>();
    }

    private IEnumerator Start()
    {
        yield return MusicSCFade(musicSC.volume);
    }

    private IEnumerator MusicSCFade(float targetVolume)
    {
        float elapsed = 0;
        musicSC.volume = 0;
        while (elapsed < musicFadeTime)
        {
            elapsed += Time.deltaTime;
            musicSC.volume = Mathf.Lerp(0, targetVolume, elapsed / musicFadeTime);
            yield return null;
        }
    }

    public void PlayButtonSound()
    {
        audioSC.PlayOneShot(buttonSound);
    }
    public void PlayHitSound()
    {
        audioSC.PlayOneShot(hitSound);
    }
    public void PlayPushSound()
    {
        audioSC.PlayOneShot(pushSound);
    }
    public void PlayFightSound()
    {
        audioSC.PlayOneShot(fightSound);
    }
    public void PlayClashSound()
    {
        audioSC.PlayOneShot(clashSound);
    }
    public void PlayWalkSound()
    {
        audioSC.PlayOneShot(walkSound);
    }
}
