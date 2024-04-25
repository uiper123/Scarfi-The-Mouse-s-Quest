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
    private bool hasCheckpoint = false;

    private PauseMenu _pausemenu;
    public GameObject loadCheckpointButton;

    public GameObject Reatgun;
    public GameObject menuButton;
    public GameObject TExtScore;
    public GameObject CountItem;


    private void Awake()
    {
        // Синглтон паттерн для GameMG
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Инициализация _pausemenu
        _pausemenu = GetComponent<PauseMenu>();
    }

    public void GameOver()
    {
        if (PlayerController.instance != null)
        {
            // Удаляем персонажа при смерти
            Destroy(PlayerController.instance.gameObject);
            UIController.Instance.mouseRoarPanel.SetActive(false); // Скрываем RoatGun в меню Game Over
            gameOverMenuUI.SetActive(true);
            menuButton.SetActive(false);
            CountItem.SetActive(false);
            TExtScore.SetActive(false);
            Reatgun.gameObject.SetActive(false); // Отключаем отображение Reatgun
            //Time.timeScale = 0;
        }
        
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        gameOverMenuUI.SetActive(false);
        menuButton.SetActive(true);
        CountItem.SetActive(true);
        TExtScore.SetActive(true);
        gameOverMenuUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Сбрасываем текущий чекпоинт и объект персонажа
        currentCheckpointIndex = 0;
        UIController.Instance.ResetUI();
        PlayerController.instance = null;

        // Запускаем возрождение персонажа с задержкой
        StartCoroutine(RespawnPlayerWithDelay());
    }

    private IEnumerator RespawnPlayerWithDelay()
    {
        // Небольшая задержка для обеспечения загрузки сцены
        yield return new WaitForSeconds(0.1f); 

        // Получаем объект игрока и компоненты
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController playerController = player.GetComponent<PlayerController>();
        HelthManager healthManager = player.GetComponent<HelthManager>();

        // Устанавливаем позицию игрока в зависимости от чекпоинта
        if (CheckpointExists(currentCheckpointIndex))
        {
            player.transform.position = LevelManagers.instance.checkpointPositions[currentCheckpointIndex].position;
        }
        else
        {
            player.transform.position = LevelManagers.instance.startMarker.position;
        }

        // Восстанавливаем здоровье и другие атрибуты
        //healthManager.currentHealth = healthManager.maxHealth;
        healthManager.currentStamina = healthManager.maxStamina;
        healthManager.armor = 0; // Сбрасываем броню

        // Восстанавливаем время перезарядки способности
        playerController.mouseRoarCooldownTimer = 0f;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Выход из игры...");
        SceneManager.LoadScene("MainWindow");
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
        if (scene.buildIndex != 0) 
        {
            // При загрузке игровой сцены вызываем SpawnPlayer()
            gameOverMenuUI.SetActive(false);
            LevelManagers.instance.SpawnPlayer();
            UIController.Instance.ResetUI(); // Сброс UI
        }
    }

    private bool CheckpointExists(int index)
    {
        return index >= 0 && index < LevelManagers.instance.checkpointPositions.Length;
    }
}
