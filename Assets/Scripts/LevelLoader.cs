using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.SaveManager;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(int levelIndex, SaveData savedData = null)
    {
        StartCoroutine(LoadLevelAsync(levelIndex, savedData));
    }

    private IEnumerator LoadLevelAsync(int levelIndex, SaveData savedData)
    {
        // Определяем сцену для загрузки
        int levelToLoad;
        if (savedData != null && savedData.level != 0)
        {
            levelToLoad = savedData.level;
        }
        else
        {
            levelToLoad = levelIndex;
        }

        // Начинаем асинхронную загрузку уровня
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
        
        
        // Ждем завершения загрузки
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        
        // Если есть сохраненные данные, восстанавливаем состояние игры
        if (savedData != null)
        {
            // Получаем объект игрока и устанавливаем его позицию
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = savedData.playerPosition;
            }

            // Восстанавливаем здоровье, выносливость и броню игрока
            HelthManager healthManager = player.GetComponent<HelthManager>();
            if (healthManager != null)
            {
                //healthManager.currentHealth = savedData.currentHealth;
                healthManager.currentStamina = savedData.currentStamina;
                healthManager.armor = (int)savedData.armor;
            }

            // Восстанавливаем время перезарядки мышиного рыка
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.mouseRoarCooldownTimer = savedData.mouseRoarCooldown;
            }

            // Восстанавливаем текущий чекпоин
            LevelManagers.instance.SetCurrentCheckpoint(savedData.currentCheckpointIndex);
        }
        else
        {
            Debug.LogWarning("No saved game data found.");
        }
        
        asyncLoad.allowSceneActivation = true;
        GameMG.instance.gameOverMenuUI.SetActive(false);
        // Воспроизводим музыку для загруженной сцены
        AudioMG.instance.PlayMusicBySceneIndex(SceneManager.GetActiveScene().buildIndex);
    }
}