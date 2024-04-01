using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Параллакс_эффектэДляОблака : MonoBehaviour
{
    public float parallaxSpeed = 0.5f; // Скорость параллакса
    private Vector3 initialPosition;
    public float resetPositionOffset = 10f; // Расстояние, на котором облако будет перемещено назад

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Рассчитываем смещение облака относительно начальной позиции
        float offsetX = Time.time * parallaxSpeed;
        transform.position = new Vector3(initialPosition.x + offsetX, initialPosition.y, initialPosition.z);

        // Проверяем, достигло ли облако точки сброса
        if(transform.position.x >= initialPosition.x + resetPositionOffset)
        {
            // Перемещаем облако назад, чтобы создать непрерывное движение
            transform.position = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z);
        }
    }
}
