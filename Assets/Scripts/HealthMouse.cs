using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMouse : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100; // Максимальное количество жизней
    private int currentHealth; // Текущее количество жизней
    [SerializeField] private float maxStamina = 100f; // Максимальное количество стамины
    private float currentStamina; // Текущее количество стамины
    [SerializeField] private float staminaRecoveryRate = 5f; // Скорость восстановления стамины
    private Animator anim;

    private void Awake()
    {
        // ... (остальной код)
        currentHealth = maxHealth; // Инициализация здоровья
        currentStamina = maxStamina; // Инициализация стамины
    }

    private void Update()
    {
        // ... (остальной код)
        RecoverStamina(); // Восстановление стамины
    }

    private void RecoverStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        anim.SetTrigger("hurt"); // Активация анимации получения урона
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Код для смерти персонажа
        anim.SetTrigger("die"); // Активация анимации смерти
        // Отключение управления персонажем или другие действия при смерти
    }

    // ... (остальной код)
}
