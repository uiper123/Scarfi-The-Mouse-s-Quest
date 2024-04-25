using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public TextMeshProUGUI scoreText;
    public int score = 0;
    public int difficultyMultiplier = 1; // Множитель сложности
    public UIController uiController;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount * difficultyMultiplier;
        scoreText.text = "Score: " + score;
    }

    public void SetDifficultyMultiplier(int multiplier)
    {
        difficultyMultiplier = multiplier;
    }
    
}
