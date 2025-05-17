using System;
using System.Collections.Generic;
using UnityEngine;

public class ColorKeyBoard : MonoBehaviour
{
    [SerializeField] private List<ColorKey> keys = new();
    
    public event Action<Symbols> OnKeyPressed;
    
    public void Initialize()
    {
        InitializeKeys();
    }
    
    private void InitializeKeys()
    {
        foreach (var key in keys)
        {
            key.OnKeyPressed += InvokeOnKeyPressed;
        }
    }

    private void InvokeOnKeyPressed(Symbols symbol)
    {
        OnKeyPressed?.Invoke(symbol);
    }
}