using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Update = UnityEngine.PlayerLoop.Update;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private bool isFasingRight = true;
    private Vector2 InputMove;
    [SerializeField] private float JumpPower = 4f;
    [SerializeField] private float walckSpeed = 3f;
    [SerializeField] private float walckSpeedSpeed = 5f;
    private ToouchingDIrection tch;

    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving;}
        private set
        {
            _isMoving = value;
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
            anim.SetBool("run", value);
        }
    }

    private float CurrentSpeed
    {
        get
        {
            if (_isMoving)
            {
                return _isRunning ? walckSpeedSpeed : walckSpeed;
            }
            else return 0;
        }
    }

    [SerializeField] private Rigidbody2D bodyPlayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        bodyPlayer = GetComponent<Rigidbody2D>();
        tch = GetComponent<ToouchingDIrection>();
    }

    private void FixedUpdate()
    {
        if (InputMove.x > 0 || InputMove.x < 0)
        {
            bodyPlayer.velocity = new Vector2(InputMove.x * CurrentSpeed, bodyPlayer.velocity.y);
        }
        else             bodyPlayer.velocity = new Vector2(0, bodyPlayer.velocity.y);
        anim.SetFloat("yVelocity", bodyPlayer.velocity.y);
    }

    private void Update()
    {
        Flip();
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
       
        if (context.started && tch._isGround)
        {
            bodyPlayer.velocity = new Vector2(bodyPlayer.velocity.x, JumpPower);
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

}
