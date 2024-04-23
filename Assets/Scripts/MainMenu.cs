using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.SaveManager;


public class MainMenu : MonoBehaviour
{
    public int levelToLoad = 2; // Индекс сцены для загрузки
    public GameObject playGameButton;
    public GameObject continueGameButton;
    

    

    private void Start()
    {

        SaveData savedData = SaveSystem.LoadProgress();
        if (savedData != null)
        {
            // Активируйте кнопку загрузки сохраненной игры
            continueGameButton.SetActive(true);
            
        }
        else
        {
            // Деактивируйте кнопку загрузки сохраненной игры
            continueGameButton.SetActive(false);
            
        }
        
        AudioMG.instance.PlayMusic(AudioMG.instance.sceneMusicClips[0]);
        
    }

    public void PlayGame()
    {
        
            // Сбрасываем все сохраненные данные
            SaveSystem.ResetProgress();

            // Загружаем начальную сцену с Timeline
            SceneManager.LoadScene("Intro");
    }

    public void LoadSavedGame()
    {
        SaveData savedData = SaveSystem.LoadProgress(); // Получаем сохраненные данные из SaveSystem
        if (savedData != null)
        {
            LevelLoader.Instance.LoadLevel(savedData.level, savedData); // Загружаем сохраненный уровень с данными
        }
        else
        {
            Debug.LogWarning("Сохраненные данные не найдены.");
        }
    }

    public void QuitGame()
    {
        // Выход из игры
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

    private bool HasSavedGame()
    {
        // Проверяем, есть ли ключи в PlayerPrefs, которые мы используем для сохранения данных
        return PlayerPrefs.HasKey("PlayerPosition") && PlayerPrefs.HasKey("PlayerLevel");
    }
}