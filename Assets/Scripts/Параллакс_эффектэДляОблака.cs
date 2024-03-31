using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Параллакс_эффектэДляОблака : MonoBehaviour
{
    public float parallaxSpeed = 0.5f; // Скорость параллакса (настройте по своему вкусу)
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Рассчитываем смещение облака относительно начальной позиции
        float offsetX = Time.time * parallaxSpeed; // Изменение по времени
        Vector3 newPosition = new Vector3(initialPosition.x + offsetX, transform.position.y, transform.position.z);

        // Применяем смещение
        transform.position = newPosition;
    }
}
