using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using Update = UnityEngine.PlayerLoop.Update;


public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private bool isFasingRight = true;
    private Vector2 InputMove;
    [SerializeField] private float JumpPower = 4f;
    private int jumpCount = 0;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float walckSpeed = 5f;
    [SerializeField] private float walckSpeedSpeed = 10f;
    private ToouchingDIrection tch;
    
    [SerializeField] private float acceleration = 1.5f; // Ускорение
    [SerializeField] private float linearDrag = 4f;     // Линейное замедление
    [SerializeField] private float deceleration = 0.5f; // Замедление

    
    public int maxHealth = 100; // Максимальное количество жизней
    public int currentHealth; // Текущее количество жизней
    
    
    public float maxStamina = 100f; // Максимальное количество стамины
    public float currentStamina; // Текущее количество стамины
    [SerializeField] private float staminaRecoveryRate = 5f; // Скорость восстановления стамины
    [SerializeField] private float staminaDecreasePerSecond = 10f; // Скорость уменьшения стамины в секунду
    
    public AudioClip jumpSound;
    public AudioClip runSound;
    public AudioClip damageSound;
    public AudioClip youdie;
    private AudioSource audioSource;
    private float runSoundTimer;
    public float runSoundCooldown = 0.5f; // Время в секундах между воспроизведениями звука
    
    
    [SerializeField] private GameObject playerOver;
    public GameOverPanel1 GameOverPanel = GameOverPanel1.Instance;
        // Обращение к экземпляру UIController
    public UIController uiController = UIController.Instance;
    
    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving;}
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
        get { return _isRunning;}
        private set
        { 
            _isRunning = value;
            runSoundCooldown = 0.34f;
            anim.SetBool("run", currentStamina > 1 && value);
        }
    }

    private float CurrentSpeed
    {
        get
        {
            if (_isMoving)
            {
                return _isRunning && currentStamina > 1 ? walckSpeedSpeed : walckSpeed;
            }
            else
                return 0;
        }
    }

    [SerializeField] private Rigidbody2D bodyPlayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bodyPlayer = GetComponent<Rigidbody2D>();
        tch = GetComponent<ToouchingDIrection>();
        audioSource = GetComponent<AudioSource>();
        bodyPlayer.drag = linearDrag; // Установка линейного замедления
        currentHealth = maxHealth; // Инициализация здоровья
        currentStamina = maxStamina; // Инициализация стамины
        uiController.SetStamina(currentStamina);
        uiController.SetHealth(currentHealth);


    }

    private void FixedUpdate()
    {
        float targetSpeed = InputMove.x * CurrentSpeed;
        if (InputMove.x != 0)
        {
            bodyPlayer.velocity = new Vector2(Mathf.MoveTowards(bodyPlayer.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime), bodyPlayer.velocity.y);
        }
        else
        {
            // Плавное замедление до стояния
            bodyPlayer.velocity = new Vector2(Mathf.MoveTowards(bodyPlayer.velocity.x, 0, deceleration * Time.fixedDeltaTime), bodyPlayer.velocity.y);
        }
        anim.SetFloat("yVelocity", bodyPlayer.velocity.y);
    }
    

    private void Update()
    {
        Flip();
        if (tch._isGround && bodyPlayer.velocity.y <= 0)
        {
            jumpCount = 0;
        }
        
        if (IsMoving && runSoundTimer <= 0 && tch.IsGround)
        {
            audioSource.PlayOneShot(runSound);
            runSoundTimer = runSoundCooldown; // Сброс таймера
        }

        // Обновление таймера
        if (runSoundTimer > 0)
        {
            runSoundTimer -= Time.deltaTime;
        }
        
        if (_isRunning)
        {
            currentStamina -= staminaDecreasePerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
        else
        {
            // Восстановление стамины
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        // Обновление UI стамины
        uiController.SetStamina(currentStamina);
        uiController.SetHealth(currentHealth);
        RecoverStamina();

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputMove = context.ReadValue<Vector2>();
        IsMoving = InputMove != Vector2.zero;
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

   public void OnJump(InputAction.CallbackContext context)
   {
       
       if (context.started)
       {
           if (tch._isGround || jumpCount < maxJumpCount)
           {
               jumpCount++;
               audioSource.PlayOneShot(jumpSound);
               bodyPlayer.velocity = new Vector2(bodyPlayer.velocity.x, JumpPower);
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
        audioSource.PlayOneShot(damageSound);
        currentHealth -= damage;
        // Активация анимации получения урона
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GameOverPanel.ShowGameOverMenu();
        playerOver.SetActive(false);
        //anim.SetTrigger("die"); // Активация анимации смерти
    }
    public void TriggerHurtAnimation()
    {
        anim.SetTrigger("hurt"); // Активация анимации получения урона
    }


}

