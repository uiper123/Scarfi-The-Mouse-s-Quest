using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerPink : MonoBehaviour
{
    public static LevelManagerPink instance;
    public GameObject playerPrefab; // Префаб персонажа
    public Transform spawnPoint; // Точка спавна


    public GameObject levelStart; // Место старта уровня
    public GameObject levelEnd; // Место завершения уровня
    public float levelBoundaryLeft; // Левая граница уровня
    public float levelBoundaryRight; // Правая граница уровня
    public float levelBoundaryTop; // Верхняя граница уровня
    public float levelBoundaryBottom; // Нижняя граница уровня

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        // Установка цвета Gizmos
        Gizmos.color = Color.red;

        // Рисование линий границ
        Gizmos.DrawLine(new Vector3(levelBoundaryLeft, levelBoundaryTop, 0), new Vector3(levelBoundaryRight, levelBoundaryTop, 0));
        Gizmos.DrawLine(new Vector3(levelBoundaryLeft, levelBoundaryBottom, 0), new Vector3(levelBoundaryRight, levelBoundaryBottom, 0));
        Gizmos.DrawLine(new Vector3(levelBoundaryLeft, levelBoundaryTop, 0), new Vector3(levelBoundaryLeft, levelBoundaryBottom, 0));
        Gizmos.DrawLine(new Vector3(levelBoundaryRight, levelBoundaryTop, 0), new Vector3(levelBoundaryRight, levelBoundaryBottom, 0));

        // Рисование меток начала и конца уровня
        if (levelStart)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(levelStart.transform.position, 0.5f);
        }

        if (levelEnd)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(levelEnd.transform.position, 0.5f);
        }
    }
    private void Start()
    {
        // Инициализация начальной позиции игрока
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpawnPlayer();
        if (player && levelStart)
        {
            player.transform.position = levelStart.transform.position;
        }
    }

    private void Update()
    {
        // Проверка границ уровня
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            if (player.transform.position.x < levelBoundaryLeft ||
                player.transform.position.x > levelBoundaryRight ||
                player.transform.position.y > levelBoundaryTop ||
                player.transform.position.y < levelBoundaryBottom)
            {
                // Игрок вышел за границы уровня
                PlayerDies();
            }
        }
    }

    public void SpawnPlayer()
    {
        if (playerPrefab && spawnPoint)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Необходимо установить playerPrefab и spawnPoint");
        }
    }
    public void PlayerDies()
    {
        // Логика смерти игрока
        Debug.Log("Игрок умер");
        // Здесь может быть перезагрузка уровня или показ экрана смерти
    }

    public void LevelCompleted()
    {
        // Логика завершения уровня
        Debug.Log("Уровень завершен");
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        int nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
        else
        {
            // Здесь может быть логика для завершения игры или перехода на экран победы
        }
    }
}
