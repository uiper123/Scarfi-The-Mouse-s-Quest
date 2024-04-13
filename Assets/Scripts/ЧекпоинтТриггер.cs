using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ЧекпоинтТриггер : MonoBehaviour
{
    // Индекс чекпоинта, который активируется при входе в триггер
    public int checkpointIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Сохраняем чекпоинт при достижении триггера
            LevelManagers.instance.SaveCheckpoint(checkpointIndex);
        }
    }
}
