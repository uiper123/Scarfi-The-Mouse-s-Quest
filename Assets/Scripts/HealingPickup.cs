using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20; // Количество здоровья, которое восстанавливает предмет
    public float respawnTime = 10f; // Время, через которое предмет респавнится (в секундах)

    private void Start()
    {
        // Устанавливаем начальное время респауна
        Invoke("RespawnPickup", respawnTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Восстановление здоровья игрока
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                AudioMG.instance.PlaySfx(2);
                ScoreManager.instance.AddScore(50); // Добавляем очки за подбор предмета
                playerController.UpdateHealth(HelthManager.currentHealth + healAmount);
            }

            // Удаление предмета из сцены
            gameObject.SetActive(false);

            // Устанавливаем время респауна
            Invoke("RespawnPickup", respawnTime);
        }
    }

    private void RespawnPickup()
    {
        // Активируем предмет снова
        gameObject.SetActive(true);
    }
}