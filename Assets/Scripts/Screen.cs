using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private SymbolSlot symbolSlotPrefab;
    private List<SymbolSlot> symbolSlots = new();
    private List<Symbol> symbols = new();

    public void Initialize()
    {
        StartTask(GameManager.Instance.CurrentTask);
        GameManager.Instance.OnCurrentTaskChanged += StartTask;
        GameManager.Instance.OnCompleted += UpdateSlotsState;
        GameManager.Instance.OnDenied += UpdateSlotsState;
    }
    
    private void StartTask(int taskID)
    {
        symbols.Clear();
        symbolSlots.Clear();
        
        symbols = GameManager.Instance.CurrentTaskSymbols;

        for (int i = 0; i < symbols.Count; i++)
        {
            var instance = Instantiate(symbolSlotPrefab, contentTransform);
            instance.Initialize(symbols[i], i);
            symbolSlots.Add(instance);
        }
    }

    private void UpdateSlotsState(int index)
    {
        for (int i = 0; i < symbolSlots.Count; i++)
        {
            if (i == index) continue;
            
            symbolSlots[i].UpdateIndicator();
        }
    }
}