using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartKey : MonoBehaviour
{
    [SerializeField] private Button button;
    
    public event Action OnKeyPressed;
    
    public void PressKey()
    {
        OnKeyPressed?.Invoke();
    }
}