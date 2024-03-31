using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
     public int damage = 10; // Количество урона
    [SerializeField] private float damageInterval = 1f; // Интервал нанесения урона в секундах
    [SerializeField] private bool useTrigger = false; // Использовать триггер вместо коллайдера

    private Coroutine damageCoroutine; // Для отслеживания корутины

    private void StartDamage(PlayerController player)
    {
        player.TakeDamage(damage); // Наносим урон сразу при контакте
        player.TriggerHurtAnimation(); // Активируем анимацию получения урона
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // Останавливаем предыдущую корутину, если она уже запущена
        }
        damageCoroutine = StartCoroutine(DamageOverTime(player)); // Запускаем корутину для нанесения урона через интервалы
    }

    private void StopDamage()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine); // Останавливаем корутину
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger)
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            if (player != null)
            {
                StartDamage(player);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!useTrigger)
        {
            StopDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (useTrigger)
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            if (player != null)
            {
                StartDamage(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (useTrigger)
        {
            StopDamage();
        }
    }

    private IEnumerator DamageOverTime(PlayerController player)
    {
        yield return new WaitForSeconds(damageInterval); // Первая задержка перед началом цикла
        while (true) // Бесконечный цикл для нанесения урона каждую секунду
        {
            player.TakeDamage(damage); // Наносим урон
            player.TriggerHurtAnimation(); // Активируем анимацию получения урона
            yield return new WaitForSeconds(damageInterval); // Ожидаем 1 секунду
        }
    }
}
