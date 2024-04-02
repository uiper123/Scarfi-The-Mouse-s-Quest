using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverPanel1 : MonoBehaviour
{
    public GameObject gameOverMenuUI , menuButton;
    
    public static GameOverPanel1 Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Защита от уничтожения
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }
    }

    // Показать меню проигрыша
    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f; // Остановить время в игре
        gameOverMenuUI.SetActive(true);
        menuButton.SetActive(false);
    }

    // Перезапустить уровень
    public void RestartLevel()
    {
        Time.timeScale = 1f; // Возобновить время в игре
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Загрузить текущий уровень заново
    }

    // Выйти из игры
    public void QuitGame()
    {
        Time.timeScale = 1f; // Возобновить время в игре
        Debug.Log("Выход из игры..."); // Сообщение в консоль
        Application.Quit(); // Закрыть игру
    }

}
