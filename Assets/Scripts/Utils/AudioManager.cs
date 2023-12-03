using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip pushSound;
    [SerializeField] AudioClip fightSound;
    [SerializeField] AudioClip clashSound;
    [SerializeField] AudioClip walkSound;
    AudioSource audioSC;

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
        audioSC = GetComponent<AudioSource>();
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
