using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    
    public Slider staminaSlider;
    public Slider healthSlider;

    // Предполагается, что у вас есть доступ к скрипту персонажа, который содержит максимальные значения здоровья и стамины
    public PlayerController player;

    public static UIController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Защита от уничтожения
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликаты
        }
    }
    private void Start()
    {
        // Синхронизация максимальных значений слайдеров со значениями игрока
        
        staminaSlider.maxValue = player.maxStamina;

        // Установка текущих значений слайдеров
      
        staminaSlider.value = player.currentStamina;

        healthSlider.maxValue = player.maxHealth;
        healthSlider.value = player.currentHealth;
    }

    // Методы для обновления слайдеров, которые будут вызываться из скрипта персонажа

    public void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }

    public void SetHealth(float health)
    {
        healthSlider.value = health;
    }
 
}
