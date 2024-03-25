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
        }

        targetPrevPosit = followTarget.position;
    }

    private void Update()
    {
        var delta = followTarget.position - targetPrevPosit;
        if (disableVerticalParallax) delta.y = 0;
        targetPrevPosit = followTarget.position;
        transform.position += delta * ParallaxStg;
    }

}
