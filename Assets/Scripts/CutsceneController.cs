using Scripts.SaveManager;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public PlayableDirector director;
    public int firstLevelIndex = 2;
    private bool sceneLoaded = false;

    private void Start()
    {
        // Останавливаем воспроизведение фоновой музыки
        AudioMG.instance.StopMusic();
        // Показываем экран загрузки перед воспроизведением катсцены
        
        // Запускаем Таймлайн после небольшой задержки (0.5 секунды)
        Invoke("PlayCutscene", 0.5f);
    }

    void PlayCutscene()
    {
        // Воспроизводим Таймлайн
        director.Play();
        HelthManager healthManager = PlayerController.instance.GetComponent<HelthManager>();
        if (healthManager != null)
        {
            HelthManager.currentHealth = healthManager.maxHealth;
        }

        // Скрываем экран загрузки
       
    }

    private void Update()
    {
        // Check if the Timeline has stopped playing and the scene hasn't been loaded yet
        if (director.state == PlayState.Paused && !sceneLoaded)
        {
            // Load the first level scene
            LevelLoader.Instance.LoadLevel(firstLevelIndex);
            sceneLoaded = true; // Set the flag to prevent further loading
        }
    }

}