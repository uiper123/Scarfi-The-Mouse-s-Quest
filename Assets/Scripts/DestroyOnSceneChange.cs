using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnSceneChange : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем текущую сцену
        if (scene.name != "YourGameScene") // Замените "YourGameScene" на название вашей игровой сцены
        {
            StartCoroutine(DestroyCharacter());
        }
    }

    private IEnumerator DestroyCharacter()
    {
        // Ждем один кадр
        yield return null;
        
        // Уничтожаем объект персонажа
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

