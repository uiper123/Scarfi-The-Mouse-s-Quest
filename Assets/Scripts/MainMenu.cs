using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public GameObject loadingScreen;
    public GameObject playGameButton;
    public GameObject continueGameButton;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
        AudioMG.instance.PlayMusic(AudioMG.instance.sceneMusicClips[0]);

        // Проверяем, есть ли сохраненные данные
        if (HasSavedGame())
        {
            // Если есть сохраненные данные, показываем кнопку "Продолжить игру" и скрываем кнопку "Играть"
            playGameButton.SetActive(false);
            continueGameButton.SetActive(true);
        }
        else
        {
            // Если нет сохраненных данных, показываем кнопку "Играть" и скрываем кнопку "Продолжить игру"
            playGameButton.SetActive(true);
            continueGameButton.SetActive(false);
        }
    }

    public void PlayGame()
    {
        // Сбрасываем все сохраненные данные
        //SaveDataResetManager.Instance.ResetAllSaveData();

        // Сбрасываем все сохраненные данные
        SaveDataResetManager.Instance.ResetAllSaveData();

        // Загружаем начальный уровень
        LevelLoader.Instance.LoadLevel("MainLevel"); 

        /*/ Загружаем уровень
        SceneManager.LoadScene(levelToLoad);

        // Обновляем прогресс загрузки (можно использовать AsyncOperation)
        LoadingScreenController.Instance.UpdateLoadingProgress(0.5f);

        // Скрываем загрузочный экран после загрузки
        LoadingScreenController.Instance.HideLoadingScreen();*/
    }
    
   
    public void ContinueGame()
    {
        SaveData savedData = SaveSystem.LoadProgress();

        if (savedData != null)
        {
            // Загружаем сохраненный уровень
            LevelLoader.Instance.LoadLevel(savedData.levelstr, savedData);
        }
        else
        {
            // Если сохраненные данные не найдены, загружаем начальный уровень
            LevelLoader.Instance.LoadLevel("MainLevel");
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
