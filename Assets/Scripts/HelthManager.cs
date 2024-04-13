using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelthManager : MonoBehaviour
{
    
    
    public int maxHealth = 100; // Максимальное количество жизней
    public static float currentHealth; // Текущее количество жизней
    
    
    public float maxStamina = 100f; // Максимальное количество стамины
    public float currentStamina; // Текущее количество стамины

    public int maxArmor = 100; // Максимальное количество брони
    public int armor = 0; // Текущее количество брони
}
