using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titri : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 2f; // Базовая скорость
    [SerializeField] private float speedMultiplier = 3f; // Множитель скорости при нажатии
    [SerializeField] private float maxHeight = 5f; // Максимальная высота
    [SerializeField] private AudioSource audioSource; // Ссылка на AudioSource

    private bool hasReachedMaxHeight = false; // Флаг для отслеживания завершения движения

    private void Update()
    {
        // Определяем текущую скорость
        float currentSpeed = Input.GetMouseButton(0) ? baseSpeed * speedMultiplier : baseSpeed;

        // Двигаем объект вверх, если он не достиг максимальной высоты
        if (transform.position.y < maxHeight)
        {
            transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
        }
        else if (!hasReachedMaxHeight)
        {
            // Если объект достиг максимальной высоты
            hasReachedMaxHeight = true;
            ReturnToMenu();
        }

        // Устанавливаем pitch аудио в зависимости от скорости
        if (audioSource != null)
        {
            audioSource.pitch = Input.GetMouseButton(0) ? 2f : 1f;
        }
    }
    
    private IEnumerator ReturnToMenuWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждём указанное время
        MusicManager.Instance.SwitchToMainMenuMusic();
        SceneManager.LoadScene("MainMenu 1");
    }

    private void ReturnToMenu()
    {
        MusicManager.Instance.canSwitch = true;
        StartCoroutine(ReturnToMenuWithDelay(4f)); // Задержка 2 секунды
    }
}