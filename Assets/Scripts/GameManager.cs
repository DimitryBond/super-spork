using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GameManager: DontDestroyOnLoadMonoSingleton<GameManager>
{
    [SerializeField] private SerializedDictionary<int, Task> tasks = new();
    [SerializeField] private KeyBoard keyboard;
    [SerializeField] private Screen screen;

    public List<Symbol> CurrentTaskSymbols { get; private set; } = new();

    public event Action<Symbol> OnCurrentSymbolChanged;
    public int CurrentTaskSymbolIndex { get; private set; }
    private Symbol currentSymbol;
    public Symbol CurrentSymbol
    {
        get => currentSymbol;
        private set
        {
            currentSymbol = value;
            OnCurrentSymbolChanged?.Invoke(value);
        }
    }
    
    public event Action<int> OnCurrentTaskChanged;
    private int currentTask;
    public int CurrentTask
    {
        get => currentTask;
        private set
        {
            currentTask = value;
            OnCurrentTaskChanged?.Invoke(value);
        }
    }

    private void Start()
    {
        StartTask();
        keyboard.Initialize();
        screen.Initialize();
        
        keyboard.OnKeyPressed += TryCompleteSymbol;
    }

    public void NextTask()
    {
        Instance.CurrentTask++;
    }

    public Task GetTask()
    {
        return tasks[Instance.CurrentTask];
    }

    public void StartTask()
    {
        CurrentTaskSymbols = tasks[CurrentTask].Symbols.ToList();
        CurrentSymbol = CurrentTaskSymbols[0];
        CurrentTaskSymbolIndex = 0;
    }

    public void CompleteTask()
    {
        
    }

    private void NextSymbol()
    {
        CurrentTaskSymbolIndex++;
        if (CurrentTaskSymbolIndex >= CurrentTaskSymbols.Count)
        {
            CompleteTask();
        }
        else
        {
            CurrentSymbol = CurrentTaskSymbols[CurrentTaskSymbolIndex];
        }
    }
    
    public event Action<int> OnCompleted;
    public event Action<int> OnDenied;
    private void TryCompleteSymbol(Symbols symbol)
    {
        if (CurrentSymbol.ThisSymbol == symbol)
        {
            NextSymbol();
            OnCompleted?.Invoke(CurrentTaskSymbolIndex);
            CurrentSymbol.Complete();
        }
        else
        {
            OnDenied?.Invoke(CurrentTaskSymbolIndex);
        }
    }
}