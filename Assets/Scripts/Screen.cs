using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private SymbolSlot symbolSlotPrefab;
    [SerializeField] private TextMeshProUGUI planetDescription;
    private List<SymbolSlot> symbolSlots = new();
    private List<Symbol> symbols = new();

    public void Initialize()
    {
        GameManager.Instance.OnTaskStarted += StartTask;
        GameManager.Instance.OnStringTaskFinished += FinishTask;
    }
    
    private void StartTask()
    {
        planetDescription.gameObject.SetActive(false);
        
        
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
        planetDescription.gameObject.SetActive(true);
        planetDescription.text = $"{GameManager.Instance.CurrentTargetPlanet.PlanetTargetDescription}";
        
        
        symbols.Clear();
        foreach (var slot in symbolSlots)
        {
            Destroy(slot.gameObject);
        }
        symbolSlots.Clear();
    }
}