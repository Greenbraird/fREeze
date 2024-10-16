using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using UnityEditor.Rendering;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioMixer mainMixer;

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip[] bgmClips;  

    [SerializeField] private AudioClip[] sfxClips;

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

        Debug.LogFormat("{0}이 실행 되었습니다.", sfxClips[indexnumber].name);
        audioSource.clip = sfxClips[indexnumber];
        audioSource.Play();
        Destroy(audioSource, sfxClips[indexnumber].length);
    }
}