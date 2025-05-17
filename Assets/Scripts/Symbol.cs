using System;
using UnityEngine;

[Serializable]
public class Symbol
{
    [SerializeField] private Symbols symbol;
    [SerializeField] private bool isHideSymbol;
    private bool isCompleted;

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

    public void Complete()
    {
        IsCompleted = true;
    }
}