using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject menuButton;

    private bool isPaused = false;

    public AudioMixer audioMixer;

    public Slider masterVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        // Загрузка сохраненных значений громкости
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.75f);

        masterVolumeSlider.value = masterVolume;
        sfxVolumeSlider.value = sfxVolume;

        // Установка громкости в аудиомиксере непосредственно при старте
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(masterVolume) * 20);
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(sfxVolume) * 20);

        // Добавление слушателей для слайдеров
        masterVolumeSlider.onValueChanged.AddListener(delegate { SetMusicVolume(masterVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { SetSFXVolume(sfxVolumeSlider.value); });
    }

   

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save(); // Убедитесь, что настройки сохраняются при выходе из игры
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
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Пауза игры
    public void Pause()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        menuButton.SetActive(false);
        isPaused = true;
    }

    // Выход из игры
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");
        SceneManager.LoadScene("MainWindow");
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume); // Обновление сохраненной настройки громкости
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxVolume", volume); // Обновление сохраненной настройки громкости
    }
}