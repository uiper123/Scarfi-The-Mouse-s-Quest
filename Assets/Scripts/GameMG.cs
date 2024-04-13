using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMG : MonoBehaviour
{
    public static GameMG instance = null;
    public GameObject gameOverMenuUI;
    
    private int currentCheckpointIndex = 0;
    private bool hasCheckpoint = false; // переменная для хранения информации о наличии сохранения

    private PauseMenu _pausemenu;
    public GameObject loadCheckpointButton; // ссылка на кнопку загрузки чекпоинта
    private void Awake()
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

    public void GameOver()
    {
        if (PlayerController.instance != null)
        {
            // Удаляем персонажа при смерти
            Destroy(PlayerController.instance.gameObject);
        }
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Сбрасываем текущий чекпоинт, чтобы при перезапуске уровня персонаж появлялся на начальной позиции
        currentCheckpointIndex = 0;
        // Также обнуляем объект персонажа, чтобы при перезапуске уровня не оставался ненужный объект персонажа в памяти
        PlayerController.instance = null;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Выход из игры...");
        SceneManager.LoadScene("MainWindow");
    }

    public void SetCurrentCheckpoint(int index)
    {
        currentCheckpointIndex = index;
    }
    
    // Метод для установки переменной hasCheckpoint
    public void SetHasCheckpoint(bool value)
    {
        hasCheckpoint = value;
    }
    
    public void LoadCheckpoint()
    {
        // Загрузка чекпоинта
        LevelManagers.instance.LoadCheckpoint();
        // Закрыть главное меню
        _pausemenu.pauseMenuUI.SetActive(false);
        // Продолжить игру
        Time.timeScale = 1f;
    }
    
    public void OpenMainMenu()
    {
        // Проверяем наличие сохранения
        if (hasCheckpoint)
        {
            // Если есть сохранение, показываем кнопку загрузки чекпоинта
            loadCheckpointButton.SetActive(true);
        }
        else
        {
            // Если сохранения нет, скрываем кнопку загрузки чекпоинта
            loadCheckpointButton.SetActive(false);
        }

        // Открываем главное меню
        _pausemenu.pauseMenuUI.SetActive(true);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, является ли загруженная сцена игровой сценой
        if (scene.buildIndex != 0) // Предполагается, что основное меню имеет индекс 0
        {
            // При загрузке игровой сцены сбрасываем текущий чекпоинт
            currentCheckpointIndex = 0;
        }
    }
    
    public void ContinueGame()
    {
        // Проверяем наличие сохраненного чекпоинта
        if (PlayerPrefs.HasKey("CheckpointIndex") && PlayerPrefs.HasKey("SceneName"))
        {
            Time.timeScale = 1f;
            _pausemenu.pauseMenuUI.SetActive(false);
            gameOverMenuUI.SetActive(false); // Скрытие меню Game Over
            // Загрузка сохраненной сцены
            string sceneName = PlayerPrefs.GetString("SceneName");
            SceneManager.LoadScene(sceneName);
            // Установка чекпоинта в соответствии с сохраненным значением
            int checkpointIndex = PlayerPrefs.GetInt("CheckpointIndex");
            LevelManagers.instance.SetCurrentCheckpoint(checkpointIndex);
        }
        else
        {
            // Если сохраненного чекпоинта нет, ничего не делаем
            Debug.Log("No checkpoint saved.");
        }
    }
}
