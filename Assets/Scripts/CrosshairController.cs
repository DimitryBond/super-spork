using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshair; // Ссылка на объект прицела
    public float moveSpeed = 5f; // Скорость перемещения прицела

    private Vector3 targetPosition; // Целевая позиция прицела

    void Start()
    {
        // Инициализация начальной позиции прицела
        targetPosition = crosshair.transform.position;
    }

    void Update()
    {
        // Плавное перемещение прицела к целевой позиции
        crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void MoveUp()
    {
        targetPosition += Vector3.up;
        ClampPosition();
    }

    public void MoveDown()
    {
        targetPosition += Vector3.down;
        ClampPosition();
    }

    public void MoveLeft()
    {
        targetPosition += Vector3.left;
        ClampPosition();
    }

    public void MoveRight()
    {
        targetPosition += Vector3.right;
        ClampPosition();
    }

    private void ClampPosition()
    {
        // Ограничение по оси X и Y (установите свои значения)
        targetPosition.x = Mathf.Clamp(targetPosition.x, -8f, 8f); // Границы по X
        targetPosition.y = Mathf.Clamp(targetPosition.y, -4.5f, 4.5f); // Границы по Y
    }
    
    // Метод для проверки планеты под прицелом
    public void Fire()
    {
        // Создаем луч из позиции прицела
        Vector2 rayDirection = Vector2.zero; // Направление луча (ноль, так как мы проверяем только точку)
        RaycastHit2D hit = Physics2D.Raycast(crosshair.transform.position, rayDirection);

        if (hit.collider != null)
        {
            // Проверяем, попал ли луч в объект с компонентом PlanetInfo
            PlanetInfo planetInfo = hit.collider.GetComponent<PlanetInfo>();
            if (planetInfo != null)
            {
                Debug.Log("Выбранная планета: " + planetInfo.GetPlanetName());
            }
            else
            {
                Debug.Log("Инфон планеты отсутствует");
            }
        }
        else
        {
            Debug.Log("Луч не попал ни в один объект.");
        }
    }
}
