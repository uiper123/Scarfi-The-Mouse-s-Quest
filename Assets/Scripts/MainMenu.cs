using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad = "MainLevel";
    public GameObject loadingScreen;
    

    private void Start()
    {
        AudioMG.instance.PlayMusic(AudioMG.instance.sceneMusicClips[0]);
    }

    public void PlayGame()
    {
        // Загрузка основного уровня и показ загрузочного экрана
        //loadingScreen.SetActive(true);
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        // Выход из игры
        Debug.Log("Выход из игры...");
        Application.Quit();
    }

    public void ContinueGame()
    {
        // Загрузка сохраненной игры
        // Здесь должен быть ваш код для загрузки сохраненной игры
    }

}
