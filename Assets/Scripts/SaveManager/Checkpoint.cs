using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    public GameObject dialogBox; // Ссылка на панель диалогового окна

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Показываем диалоговое окно
            dialogBox.SetActive(true);
            // Останавливаем игру (опционально)
            Time.timeScale = 0f; 
        }
    }

    // Вызывается при нажатии кнопки "Да"
    public void SaveAtCheckpoint()
    {
        LevelManagers.instance.SaveCheckpoint(checkpointIndex);
        dialogBox.SetActive(false);
        Time.timeScale = 1f; // Возобновляем игру
        // Добавьте визуальный или звуковой эффект сохранения
    }

    // Вызывается при нажатии кнопки "Нет"
    public void CancelCheckpoint()
    {
        dialogBox.SetActive(false);
        Time.timeScale = 1f; // Возобновляем игру
    }
}

