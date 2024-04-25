using System.Collections;
using System.Collections.Generic;
using MoreMountains.CorgiEngine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveAtCheckpoint();
            // Останавливаем игру (опционально)
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveAtCheckpoint();
            // Возобновляем игру
            Time.timeScale = 1f;
        }
    }


    public void SaveAtCheckpoint()
    {
        LevelManagers.instance.SaveCheckpoint(checkpointIndex);
        
        Time.timeScale = 1f; // Возобновляем игру
        
        Debug.Log("Save");
    
        // Вызываем метод из UIController, чтобы показать сообщение о сохранении
    }
    

}

