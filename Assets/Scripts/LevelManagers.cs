using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagers : MonoBehaviour
{
     public static LevelManagers instance = null;

    public GameObject playerPrefab;
    public Transform startMarker;
    public Transform[] checkpointPositions;
    public Transform endMarker;
    public Vector2 levelBoundsMin;
    public Vector2 levelBoundsMax;

    private int currentCheckpointIndex = 0;
    private GameObject currentPlayerInstance; // Для хранения текущего объекта персонажа

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Проверяем, находимся ли мы в главном меню
        if (SceneManager.GetActiveScene().name != "MainWindow" && currentPlayerInstance == null)
        {
            SpawnPlayer();
        }
    
    }

    public void SpawnPlayer()
    {
        // Проверяем текущую сцену, чтобы избежать спавна персонажа в основном меню
        if (SceneManager.GetActiveScene().name != "MainWindow")
        {
            Vector3 spawnPosition;
            if (CheckpointExists(currentCheckpointIndex))
            {
                spawnPosition = checkpointPositions[currentCheckpointIndex].position;
            }
            else if (startMarker != null) // Добавьте проверку на null
            {
                spawnPosition = startMarker.position;
            }
            else
            {
                // Если startMarker равен null, выполните какие-то действия по умолчанию
                Debug.LogError("startMarker is null. Please assign a valid Transform in the Inspector.");
                return;
            }

            // Уничтожаем предыдущий объект персонажа, если он существует
            if (currentPlayerInstance != null)
            {
                Destroy(currentPlayerInstance);
            }

            if (playerPrefab != null)
            {
                // Создаем новый объект персонажа и сохраняем его в currentPlayerInstance
                currentPlayerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                Debug.LogError("Player prefab is not assigned. Please assign it in the Inspector.");
            }

            SaveData data = SaveSystem.LoadProgress();
            if (data != null)
            {
                // Загружаем уровень, если он был сохранен
                if (data.level != SceneManager.GetActiveScene().buildIndex)
                {
                    SceneManager.LoadScene(data.level);
                    return; // Выходим из метода, чтобы избежать повторного спавна игрока
                }

                // Устанавливаем позицию игрока
                currentPlayerInstance.transform.position = data.playerPosition;
                // Устанавливаем остальные данные игрока (здоровье, выносливость, броню и т.д.)
                //currentPlayerInstance.GetComponent<HelthManager>().currentHealth = data.currentHealth;
                currentPlayerInstance.GetComponent<HelthManager>().currentStamina = data.currentStamina;
                currentPlayerInstance.GetComponent<HelthManager>().armor = (int)data.armor;
            }
        }
    }

    bool CheckpointExists(int index)
    {
        return index >= 0 && index < checkpointPositions.Length;
    }
    public void LoadCheckpoint()
    {
        if (PlayerController.instance != null)
        {
            // Устанавливаем позицию игрока на последний чекпоинт
            PlayerController.instance.SetPlayerPosition(checkpointPositions[currentCheckpointIndex].position);

            // Восстанавливаем данные игрока из сохраненного чекпоина
            SaveData data = SaveSystem.LoadProgress();
            if (data != null)
            {
                // Устанавливаем здоровье, выносливость и броню игрока
                //currentPlayerInstance.GetComponent<HelthManager>().currentHealth = data.currentHealth;
                currentPlayerInstance.GetComponent<HelthManager>().currentStamina = data.currentStamina;
                currentPlayerInstance.GetComponent<HelthManager>().armor = (int)data.armor;

                // Восстанавливаем время перезарядки мышиного рыка
                PlayerController.instance.mouseRoarCooldownTimer = data.mouseRoarCooldown;
            }
        }
    }
    public void SaveCheckpoint(int index)
    {
        SaveData data = new SaveData();
        data.playerPosition = currentPlayerInstance.transform.position;
        data.level = SceneManager.GetActiveScene().buildIndex;

        HelthManager healthManager = currentPlayerInstance.GetComponent<HelthManager>();
        data.maxHealth = healthManager.maxHealth;
        //data.currentHealth = healthManager.currentHealth;
        data.currentStamina = healthManager.currentStamina;
        //data.armor = healthManager.armor;

        data.currentCheckpointIndex = index;

        // Сохраняем время перезарядки мышиного рыка
        data.mouseRoarCooldown = PlayerController.instance.mouseRoarCooldownTimer;

        SaveSystem.SaveProgress(data);
    }
    public void SetCurrentCheckpoint(int index)
    {
        currentCheckpointIndex = index;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.position == endMarker.position)
        {
            // Если персонаж достиг маркера окончания уровня
            Debug.Log("Level completed!");
            // Здесь вы можете выполнить какие-то действия, например, загрузить следующий уровень
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        SpawnPlayer();
    }

    void OnDrawGizmosSelected()
    {
        // Рисуем границы уровня
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMin.y, 0), new Vector3(levelBoundsMax.x, levelBoundsMin.y, 0));
        Gizmos.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMin.y, 0), new Vector3(levelBoundsMax.x, levelBoundsMax.y, 0));
        Gizmos.DrawLine(new Vector3(levelBoundsMax.x, levelBoundsMax.y, 0), new Vector3(levelBoundsMin.x, levelBoundsMax.y, 0));
        Gizmos.DrawLine(new Vector3(levelBoundsMin.x, levelBoundsMax.y, 0), new Vector3(levelBoundsMin.x, levelBoundsMin.y, 0));
        
        // Отмечаем начальную позицию и маркер окончания уровня
        if (startMarker != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startMarker.position, 0.5f);
        }
        if (endMarker != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(endMarker.position, 0.5f);
        }
        
        // Рисуем позиции чекпоинтов
        Gizmos.color = Color.yellow;
        foreach (Transform checkpoint in checkpointPositions)
        {
            Gizmos.DrawWireSphere(checkpoint.position, 0.5f);
        }
    }
}
