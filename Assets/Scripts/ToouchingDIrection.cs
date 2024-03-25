using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToouchingDIrection : MonoBehaviour
{
    public ContactFilter2D CastingFilter2D;
    private CapsuleCollider2D col;
    [SerializeField] private float groundDistance = 0.05f;
    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    public bool _isGround;
    private Animator anim;
    public bool IsGround
    {
        get { return _isGround;}
        private  set
        {
            _isGround = value;
            anim.SetBool("IsGround", value);} }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGround =  col.Cast(Vector2.down, CastingFilter2D, groundHits, groundDistance) > 0;
    }

}
