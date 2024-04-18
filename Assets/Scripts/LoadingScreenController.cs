using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public static LoadingScreenController Instance;
    public GameObject loadingScreenPanel;
    public Slider loadingProgressSlider;
    public Text loadingProgressText;
    
    public void ShowLoadingScreen()
    {
        loadingScreenPanel.SetActive(true);
        // Можно запустить анимацию или визуальные эффекты, чтобы привлечь внимание игрока
    }
    
    public void UpdateLoadingProgress(float progress)
    {
        loadingProgressSlider.value = progress;
        loadingProgressText.text = Mathf.RoundToInt(progress * 100) + "%";
    }
    
    public void HideLoadingScreen()
    {
        loadingScreenPanel.SetActive(false);
    }
    
}
