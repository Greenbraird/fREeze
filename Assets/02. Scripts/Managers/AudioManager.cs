using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using UnityEditor.Rendering;
using UnityEditor.Build.Reporting;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] bgmClips;  

    [SerializeField] private AudioClip[] sfxClips;

    [SerializeField] private AudioClip[] footClips;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        bgmSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("BGM")[0];
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayBGM(scene.buildIndex); // 해당 씬에 맞는 BGM 재생
        if(scene.buildIndex == 2)
        {
            mainMixer.SetFloat("BGMLowpass",500);
        }
    }

    public void PlayBGM(int sceneIndex)
    {
        if (bgmClips.Length > sceneIndex && bgmClips[sceneIndex] != null)
        {
            
            if(bgmSource.clip != bgmClips[sceneIndex])
            {
                bgmSource.clip = bgmClips[sceneIndex];
                bgmSource.Play();
            }
      
        }
    }

    public void SFXPlay(GameObject playgameObject, int indexnumber)
    {
        AudioSource audioSource = playgameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = sfxClips[indexnumber];
        audioSource.Play();
        Destroy(audioSource, sfxClips[indexnumber].length);
    }

    public void footSFXPlay(GameObject playobject, int step)
    {
        AudioSource audioSource = playobject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mainMixer.FindMatchingGroups("SFX")[0];

        audioSource.clip = footClips[step];
        audioSource.Play();
        Destroy(audioSource, footClips[step].length);
    }
}