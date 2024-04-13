using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject mouseRoarPanel;
    public Text mouseRoarCooldownText;
    private PlayerController playerController;

    public Slider staminaSlider;
    public Slider healthSlider;
    public Slider armorSlider;
    public HelthManager hpManager;

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
        playerController = PlayerController.instance;
        
        
        AudioMG.instance.PlayMusic(AudioMG.instance.sceneMusicClips[1]);
        
        staminaSlider.maxValue = hpManager.maxStamina;
        staminaSlider.value = hpManager.currentStamina;

        healthSlider.maxValue = hpManager.maxHealth;
        healthSlider.value = HelthManager.currentHealth;
        // Синхронизация максимальных значений слайдеров со значениями игрока
        armorSlider.maxValue = hpManager.maxArmor;
        armorSlider.value = hpManager.armor;
        
        UpdateMouseRoarPanel();
        UpdateArmor();
    }

    private void Update()
    {
        UpdateMouseRoarPanel();
        UpdateArmor();
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
    private void UpdateArmor()
    {
        armorSlider.value = hpManager.armor;
    }
    public void UpdateMouseRoarPanel()
    {
        if (playerController.mouseRoarItem != null)
        {
            // Мышиный рык доступен в инвентаре
            mouseRoarPanel.SetActive(true);

            // Получаем оставшееся время перезарядки из скрипта PlayerController
            float remainingCooldown = playerController.mouseRoarCooldownTimer;

            // Проверяем, активен ли перезарядочный таймер
            if (remainingCooldown <= 0)
            {
                // Перезарядка завершена, показываем изображение и скрываем текст
                mouseRoarCooldownText.gameObject.SetActive(false);
                mouseRoarPanel.GetComponent<Image>().color = Color.white;
            }
            else
            {
                // Перезарядка активна, показываем текст и обновляем отсчет
                mouseRoarCooldownText.gameObject.SetActive(true);
                mouseRoarCooldownText.text = Mathf.CeilToInt(remainingCooldown).ToString();
                mouseRoarPanel.GetComponent<Image>().color = Color.red;
            }
        }
        else
        {
            // Мышиный рык недоступен в инвентаре
            mouseRoarPanel.SetActive(false);
        }
    }
    }
