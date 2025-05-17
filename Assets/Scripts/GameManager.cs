using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class GameManager: DontDestroyOnLoadMonoSingleton<GameManager>
{
    [SerializeField] private SerializedDictionary<int, Task> tasks = new();
    [SerializeField] private KeyBoard keyboard;
    [SerializeField] private ColorKeyBoard colorKeyBoard;
    [SerializeField] private Screen screen;

    public List<Symbol> CurrentTaskSymbols { get; private set; } = new();
    private bool canInput;

    public event Action<int> OnCurrentSymbolIndexChanged; 
    private int currentSymbolIndex;
    public int CurrentSymbolIndex
    { 
        get => currentSymbolIndex;
        private set
        {
            currentSymbolIndex = value;
            OnCurrentSymbolIndexChanged?.Invoke(value);
        }
    }
    
    public event Action<Symbol> OnCurrentSymbolChanged;
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
        colorKeyBoard.Initialize();
        keyboard.Initialize();
        screen.Initialize();
        StartTask();
        canInput = true;

        colorKeyBoard.OnKeyPressed += TryCompleteSymbol;
        keyboard.OnKeyPressed += TryCompleteSymbol;
    }

    public Task GetTask()
    {
        return tasks[CurrentTask];
    }

    public event Action OnTaskStarted; 
    public void StartTask()
    {
        CurrentTaskSymbols.Clear();
        CurrentTaskSymbols = tasks[CurrentTask].Symbols.ToList();
        CurrentSymbol = CurrentTaskSymbols[0];
        CurrentSymbolIndex = 0;
        OnTaskStarted?.Invoke();
    }

    public void CompleteTask()
    {
        CurrentTask++;
        if (tasks.ContainsKey(CurrentTask))
        {
            StartTask();
        }
        else
        {
            //Разблокировать огонь
        }
    }

    private void NextSymbol()
    {
        CurrentSymbolIndex++;
        if (CurrentSymbolIndex >= CurrentTaskSymbols.Count)
        {
            CompleteTask();
        }
        else
        {
            CurrentSymbol = CurrentTaskSymbols[CurrentSymbolIndex];
        }
    }
    
    public event Action<int> OnCompleted;
    public event Action<int> OnDenied;
    private void TryCompleteSymbol(Symbols symbol)
    {
        if (!canInput) return;
        
        if (CurrentSymbol.ThisSymbol == symbol)
        {
            CurrentSymbol.Complete();
            OnCompleted?.Invoke(CurrentSymbolIndex);
            NextSymbol();
        }
        else
        {
            CurrentSymbol.Deny();
            OnDenied?.Invoke(CurrentSymbolIndex);
            canInput = false;
        }
    }
}