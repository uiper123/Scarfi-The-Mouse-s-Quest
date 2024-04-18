using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void LoadLevel(string levelName, SaveData savedData = null)
    {
        StartCoroutine(LoadLevelAsync(levelName, savedData));
    }

    private IEnumerator LoadLevelAsync(string levelName, SaveData savedData)
    {
        // Загружаем сцену с загрузочным экраном
        SceneManager.LoadSceneAsync("LoadingScreen");

        // Ждем один кадр, чтобы загрузочный экран успел отобразиться
        yield return null;

        // Начинаем асинхронную загрузку уровня
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName);

        // Скрываем сцену, пока она загружается
        asyncLoad.allowSceneActivation = false;

        // Цикл обновления прогресса загрузки
        while (!asyncLoad.isDone)
        {
            // Обновляем слайдер прогресса в загрузочном экране
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            LoadingScreenController.Instance.UpdateLoadingProgress(progress);

            // Когда загрузка почти завершена, активируем сцену
            if (progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // Если есть сохраненные данные, восстанавливаем состояние игры
        if (savedData != null)
        {
            // Загружаем уровень, на котором было последнее сохранение
            SceneManager.LoadScene(savedData.level);

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
    }
}
