using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector2 playerPosition;
    public int level;
    public int maxHealth;
    // Другие переменные состояния игры
    public float currentHealth;
    public float currentStamina;
    public int armor;
}
