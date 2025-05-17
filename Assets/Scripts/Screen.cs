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
        GameManager.Instance.OnTaskStarted += StartTask;
        GameManager.Instance.OnTaskFinished += FinishTask;
    }
    
    private void StartTask()
    {
        symbols = GameManager.Instance.CurrentTaskSymbols;

        for (int i = 0; i < symbols.Count; i++)
        {
            var instance = Instantiate(symbolSlotPrefab, contentTransform);
            instance.Initialize(symbols[i], i);
            symbolSlots.Add(instance);
        }
    }

    private void FinishTask()
    {
        symbols.Clear();
        foreach (var slot in symbolSlots)
        {
            Destroy(slot.gameObject);
        }
        symbolSlots.Clear();
    }
}