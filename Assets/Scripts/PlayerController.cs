using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Update = UnityEngine.PlayerLoop.Update;


public class PlayerController : MonoBehaviour
{

    public Item mouseRoarItem;
    
    
    
    private Inventory inventory; 
    
    public static PlayerController instance = null;
    private Animator anim;
    private Vector3 currentPosition;
    private bool isFasingRight = true;
    private Vector2 InputMove;
    [SerializeField] private float JumpPower = 4f;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float walckSpeed = 5f;
    [SerializeField] private float walckSpeedSpeed = 10f;
    private ToouchingDIrection tch;

    [SerializeField] private float acceleration = 1.5f; // Ускорение
    [SerializeField] private float linearDrag = 4f; // Линейное замедление
    [SerializeField] private float deceleration = 0.5f; // Замедление

    private HelthManager hpManager;

    public float jumpAttackDamage = 10f;


    private float runSoundTimer;
    public float runSoundCooldown = 0.5f; // Время в секундах между воспроизведениями звука

    [SerializeField] private float staminaRecoveryRate = 5f; // Скорость восстановления стамины
    [SerializeField] private float staminaDecreasePerSecond = 10f; // Скорость уменьшения стамины в секунду


    [SerializeField] private GameObject playerOver;

    // Обращение к экземпляру UIController
    public UIController uiController;


    public float knockbackForce = 5f; // Сила отбрасывания
    public float knockbackDuration = 0.2f; // Длительность отбрасывания
    private bool isKnockedBack = false; // Флаг, указывающий на отбрасывание
    private float knockbackTimer = 0f; // Таймер отбрасывания


    private bool hasMouseRoarAbility = false; // Флаг, указывающий, есть ли у игрока способность мышиного рыка

