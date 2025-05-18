using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : DontDestroyOnLoadMonoSingleton<GameManager>
{
    [SerializeField] private SerializedDictionary<int, Task> tasks = new();
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private KeyBoard keyboard;
    [SerializeField] private ColorKeyBoard colorKeyBoard;
    [SerializeField] private Screen screen;
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private HintSystem hintSystem;
    [SerializeField] private DialogueData dialogueDatabase;
    [SerializeField] private GameObject EnemyShip;
    
    private int currentRound = 0;
    private PlanetInfo currentTargetPlanet;
    public PlanetInfo CurrentTargetPlanet => currentTargetPlanet;
    private List<PlanetInfo> allPlanets = new();
    
    public event Action<int> OnHealthChanged;
    private int health;
    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnHealthChanged?.Invoke(value);
        }
    }
    
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
        Health = 3;
        crosshairController.Initialize();
        colorKeyBoard.Initialize();
        keyboard.Initialize();
        screen.Initialize();
        canInput = true;

        dialogSystem.OnDialogFinished += StartTask;
        crosshairController.OnPlanetSelected += OnPlanetShot; // заменили StartTask на OnPlanetShot
        colorKeyBoard.OnKeyPressed += TryCompleteSymbol;
        keyboard.OnKeyPressed += TryCompleteSymbol;
        keyboard.OnRestartKeyPressed += RestartStringTask;
        
        var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
        dialogSystem.ShowDialogue(messages);
    }

    public event Action OnStringTaskRestarted;
    private void RestartStringTask()
    {
        CurrentTaskSymbols.Clear();
        CurrentTaskSymbols = tasks[CurrentTask].Symbols.ToList();
        CurrentSymbol = CurrentTaskSymbols[0];
        CurrentSymbolIndex = 0;
        OnStringTaskRestarted?.Invoke();
    }

    public event Action OnTaskStarted;
    public void StartTask()
    {
        SetupTargetPlanet();
        CurrentTaskSymbols.Clear();
        CurrentTaskSymbols = tasks[CurrentTask].Symbols.ToList();
        CurrentSymbol = CurrentTaskSymbols[0];
        CurrentSymbolIndex = 0;
        OnTaskStarted?.Invoke();
        
        var hints = dialogueDatabase.GetHints($"hint_{CurrentTask}");
        hintSystem.ShowHints(hints);
        IsTaskActive = true;
    }

    public event Action OnTaskFinished;
    public void CompleteTask(bool planetWasDestroy)
    {
        if (planetWasDestroy)
        {
            var dialog = dialogueDatabase.GetDialogue($"{currentTargetPlanet.PlanetName}_Destroy");
            dialogSystem.ShowDialogue(dialog);
            
            if (CurrentTask == 1)
            {
                var messages = dialog.Concat(dialogueDatabase.GetDialogue("TaskWithShift").Concat(dialogueDatabase.GetDialogue("Task"))).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 2)
            {
                var messages = dialog.Concat(dialogueDatabase.GetDialogue("TaskWithColor").Concat(dialogueDatabase.GetDialogue("TaskWithHints")).Concat(dialogueDatabase.GetDialogue("Task"))) .ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 3)
            {
                var messages = dialog.Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 4)
            {
                var messages = dialog.Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
                EnemyShip.SetActive(true);
            }
            //конец игры
            else if (CurrentTask == 5)
            {
            }
        }
        else
        {
            var id = $"Health={health}";
            var dialog = dialogueDatabase.GetDialogue(id);
            dialogSystem.ShowDialogue(dialog);
            
            if (CurrentTask == 1)
            {
                var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 2)
            {
                var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 3)
            {
                var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            else if (CurrentTask == 4)
            {
                var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                dialogSystem.ShowDialogue(messages);
            }
            //конец игры
            else if (CurrentTask == 5)
            {
                var messages = dialogueDatabase.GetDialogue("Start").Concat(dialogueDatabase.GetDialogue("Task")).ToArray();
                EnemyShip.SetActive(true);
                dialogSystem.ShowDialogue(messages);
            }
        }
        
        CurrentTask++;
        hintSystem.HideHints();
        IsTaskActive = false;
        OnTaskFinished?.Invoke();
    }

    public void TriggerEndingDialogue(string endingId)
    {
        if (dialogueDatabase == null) return;

        var dialogue = dialogueDatabase.GetDialogue(endingId);
        dialogSystem.ShowDialogue(dialogue);
    }

    public event Action OnStringTaskFinished;
    private void NextSymbol()
    {
        CurrentSymbolIndex++;
        if (CurrentSymbolIndex >= CurrentTaskSymbols.Count)
        {
            OnStringTaskFinished?.Invoke();
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
    private void SetupTargetPlanet()
    {
        allPlanets = new List<PlanetInfo>(FindObjectsOfType<PlanetInfo>());

        foreach (var planet in allPlanets)
        {
            planet.IsTarget = false;
        }

        var randomIndex = Random.Range(0, allPlanets.Count);
        currentTargetPlanet = allPlanets[randomIndex];
        currentTargetPlanet.IsTarget = true;
    }
    
    private void OnPlanetShot(PlanetInfo hitPlanet)
    {
        //Не попал
        if (hitPlanet == null)
        {
            Health--;
            CompleteTask(false);
            return;
        }

        //Попал куда надо
        if (hitPlanet == currentTargetPlanet)
        {
            CompleteTask(true);
        }
        //Попал куда НЕ надо
        else
        {
            Health--;
            CompleteTask(false);
            //TriggerEndingDialogue("ending_fail");
        }
    }
}
