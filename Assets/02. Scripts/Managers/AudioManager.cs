using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance = null;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    [Header("BGM AudioSource")]
    public AudioSource audioSource1;

    [Header("SFX AudioSource")]
    public AudioSource audioSource2;

    [Header("SFX AudioClip")]
    public AudioClip GetCoinSFX;
    public AudioClip JumpSFX;
    public AudioClip SlidingSFX;
    public AudioClip LeftRightSFX;

    public void JumpSFXPlay()
    {
        audioSource2.clip = JumpSFX;
        audioSource2.Play();
    }

    public void SlidingSFXPlay()
    {
        audioSource2.clip = SlidingSFX;
        audioSource2.Play();
    }

    public void LeftRightSFXPlay()
    {
        audioSource2.clip = LeftRightSFX;
        audioSource2.Play();
    }

    public void GetCoinSFXPlay()
    {
        audioSource2.clip = GetCoinSFX;
        audioSource2.Play();
    }

}
