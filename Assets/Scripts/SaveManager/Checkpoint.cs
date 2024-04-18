using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;
    public GameObject dialogBox;
    public GameObject checkpointIndicator;
    public Animator checkpointIndicatorAnimator;

    public void ShowCheckpointIndicator()
    {
        checkpointIndicator.SetActive(true);
        checkpointIndicatorAnimator.SetTrigger("Show");
    }

    public void HideCheckpointIndicator()
    {
        checkpointIndicatorAnimator.SetTrigger("Hide");
        checkpointIndicator.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Показываем диалоговое окно
            dialogBox.SetActive(true);
            // Активируем табличку
            ShowCheckpointIndicator();
            // Останавливаем игру (опционально)
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Скрываем диалоговое окно
            // Деактивируем табличку
            HideCheckpointIndicator();
            SaveAtCheckpoint();
            // Возобновляем игру
            Time.timeScale = 1f;
        }
    }


    public void SaveAtCheckpoint()
    {
        LevelManagers.instance.SaveCheckpoint(checkpointIndex);
        dialogBox.SetActive(false);
        Time.timeScale = 1f; // Возобновляем игру
        HideCheckpointIndicator(); // Скрываем табличку
        Debug.Log("Save");
    
        // Вызываем метод из UIController, чтобы показать сообщение о сохранении
    }
    
    
    
    public void CancelCheckpoint()
    {
        dialogBox.SetActive(false);
        Time.timeScale = 1f; // Возобновляем игру
    }

}

