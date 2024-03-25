using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Параллакс_эффектэ : MonoBehaviour
{
    private Transform camTransform;
    private Vector3 lastCamPosition;
    private float textureUnitSizeX;
    
    [SerializeField] private Vector2 pralaxeffect;
    private void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPosition = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        

    }

    private void LateUpdate()
    {
        Vector3 deltaMovment = camTransform.position - lastCamPosition;
        transform.position += new Vector3(deltaMovment.x * pralaxeffect.x, deltaMovment.y * pralaxeffect.y, transform.position.z);
        lastCamPosition = camTransform.position;
        if (Mathf.Abs(camTransform.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPosiionX = (camTransform.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(camTransform.position.x + offsetPosiionX, transform.position.y);
        }

    }
    
    
}
