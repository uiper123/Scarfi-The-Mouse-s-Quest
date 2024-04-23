using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject Reatgun;
    public GameObject pauseMenuUI;
    public GameObject menuButton;

    private bool isPaused = false;

    public AudioMixer audioMixer;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider sfxVolumePlayerSlider;

    public GameObject player;

    private void Start()
    {
        // Загрузка сохраненных значений громкости
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
        float sfxPlayerVolume = PlayerPrefs.GetFloat("SfxPlayerVolume", 0.75f);

        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        sfxVolumeSlider.value = sfxVolume;
        sfxVolumePlayerSlider.value = sfxPlayerVolume;

        // Установка громкости в аудио микшере при запуске
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        SetSFXPlayerVolume(sfxPlayerVolume);

        // Добавление слушателей для слайдеров
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        sfxVolumePlayerSlider.onValueChanged.AddListener(SetSFXPlayerVolume);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Сохранение настроек при выходе из игры
    }

    // Обработка нажатия кнопки паузы
    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // Продолжить игру
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        menuButton.SetActive(true);
        Reatgun.SetActive(true); // Показываем способность мышиного рыка
        Time.timeScale = 1f;
        isPaused = false;
        UIController.isPaused = false;
    }

    // Пауза игры
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        menuButton.SetActive(false);
        Reatgun.SetActive(false); // Скрываем способность мышиного рыка
        isPaused = true;
        UIController.isPaused = true;
    }

    // Выход из игры
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        pauseMenuUI.SetActive(false);
        menuButton.SetActive(true);
        AudioMG.instance.StopMusic();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainWindow");
    }

    // Методы для установки громкости
    // Methods for setting volume
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume <= 0.1f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume <= 0.1f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", volume <= 0.1f ? -80f : Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", volume); 
    }

    public void SetSFXPlayerVolume(float volume)
    {
        audioMixer.SetFloat("SfxPlayerVolume", volume <= 0.1f ? -80f : Mathf.Log10(volume) * 20); 
        PlayerPrefs.SetFloat("SfxPlayerVolume", volume); 
    }

}