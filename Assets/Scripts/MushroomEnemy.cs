using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : MonoBehaviour
{
   public float maxHealth = 100f;
    public float currentHealth;
    public float attackRange = 2f;
    public float explosionRange = 3f;
    public int explosionDamage = 50;
    public float knockbackForce = 10f;
    public float explosionDelay = 1f;
    public float moveSpeed = 3f;
    public float delayBeforeActivation = 5f; // Время, через которое враг активируется после прохождения игрока
    public float deactivationDelay = 3f; // Время, за которое враг деактивируется, если игрок покинет его зону видимости
    private GameObject playerGameObject;
    private PlayerController playerController;
    public Animator animator;

    private bool isExploding = false;
    private bool isPlayerInRange = false;
    private bool isActive = false;
    private bool isTransparent = true;
    
    private Collider2D enemyCollider;
    private Rigidbody2D enemyRigidbody;
    private float nextActivationTime = 0f;
    public float activationCooldown = 3f; // Время между активациями гриба
    private bool isPlayerInside = false;
    private bool wasPlayerInRangeBeforeActivation = false;

    private void Start()
    {
        currentHealth = maxHealth;
        FindPlayer();
        animator.SetBool("IsActive", false); // Изначально враг неактивен
      
        enemyCollider = GetComponent<Collider2D>();
        enemyCollider.enabled = true; // Включаем коллайдер, чтобы игрок не мог пройти сквозь него
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (playerGameObject != null)
        {
            if (isActive)
            {
                CheckPlayerDistance();
            }
            else
            {
                CheckPlayerProximity();
            }
        }
    }

    private void FindPlayer()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObject != null)
        {
            playerController = playerGameObject.GetComponent<PlayerController>();
        }
        else
        {
            Invoke(nameof(FindPlayer), 0.1f); // Повторная проверка через 0.1 секунды
        }
    }

    private void CheckPlayerProximity()
    {
        if (playerGameObject != null)
        {
            float distance = Vector2.Distance(transform.position, playerGameObject.transform.position);
            if (distance <= attackRange && Time.time >= nextActivationTime)
            {
                Activate();
                nextActivationTime = Time.time + activationCooldown; // Устанавливаем время следующей активации
            }
        }
    }

    private void Activate()
    {
        isActive = true;
        animator.SetBool("IsActive", true);
        MakeVisible();
        StartCoroutine(ActivateEnemy());
    }

    private IEnumerator ActivateEnemy()
    {
        animator.SetTrigger("Rise"); // Запускаем анимацию вставания гриба
        yield return new WaitForSeconds(delayBeforeActivation);
        MoveToPlayer();
    }

    private void MakeVisible()
    {
        if (isTransparent)
        {
            isTransparent = false;
            // Здесь можно добавить код, который сделает гриба видимым
        }
    }

    private void CheckPlayerDistance()
    {
        if (playerGameObject != null)
        {
            float distance = Vector2.Distance(transform.position, playerGameObject.transform.position);
            if (distance <= attackRange)
            {
                isPlayerInRange = true;
                MoveToPlayer();
            }
            else
            {
                isPlayerInRange = false;
            }
        }
    }

    private void MoveToPlayer()
    {
        // Определение направления для поворота врага
        if (playerGameObject.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Перемещение врага к игроку
        transform.position = Vector2.MoveTowards(transform.position, playerGameObject.transform.position, moveSpeed * Time.deltaTime);

        // Проверка расстояния до игрока, если меньше взрывного радиуса, запускаем взрыв
        float distance = Vector2.Distance(transform.position, playerGameObject.transform.position);
        if (distance <= explosionRange)
        {
            StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        if (!isExploding)
        {
            isExploding = true;
            animator.SetTrigger("Explode");

            yield return new WaitForSeconds(explosionDelay);

            // Нанесение урона игроку
            if (playerController != null)
            {
                playerController.TakeDamage(explosionDamage);
                playerController.Knockback((playerController.transform.position - transform.position).normalized * knockbackForce);
            }

            // Уничтожение врага
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (!isActive)
            {
                Activate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;
            StartCoroutine(DeactivateIfPlayerStaysAway());
        }
    }

    private IEnumerator DeactivateIfPlayerStaysAway()
    {
        float timer = 0f;
        while (!isPlayerInside && timer < deactivationDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (!isPlayerInside)
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        isActive = false;
        animator.SetBool("IsActive", false);
        isExploding = false;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
