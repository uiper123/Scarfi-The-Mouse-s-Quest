using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataResetManager : MonoBehaviour
{
    public static SaveDataResetManager Instance { get; private set; }

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

    public void ResetAllSaveData()
    {
        // Удаляем все сохраненные данные из PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("All save data has been reset.");
    }

    // Дополнительные методы для управления сохранениями, если необходимо
    public bool IsSaveDataAvailable()
    {
        return PlayerPrefs.HasKey("PlayerPosition");
    }

    public void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
