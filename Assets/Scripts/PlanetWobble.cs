using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWobble : MonoBehaviour
{
    [SerializeField] private float shakeSpeed = 1f; // Скорость шатания
    [SerializeField] private float shakeAmount = 0.1f; // Амплитуда шатания (сила)
    [SerializeField] private bool useRandomOffset = true; // Использовать случайный сдвиг фазы

    private float randomOffset; // Случайный сдвиг фазы для уникальности
    private Vector3 initialPosition; // Начальная позиция объекта

    private void Start()
    {
        // Сохраняем начальную позицию объекта
        initialPosition = transform.position;

        // Генерируем случайный сдвиг фазы, если включено
        if (useRandomOffset)
        {
            randomOffset = Random.Range(0f, 100f); // Большая случайная величина для разнообразия
        }
    }

    private void Update()
    {
        // Вычисляем смещение на основе времени и случайного сдвига
        float xOffset = Mathf.Sin((Time.time + randomOffset) * shakeSpeed) * shakeAmount;

        // Применяем смещение к объекту
        transform.position = initialPosition + new Vector3(xOffset, 0f, 0f);
    }
}
