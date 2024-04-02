using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPink : MonoBehaviour
{
    void Start()
    {
        SaveData data = SaveSystem.LoadProgress();
        if (data != null)
        {
            // Переместите игрока на последнюю сохраненную позицию
            // и восстановите другие сохраненные параметры
        }
    }
}
