using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public int level;
    public string levelstr; // Или int level, если вы используете индексы уровней
    public float maxHealth;
    public int currentHealth;
    public float currentStamina;
    public float armor;
    public int currentCheckpointIndex;
    public float mouseRoarCooldown;
    
}
