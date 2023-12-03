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
    [SerializeField] AudioClip musicSound;
    [SerializeField] AudioSource musicSC;
    AudioSource audioSC;

    const float musicFadeTime = 4f;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
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
        PlaySound(buttonSound);
    }
    public void PlayHitSound()
    {
        PlaySound(hitSound, 1, 0.8f, 1.2f);
    }
    public void PlayPushSound()
    {
        PlaySound(pushSound);
    }
    public void PlayFightSound()
    {
        PlaySound(fightSound);
    }
    public void PlayClashSound()
    {
        PlaySound(clashSound);
    }
    public void PlayWalkSound()
    {
        PlaySound(walkSound, 0.6f, 0.8f, 1.2f);
    }

    private void PlaySound(AudioClip original, float volume = 1, float minPitch = 1, float maxPitch = 1)
    {
        audioSC.volume = volume;
        if (minPitch != 1 || maxPitch != 1)
        {
            audioSC.pitch = Random.Range(minPitch, maxPitch);
        }
        else
        {
            audioSC.pitch = 1;
        }
        audioSC.PlayOneShot(original);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayWalkSound();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayHitSound();
        }
    }
}
