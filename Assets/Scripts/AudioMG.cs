using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioMG : MonoBehaviour
{
    public static AudioMG instance;

    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;
    public AudioMixerGroup playerSfxMixerGroup;

    public AudioSource musicSource;
    public AudioSource[] sfxSources;
    public AudioClip[] sfxClips;
    public AudioSource[] playerSfxSources;
    public AudioClip[] sceneMusicClips;
    public AudioClip[] playerSfx;
    

    
    public AudioMixerGroup enemyMixerGroup;
    public AudioClip EnemySfx;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSourcesPool();
            InitializePlayerAudioSourcesPool(); // Вызываем метод инициализации
        }
        else
        {
            Destroy(gameObject);
        }
        
        
        // Назначение AudioMixerGroup для каждого AudioSource
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        foreach (AudioSource sfxSource in sfxSources)
        {
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
        }
        
        // Назначение AudioMixerGroup для каждого AudioSource звуков персонажа
        foreach (AudioSource sfxSource in playerSfxSources)
        {
            sfxSource.outputAudioMixerGroup = playerSfxMixerGroup;
        }
    }
    
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void InitializeAudioSourcesPool()
    {
        // Инициализация пула звуковых эффектов игрока
        sfxSources = new AudioSource[5]; // Пример размера пула для звуков игрока
        for (int i = 0; i < sfxSources.Length; i++)
        {
            GameObject obj = new GameObject("SfxSourse" + i);
            obj.transform.SetParent(transform);
            sfxSources[i] = obj.AddComponent<AudioSource>();
            sfxSources[i].outputAudioMixerGroup = sfxMixerGroup;
        }
    }
    
    private void InitializePlayerAudioSourcesPool()
    {
        // Инициализация пула звуковых эффектов игрока
        playerSfxSources = new AudioSource[3]; // Пример размера пула для звуков игрока
        for (int i = 0; i < playerSfxSources.Length; i++)
        {
            GameObject obj = new GameObject("PlayerSFXSource_" + i);
            obj.transform.SetParent(transform);
            playerSfxSources[i] = obj.AddComponent<AudioSource>();
            playerSfxSources[i].outputAudioMixerGroup = playerSfxMixerGroup;
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicBySceneIndex(scene.buildIndex);
    }
    
    public void PlaySfx(int sfxIndex)
    {
        if (sfxIndex >= 0 && sfxIndex < sfxClips.Length)
        {
            AudioSource sourceToPlay = FindAvailableSourceInPool();
            if (sourceToPlay != null)
            {
                sourceToPlay.clip = sfxClips[sfxIndex];
                sourceToPlay.Play();
            }
            else
            {
                Debug.LogWarning("No available audio source in the pool to play SFX.");
            }
        }
        else
        {
            Debug.LogWarning("SFX clip index out of range: " + sfxIndex);
        }
    }
    
    public void PlayMusicBySceneIndex(int index)
    {
        if (musicSource != null) // Проверка, что musicSource не равен null
        {
            if (index >= 0 && index < sceneMusicClips.Length)
            {
                musicSource.clip = sceneMusicClips[index];
                musicSource.Play();
            }
            else
            {
                Debug.LogWarning("Music clip not found for scene index: " + index);
            }
        }
        else
        {
            Debug.LogError("Music source is not assigned or has been destroyed.");
        }
    }

    public void PlayMusic(AudioClip musicClip)
    {
        if (musicSource != null)
        {
            if (musicSource.clip != musicClip)
            {
                musicSource.clip = musicClip;
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("Music source is not assigned!");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
        else
        {
            Debug.LogWarning("Music source is not assigned!");
        }
    }

    

    public void PlaySfx(AudioClip clip)
    {
        AudioSource sourceToPlay = FindAvailableSourceInPool();
        if (sourceToPlay != null)
        {
            sourceToPlay.clip = clip;
            sourceToPlay.Play();
        }
        else
        {
            Debug.LogWarning("No available audio source in the pool to play SFX.");
        }
    }
    private AudioSource FindAvailableSourceInPool()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null; // Все источники заняты
    }

    public void PlayPlayerSfx(int sfxIndex)
    {
        // Воспроизведение звуковых эффектов игрока
        if (sfxIndex >= 0 && sfxIndex < playerSfx.Length)
        {
            AudioSource sourceToPlay = FindAvailablePlayerSourceInPool();
            if (sourceToPlay != null)
            {
                sourceToPlay.clip = playerSfx[sfxIndex];
                sourceToPlay.Play();
            }
            else
            {
                Debug.LogWarning("No available player audio source in the pool to play SFX.");
            }
        }
        else
        {
            Debug.LogWarning("Player SFX clip index out of range: " + sfxIndex);
        }
    }
    private AudioSource FindAvailablePlayerSourceInPool()
    {
        // Поиск доступного AudioSource для звуков игрока
        foreach (var source in playerSfxSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }
        return null; // Все источники заняты
    }
}
