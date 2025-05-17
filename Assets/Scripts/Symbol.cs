using System;
using UnityEngine;

[Serializable]
public class Symbol
{
    [SerializeField] private Symbols symbol;
    [SerializeField] private bool isHideSymbol;
    private bool isCompleted;
    private bool isDenied;

    public Symbols ThisSymbol => symbol;
    public bool IsHideSymbol => isHideSymbol;

    public event Action<bool> OnCompletedChanged;
    public bool IsCompleted
    {
        get => isCompleted;
        private set
        {
            if (value == isCompleted) return;
            isCompleted = value;
            OnCompletedChanged?.Invoke(value);
        }
    }
    
    public event Action<bool> OnDenaidChanged;
    public bool IsDenied
    {
        get => isDenied;
        private set
        {
            if (value == isDenied) return;
            isDenied = value;
            OnDenaidChanged?.Invoke(value);
        }
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Deny()
    {
        IsDenied = true;
    }
}