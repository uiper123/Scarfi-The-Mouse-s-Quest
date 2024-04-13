using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
public class FollowCamera : MonoBehaviour
{
    public string playerTag = "Player"; // Тег игрока
    private CinemachineVirtualCamera cinemachineCamera;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        FindAndFollowPlayer(); // Вызываем здесь для начала отслеживания игрока сразу после загрузки сцены
    }

    public void FindAndFollowPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null && cinemachineCamera != null)
        {
            cinemachineCamera.Follow = player.transform;
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindPlayerAfterDelay());
    }

    private IEnumerator FindPlayerAfterDelay(float delay = 0.1f)
    {
        yield return new WaitForSeconds(delay); // Небольшая задержка для гарантии загрузки персонажа
        FindAndFollowPlayer();
    }
}
