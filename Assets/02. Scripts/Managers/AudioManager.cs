using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    // Dictionary to manage sound clips for quick access
    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> backgroundMusic = new Dictionary<string, AudioClip>();

    // AudioSource for background music
    private AudioSource musicSource;

    // List of AudioSources for SFX pooling
    private List<AudioSource> sfxSources = new List<AudioSource>();
    public int sfxSourcePoolSize = 10; // Number of AudioSources for SFX pooling

    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    void Awake()
    {
        // Setup the music AudioSource
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = musicVolume;
        musicSource.loop = true; // Loop the music

        // Initialize SFX AudioSource pool
        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.volume = sfxVolume;
            sfxSources.Add(sfxSource);
        }
    }

    // Method to load a sound effect into the dictionary
    public void LoadSoundEffect(string key, AudioClip clip)
    {
        if (!soundEffects.ContainsKey(key))
        {
            soundEffects[key] = clip;
        }
    }

    // Method to load background music into the dictionary
    public void LoadBackgroundMusic(string key, AudioClip clip)
    {
        if (!backgroundMusic.ContainsKey(key))
        {
            backgroundMusic[key] = clip;
        }
    }

    // Play sound effect by key using an available AudioSource from the pool
    public void PlaySFX(string key)
    {
        if (soundEffects.ContainsKey(key))
        {
            AudioSource availableSource = GetAvailableSFXSource();
            if (availableSource != null)
            {
                availableSource.PlayOneShot(soundEffects[key], sfxVolume);
            }
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + key);
        }
    }

    // Play background music by key
    public void PlayMusic(string key)
    {
        if (backgroundMusic.ContainsKey(key))
        {
            musicSource.clip = backgroundMusic[key];
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music not found: " + key);
        }
    }

    // Stop the currently playing music
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Adjust SFX volume
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        foreach (AudioSource source in sfxSources)
        {
            source.volume = sfxVolume;
        }
    }

    // Adjust Music volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume;
    }

    // Get an available AudioSource from the pool
    private AudioSource GetAvailableSFXSource()
    {
        foreach (AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        Debug.LogWarning("No available AudioSource to play SFX");
        return null;
    }
}
