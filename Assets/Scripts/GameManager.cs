using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using UnityEngine;

public class GameManager: DontDestroyOnLoadMonoSingleton<GameManager>
{
    [SerializeField] private SerializedDictionary<int, Task> tasks = new();
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private KeyBoard keyboard;
    [SerializeField] private ColorKeyBoard colorKeyBoard;
    [SerializeField] private Screen screen;
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private HintSystem hintSystem;

    [SerializeField] private DialogueData dialogueDatabase;
    
    public event Action<bool> OnIsTaskActiveChanged;
    private bool isTaskActive;
    public bool IsTaskActive
    {
        get => isTaskActive;
        set
        {
            if (value == isTaskActive) return;
            isTaskActive = value;
            OnIsTaskActiveChanged?.Invoke(value);
        }
    }


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
        crosshairController.Initialize();
        colorKeyBoard.Initialize();
        keyboard.Initialize();
        screen.Initialize();
        canInput = true;

        crosshairController.OnTruePlanetDestroy += StartTask;
        colorKeyBoard.OnKeyPressed += TryCompleteSymbol;
        keyboard.OnKeyPressed += TryCompleteSymbol;
        dialogSystem.ShowDialogue(dialogueDatabase.GetDialogue("task_0"));
        IsTaskActive = true;
    }
    
    public event Action OnTaskStarted; 
    public void StartTask()
    {
        CurrentTaskSymbols.Clear();
        CurrentTaskSymbols = tasks[CurrentTask].Symbols.ToList();
        CurrentSymbol = CurrentTaskSymbols[0];
        CurrentSymbolIndex = 0;
        OnTaskStarted?.Invoke();
        var hints = dialogueDatabase.GetHints($"task_{CurrentTask}");
        hintSystem.ShowHints(hints);
    }

    public event Action OnTaskFinished; 
    public void CompleteTask()
    {
        CurrentTask++;
        hintSystem.HideHints();
        IsTaskActive = false;
        OnTaskFinished?.Invoke();
    }

    public void NextDialogue()
    {
        if (IsTaskActive) return;

        string id = $"task_{CurrentTask}";
        var dialogue = dialogueDatabase.GetDialogue(id);
        dialogSystem.ShowDialogue(dialogue);
    }
    
    public void TriggerEndingDialogue(string endingId)
    {
        if (dialogueDatabase == null) return;

        var dialogue = dialogueDatabase.GetDialogue(endingId);
        dialogSystem.ShowDialogue(dialogue);
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