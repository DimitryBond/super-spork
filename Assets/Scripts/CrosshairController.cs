using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private Crosshair crosshair;
    [SerializeField] private List<Button> buttons;
    private bool canControl;

    public void Initialize()
    {
        LockedControl();
        GameManager.Instance.OnTaskFinished += UnlockedControl;
        GameManager.Instance.OnTaskStarted += LockedControl;
    }

    public void LockedControl()
    {
        canControl = false;
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    private void UnlockedControl()
    {
        canControl = true;
        foreach (var button in buttons)
        {
            button.interactable = true;
        }
    }

    public void MoveUp()
    {
        if (!canControl) return;
        crosshair.MoveUp();
    }

    public void MoveDown()
    {
        if (!canControl) return;
        crosshair.MoveDown();
    }

    public void MoveLeft()
    {
        if (!canControl) return;
        crosshair.MoveLeft();
    }

    public void MoveRight()
    {
        if (!canControl) return;
        crosshair.MoveRight();
    }
    
    public event Action<PlanetInfo> OnTruePlanetDestroy;

    public void Fire()
    {
        if (!canControl) return;
        
        var rayDirection = Vector2.zero; 
        var hit = Physics2D.Raycast(crosshair.transform.position, rayDirection);

        if (hit.collider != null)
        {
            // Проверяем, попал ли луч в объект с компонентом PlanetInfo
            var planetInfo = hit.collider.GetComponent<PlanetInfo>();
            if (planetInfo != null)
            {
                Debug.Log("Выбранная планета: " + planetInfo.GetPlanetName());
                OnTruePlanetDestroy?.Invoke(planetInfo); 
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