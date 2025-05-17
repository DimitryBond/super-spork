using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using DefaultNamespace;
using UnityEngine;

public class GameManager : DontDestroyOnLoadMonoSingleton<GameManager>
{
    [SerializeField] private SerializedDictionary<int, Task> tasks = new();
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private KeyBoard keyboard;
    [SerializeField] private ColorKeyBoard colorKeyBoard;
    [SerializeField] private Screen screen;
    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private HintSystem hintSystem;
    [SerializeField] private TargetPlanetData targetPlanetData;
    [SerializeField] private DialogueData dialogueDatabase;

    private int currentRound = 0;
    private PlanetInfo currentTargetPlanet;
    private List<PlanetInfo> allPlanets = new();

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

        crosshairController.OnTruePlanetDestroy += OnPlanetShot; // заменили StartTask на OnPlanetShot
        colorKeyBoard.OnKeyPressed += TryCompleteSymbol;
        keyboard.OnKeyPressed += TryCompleteSymbol;

        dialogSystem.ShowDialogue(dialogueDatabase.GetDialogue("task_0"));
        IsTaskActive = true;
        
        SetupTargetPlanet(); // задаём первую цель
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
    private void SetupTargetPlanet()
    {
        allPlanets = new List<PlanetInfo>(FindObjectsOfType<PlanetInfo>());

        foreach (var planet in allPlanets)
        {
            planet.IsTarget = false;
        }

        string targetName = targetPlanetData.GetTargetPlanetName(currentRound);
        currentTargetPlanet = allPlanets.Find(p => p.planetName == targetName);

        if (currentTargetPlanet != null)
        {
            currentTargetPlanet.IsTarget = true;
            Debug.Log($"Раунд {currentRound + 1}: Цель — {currentTargetPlanet.planetName}");
        }
        else
        {
            Debug.LogError($"Не найдена планета с именем: {targetName}");
        }
    }
    
    private void OnPlanetShot(PlanetInfo hitPlanet)
    {
        if (hitPlanet == null)
        {
            Debug.Log("Промах — объект не планета.");
            return;
        }

        if (hitPlanet == currentTargetPlanet)
        {
            Debug.Log($"Попал в нужную планету: {hitPlanet.planetName}");

            currentRound++;
            if (currentRound < targetPlanetData.TotalRounds)
            {
                SetupTargetPlanet();
                dialogSystem.ShowDialogue(dialogueDatabase.GetDialogue($"task_{currentRound}"));

            }
            else
            {
                Debug.Log("Игра завершена! Все раунды пройдены.");
                TriggerEndingDialogue("ending_success");
            }
        }
        else
        {
            Debug.Log($"Ошибка. Попал в {hitPlanet.planetName}, а нужно было {currentTargetPlanet.planetName}");
            TriggerEndingDialogue("ending_fail");
        }
    }
}
