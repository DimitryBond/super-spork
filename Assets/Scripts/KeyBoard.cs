using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private ShiftKey shiftKey;
    [SerializeField] private List<Key> keys = new();
    
    public event Action<Symbols> OnKeyPressed;

    public event Action<bool> OnShiftIsActiveChanged;
    private bool shiftIsActive;
    public bool ShiftIsActive
    {
        get => shiftIsActive;
        private set
        {
            if (value == shiftIsActive) return;
            shiftIsActive = value;
            OnShiftIsActiveChanged?.Invoke(shiftIsActive);
        }
    }
    
    public void Initialize()
    {
        InitializeKeys();
    }
    
    private void InitializeKeys()
    {
        foreach (var key in keys)
        {
            key.Initialize(this);
            key.OnKeyPressed += InvokeOnKeyPressed;
        }
        
        shiftKey.OnKeyPressed += ToggleShift;
    }

    private void ToggleShift()
    {
        ShiftIsActive = !ShiftIsActive;
    }

    private void InvokeOnKeyPressed(Symbols symbol)
    {
        OnKeyPressed?.Invoke(symbol);
    }
}