    private PauseMenu PauseMn;
    private bool _isMoving = false;
    private bool _tryToRunWithoutStamina = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            runSoundCooldown = 0.47f;
            anim.SetBool("walk", value);

        }
    }

    private bool _isRunning = false;

    public bool IsRunning
    {
        get { return _isRunning; }
        private set
        {
            _isRunning = value;
            runSoundCooldown = 0.34f;

            // Проверяем, есть ли достаточно стамины для бега
            if (hpManager.currentStamina > 1 && value)
            {
                // Достаточно стамины, запускаем анимацию бега
                anim.SetBool("run", true);
                anim.SetBool("walk", false);
            }
            else
            {
                // Недостаточно стамины, запускаем анимацию ходьбы
                anim.SetBool("run", false);
                anim.SetBool("walk", true);
            }
        }
    }

    private float CurrentSpeed
    {
        get
        {
            if (_isMoving)
            {
                return _isRunning && hpManager.currentStamina > 1 ? walckSpeedSpeed : walckSpeed;
            }
            else
                return 0;
        }
    }

    [SerializeField] public Rigidbody2D bodyPlayer;

    private void Awake()
    {
        if (mouseRoarItem != null)
        {
            hasMouseRoarAbility = true;
        }
        else
        {
            hasMouseRoarAbility = false;
        }

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //UI
        uiController = UIController.Instance;
        //Pause
        // Получаем ссылку на компонент HelthManager
        hpManager = GetComponent<HelthManager>();


        anim = GetComponent<Animator>();
        bodyPlayer = GetComponent<Rigidbody2D>();
        tch = GetComponent<ToouchingDIrection>();

        bodyPlayer.drag = linearDrag; // Установка линейного замедления
        HelthManager.currentHealth = hpManager.maxHealth; // Инициализация здоровья
        hpManager.currentStamina = hpManager.maxStamina; // Инициализация стамины
        uiController.SetStamina(hpManager.currentStamina);
        uiController.SetHealth(HelthManager.currentHealth);

    }
    
    

    [SerializeField] public float mouseRoarCooldown = 25f; // Время перезарядки мышиного рыка в секундах
    public float mouseRoarCooldownTimer = 0f; // Таймер для отслеживания перезарядки мышиного рыка

    public void UseMouseRoar(InputAction.CallbackContext context)
    {
        if (mouseRoarItem != null && context.started)
        {
            hasMouseRoarAbility = true;
            // Проверяем, есть ли возможность использовать мышиной рык в данный момент
            if (mouseRoarCooldownTimer <= 0)
            {
                // Ability is available, use it
                MouseRoar();

                // Обновляем таймер перезарядки мышиного рыка
                mouseRoarCooldownTimer = mouseRoarCooldown;
                //uiController.UpdateMouseRoarPanel();
            }

        }


    }

    private void FixedUpdate()
    {
        float targetSpeed = InputMove.x * CurrentSpeed;
        if (InputMove.x != 0)
        {
            bodyPlayer.velocity =
                new Vector2(Mathf.MoveTowards(bodyPlayer.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime),
                    bodyPlayer.velocity.y);

            // Проверяем, есть ли достаточно стамины для бега
            if (hpManager.currentStamina > 1 && _isRunning)
            {
                // Достаточно стамины, устанавливаем анимацию бега
                anim.SetBool("run", true);
                anim.SetBool("walk", false);

                // Обновляем runSoundCooldown в зависимости от текущей скорости
                if (Mathf.Abs(bodyPlayer.velocity.x) > 0.1f)
                {
                    runSoundCooldown = 0.34f;
                }
                else
                {
                    runSoundCooldown = 0.47f;
                }
            }
            else
            {
                // Недостаточно стамины, устанавливаем анимацию ходьбы
                anim.SetBool("run", false);
                anim.SetBool("walk", true);

                // Обновляем runSoundCooldown в зависимости от текущей скорости
                if (Mathf.Abs(bodyPlayer.velocity.x) > 0.1f)
                {
                    runSoundCooldown = 0.47f;
                }
                else
                {
                    runSoundCooldown = 0.5f;
                }
            }
        }
        else
        {
            // Плавное замедление до стояния
            bodyPlayer.velocity =
                new Vector2(Mathf.MoveTowards(bodyPlayer.velocity.x, 0, deceleration * Time.fixedDeltaTime),
                    bodyPlayer.velocity.y);

            // Обновляем runSoundCooldown, когда персонаж не двигается
            runSoundCooldown = 0.5f;
        }

        anim.SetFloat("yVelocity", bodyPlayer.velocity.y);

    }

    

    private void Update()
    {
        // Обновляем таймер перезарядки мышиного рыка
        if (mouseRoarCooldownTimer > 0)
        {
            mouseRoarCooldownTimer -= Time.deltaTime;
        }

        Flip();

        
        if (transform.position.y < LevelManagers.instance.levelBoundsMin.y)
        {
            // Если персонаж вышел за нижнюю границу уровня, вызываем метод GameOver у GameMG
            GameMG.instance.GameOver();
        }

        if (tch._isGround && bodyPlayer.velocity.y <= 0)
        {
            jumpCount = 0;
        }

        if (IsMoving && runSoundTimer <= 0 && tch.IsGround)
        {
            AudioMG.instance.PlayPlayerSfx(0);
            runSoundTimer = runSoundCooldown; // Сброс таймера
        }

        // Обновление таймера
        if (runSoundTimer > 0)
        {
            runSoundTimer -= Time.deltaTime;
        }

        if (_isRunning)
        {
            hpManager.currentStamina -= staminaDecreasePerSecond * Time.deltaTime;
            hpManager.currentStamina = Mathf.Clamp(hpManager.currentStamina, 0, hpManager.maxStamina);
        }
        else
        {
            // Восстановление стамины
            hpManager.currentStamina += staminaRecoveryRate * Time.deltaTime;
            hpManager.currentStamina = Mathf.Clamp(hpManager.currentStamina, 0, hpManager.maxStamina);
        }

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
        }

        // Обновление UI стамины
        currentPosition = transform.position;
        uiController.SetStamina(hpManager.currentStamina);
        uiController.SetHealth(HelthManager.currentHealth);
        uiController.UpdateArmor();
        RecoverStamina();

    }


    public void Knockback(Vector2 direction)
    {
        isKnockedBack = true;
        knockbackTimer = knockbackDuration;
        bodyPlayer.velocity = direction * knockbackForce;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
            Vector2 knockbackDirection = (transform.position - collision.gameObject.transform.position).normalized;
            Knockback(knockbackDirection);
        }
    }

    public void SetPlayerPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
        IsMoving = InputMove != Vector2.zero;
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (context.started && Mathf.Abs(InputMove.x) > 0.1f)
        {
            // Проверяем, есть ли достаточно стамины для бега
            if (hpManager.currentStamina > 1)
            {
                IsRunning = true;
            }
            else
            {
                // Если стамины недостаточно, устанавливаем IsRunning в false, чтобы активировалась анимация ходьбы
                IsRunning = false;
                // Установите флаг, чтобы отследить, что игрок пытается бежать без стамины
                _tryToRunWithoutStamina = true;
            }
        }
        else if (context.canceled)
        {
            IsRunning = false;
            // Сбросьте флаг, чтобы отследить, что игрок больше не пытается бежать без стамины
            _tryToRunWithoutStamina = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {

        if (context.started)
        {
            if (tch._isGround || jumpCount < maxJumpCount)
            {
                jumpCount++;
                AudioMG.instance.PlayPlayerSfx(1);
                bodyPlayer.velocity = new Vector2(bodyPlayer.velocity.x, JumpPower);

                // Проверяем, есть ли враг под игроком
                RaycastHit2D hit =
                    Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Enemies"));
                if (hit.collider != null)
                {
                    // Наносим урон врагу при прыжке сверху

                    hit.collider.GetComponent<EnemyScript>().TakeDamage(jumpAttackDamage);
                }
            }
        }

    }

    private void Flip()
    {
        if ((isFasingRight && InputMove.x < 0f) || (!isFasingRight && InputMove.x > 0f))
        {
            isFasingRight = !isFasingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }

    public float mouseRoarRange = 5f;
    public float mouseRoarDamage = 30f;

    public void MouseRoar()
    {
        // Проверяем, доступен ли мышиный рык
        if (hasMouseRoarAbility && Time.time >= mouseRoarCooldownTimer)
        {
            // Запускаем анимацию мышиного рыка
            anim.SetTrigger("RoatGun");
            AudioMG.instance.PlayPlayerSfx(3);
            // Проверяем, есть ли враги в пределах досягаемости
            Collider2D[] enemies =
                Physics2D.OverlapCircleAll(transform.position, mouseRoarRange, LayerMask.GetMask("Enemies"));
            foreach (Collider2D enemy in enemies)
            {
                // Наносим урон всем найденным врагам
                enemy.GetComponent<EnemyScript>().TakeDamage(mouseRoarDamage);
            }

            hasMouseRoarAbility = false;
            // Устанавливаем время следующего использования мышиного рыка
            mouseRoarCooldownTimer = mouseRoarCooldown;
        }

    }


    private void RecoverStamina()
    {
        if (hpManager.currentStamina < hpManager.maxStamina)
        {
            hpManager.currentStamina += staminaRecoveryRate * Time.deltaTime;
            hpManager.currentStamina = Mathf.Clamp(hpManager.currentStamina, 0, hpManager.maxStamina);
        }
    }

    public void TakeDamage(int damage)
    {
        AudioMG.instance.PlayPlayerSfx(2);
        anim.SetTrigger("hurt");

        // Рассчитываем фактический урон с учетом брони
        int damageReduction = Mathf.RoundToInt(hpManager.armor * damage / hpManager.maxArmor);
        int actualDamage = Mathf.Max(damage - damageReduction, 1); // Гарантируем, что урон будет не менее 1

        HelthManager.currentHealth -= actualDamage;

        if (HelthManager.currentHealth <= 0 && instance != null)
        {
            Die(); // Вызов метода смерти при нулевом здоровье
        }
        else
        {
            // Если здоровье не равно 0, обновляем UI
            uiController.SetHealth(HelthManager.currentHealth);
            uiController.UpdateArmor(); // Обновляем UI брони
        }

        // Отбрасываем игрока назад
        StartCoroutine(DisableMovementTemporarily(0.2f));
    }

    // Корутина для временного отключения управления
    private IEnumerator DisableMovementTemporarily(float duration)
    {
        // Отключаем управление
        GetComponent<PlayerInput>().enabled = false;

        // Ждем заданное время
        yield return new WaitForSeconds(duration);

        // Включаем управление
        GetComponent<PlayerInput>().enabled = true;
    }

    private void Die()
    {
        uiController.SetHealth(0);
        AudioMG.instance.PlayPlayerSfx(4);
        GameMG.instance.GameOver();
       // PauseMn.menuButton.SetActive(false);

        //anim.SetTrigger("die"); // Активация анимации смерти
    }

    public void TriggerHurtAnimation()
    {
        anim.SetTrigger("hurt"); // Активация анимации получения урона
    }

    public void UpdateHealth(float newHealth)
    {
        HelthManager.SetCurrentHealth(newHealth);
        uiController.SetHealth(HelthManager.currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MouseRoar")) // Замените "MouseRoarPickup" на ваш тег
        {
            // Игрок коснулся предмета

            mouseRoarItem = other.GetComponent<Pickup>().item; // Получите ссылку на компонент предмета
            hasMouseRoarAbility = true; // Установите флаг, указывающий, что у игрока есть способность
            AudioMG.instance.PlaySfx(0);
            uiController.UpdateMouseRoarPanel(); // Обновить UI 
            Destroy(other.gameObject); // Удалите объект подбора из сцены
            ScoreManager.instance.AddScore(1000); // Добавляем очки за подбор предмета
            // Добавьте здесь любую дополнительную логику, например, воспроизведение звука или обновление UI
        }


        if (other.CompareTag("Key"))
        {
            Item item = other.GetComponent<Pickup>().item;
            InventoryManager.instance.AddItemToInventory(item);  // Pass the tag for identification
            Destroy(other.gameObject);
            ScoreManager.instance.AddScore(5000);
        }
        

    }
    
    private void LevelComplete()
    {
        // Добавьте здесь код для завершения уровня, например:
        Debug.Log("Уровень пройден!"); 
        SceneManager.LoadScene("EndGameScene"); // Замените "EndGameScene" на имя вашей сцены завершения игры
        // ... Можно загрузить следующую сцену, показать сообщение о победе и т.д. ... 
    }
}
