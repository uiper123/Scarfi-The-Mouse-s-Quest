using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField, Range(0f, 1f)] private float ParallaxStg = 0.1f;
    [SerializeField] private bool disableVerticalParallax;
    private Vector3 targetPrevPosit;

    private void Start()
    {
        
        if (!followTarget)
        {
            followTarget = Camera.main.transform;
            if (!followTarget)
            {
                Debug.LogError("The main camera is missing from the scene.");
                this.enabled = false; // Отключить скрипт, чтобы избежать дальнейших ошибок.
            }
        }

        targetPrevPosit = followTarget ? followTarget.position : Vector3.zero;
    }

    private void Update()
    {
        if (followTarget == null)
        {
            followTarget = FindObjectOfType<Camera>().transform;
            if (followTarget == null)
            {
                Debug.LogError("Не удалось найти объект для слежения.");
                return;
            }
            targetPrevPosit = followTarget.position;
        }

        var delta = followTarget.position - targetPrevPosit;
        if (disableVerticalParallax) delta.y = 0;
        targetPrevPosit = followTarget.position;
        transform.position += delta * ParallaxStg;
    }

}
