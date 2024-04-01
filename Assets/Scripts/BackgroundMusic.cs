using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    public AudioSource audioSource;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveMusicTime()
    {
        PlayerPrefs.SetFloat("MusicTime", audioSource.time);
    }

    public void LoadMusicTime()
    {
        audioSource.time = PlayerPrefs.GetFloat("MusicTime");
    }

    public void RestartScene()
    {
        SaveMusicTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        if (!audioSource.isPlaying)
        {
            LoadMusicTime();
            audioSource.Play();
        }
    }
    
}
