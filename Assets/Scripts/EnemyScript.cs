using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{

    private Rigidbody2D bodyenemy;
    public float health = 100f;
    public GameObject player;
    public Animator animator;
    public float moveSpeed = 2f;
    private Vector2 targetPosition;
    private bool isMoving = true;
    public Vector2 leftBoundaryPosition;
    public Vector2 rightBoundaryPosition;
    private float targetReachedTime;
    public float minTimeToReachTarget = 2f;
    public float maxTimeToReachTarget = 5f;
    public float detectionRange = 5f; // Дистанция, при которой враг начинает преследовать игрока
    public float attackRange = 1f; // Радиус зоны атаки
    public int attackDamage = 20; // Урон от атаки
    public float attackRate = 1f; // Скорость атаки (атак в секунду)
    private float nextAttackTime = 0f; // Время, когда будет доступна следующая атака
    private LayerMask groundLayer; // Слой земли
    private bool isPlayerDetected = false;
    
    public AudioSource enemyAudioSource;
    public AudioClip enemyWalkSfx;
    public AudioClip enemyAttackSfx;
    public AudioClip enemyHurtSfx;
    public AudioClip enemyDeathSfx;
    public float runSoundTimerEnemy = 0.5f;
    
    
    public float respawnTime = 20f; // Время респауна
    
    public float knockbackForce = 5f; // Сила отбрасывания
    public float knockbackDuration = 0.2f; // Длительность отбрасывания
    private bool isKnockedBack = false; // Флаг, указывающий на отбрасывание
    private float knockbackTimer = 0f; // Таймер отбрасывания
    
    private void OnDrawGizmos()
    {
        // Рисуем линию для левой границы
        Gizmos.DrawLine(new Vector2(leftBoundaryPosition.x, leftBoundaryPosition.y - 5f),
            new Vector2(leftBoundaryPosition.x, leftBoundaryPosition.y + 5f));
        // Рисуем линию для правой границы
        Gizmos.DrawLine(new Vector2(rightBoundaryPosition.x, rightBoundaryPosition.y - 5f),
            new Vector2(rightBoundaryPosition.x, rightBoundaryPosition.y + 5f));

        // Рисуем круг для зоны обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Вычисляем направление отталкивания на основе положения игрока относительно врага
            Vector3 knockbackDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<PlayerController>().Knockback(new Vector2(knockbackDirection.x, knockbackDirection.y));

        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float scaleX = transform.localScale.x;
        Gizmos.DrawRay(transform.position, new Vector3(scaleX * attackRange, 0f, 0f));
    }

    private void Start()
    {
        ScoreManager.instance.AddScore(0); // Инициализация отображения очков
        bodyenemy = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Platforms"); // Получаем слой земли
        SetRandomTargetPosition();
    
        // Инициализация AudioSource для врага
        enemyAudioSource = gameObject.AddComponent<AudioSource>();
        enemyAudioSource.outputAudioMixerGroup = AudioMG.instance.enemyMixerGroup; // Используйте группу микшера для врагов
    }
    private bool isAtTargetPosition = false;

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            isPlayerDetected = true;

            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                MoveToPlayer();
            }
        }
        else
        {
            isPlayerDetected = false;

            // Остановка звука врага, если игрок вне поля зрения
            enemyAudioSource.Stop();

            // Проверяем, достиг ли враг целевой позиции
            if (isAtTargetPosition)
            {
                // Враг достиг целевой позиции, вызываем SetRandomTargetPosition()
                SetRandomTargetPosition();
                isAtTargetPosition = false;
            }
            else
            {
                // Враг еще не достиг целевой позиции, продолжаем движение
                MoveToRandomPosition();

                // Проверяем, достиг ли враг целевой позиции
                if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
                {
                    isAtTargetPosition = true;
                }
            }
        }
        
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
        }
        CheckHealth();
    }
    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            groundLayer = LayerMask.GetMask("Platforms");
            SetRandomTargetPosition();
        }
        else
        {
            Invoke(nameof(FindPlayer), 0.1f);
        }
    }
    private void MoveToPlayer()
    {
        // Определение направления для поворота врага
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Проверяем, есть ли земля под ногами врага
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        if (hit.collider != null)
        {
            // Перемещение врага только по оси X
            float step = moveSpeed * Time.deltaTime;
            float newX = Mathf.MoveTowards(transform.position.x, player.transform.position.x, step);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    private void MoveToRandomPosition()
    {
        // Определение направления для поворота врага
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Проверяем, есть ли земля под ногами врага
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        if (hit.collider != null)
        {
            // Перемещение врага только по оси X
            float step = moveSpeed * Time.deltaTime;
            float newX = Mathf.MoveTowards(transform.position.x, targetPosition.x, step);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);

            // Воспроизведение звука ходьбы врага
            if (!enemyAudioSource.isPlaying || enemyAudioSource.clip != enemyWalkSfx && runSoundTimerEnemy <= 0)
            {
                enemyAudioSource.clip = enemyWalkSfx;
                enemyAudioSource.Play();
            }

            // Проверяем, достиг ли враг целевой позиции
            if (Mathf.Abs(transform.position.x - targetPosition.x) < 0.1f)
            {
                SetRandomTargetPosition();
            }
        }
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = new Vector2(Random.Range(leftBoundaryPosition.x, rightBoundaryPosition.x),
            transform.position.y);
        isMoving = true;
        animator.SetBool("_isMovingEnem", true);
    }

    public void Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        bodyenemy.velocity = direction * knockbackForce; // Заменяем bodyPlayer на соответствующий Rigidbody2D
    }
    
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Воспроизведение звука получения урона врагом
            if (!enemyAudioSource.isPlaying || enemyAudioSource.clip != enemyHurtSfx)
            {
                enemyAudioSource.clip = enemyHurtSfx;
                enemyAudioSource.Play();
            }
        }
    }

    private void Die()
    {
        if (!enemyAudioSource.isPlaying || enemyAudioSource.clip != enemyDeathSfx)
        {
            ScoreManager.instance.AddScore(100); // Добавляем очки за убийство врага
            enemyAudioSource.clip = enemyDeathSfx;
            enemyAudioSource.Play();
        }
    
        gameObject.SetActive(false);
        Invoke("Respawn", respawnTime);
    }
    private void Respawn()
    {
        gameObject.SetActive(true);
        float maxHealth = 100;
        health = maxHealth; // Восстанавливаем здоровье
        // ... Добавьте здесь любой другой код для сброса состояния врага ...
    }
    private void CheckHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            // Запускаем анимацию атаки
            animator.SetTrigger("_isattackEnem");
            
            if (!enemyAudioSource.isPlaying || enemyAudioSource.clip != enemyAttackSfx)
            {
                enemyAudioSource.clip = enemyAttackSfx;
                enemyAudioSource.Play();
            }
            // Проверяем, есть ли игрок в пределах досягаемости
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, 0f) * attackRange, attackRange, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                // Игрок найден, наносим урон
                hit.collider.GetComponent<PlayerController>().TakeDamage(attackDamage);
                // Игрок найден, наносим урон и отталкиваем
                PlayerController playerController = hit.collider.GetComponent<PlayerController>();
                playerController.TakeDamage(attackDamage);
                playerController.Knockback((playerController.transform.position - transform.position).normalized);

                // Отбрасываем и самого врага
                Knockback((transform.position - playerController.transform.position).normalized);
            }
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
}