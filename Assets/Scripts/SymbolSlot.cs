using UnityEngine;
using UnityEngine.UI;

public class SymbolSlot: MonoBehaviour
{   
    public Symbol SymbolInSlot { get; private set; }
    public int SlotIndex { get; private set; }

    [SerializeField] private SymbolConfig symbolConfig;
    [SerializeField] private Image symbolImage;
    [SerializeField] private CompleteIndicator completeIndicator;
    [SerializeField] private Sprite hideSymbol;
    
    public void Initialize(Symbol symbolModel, int index)
    {
        SymbolInSlot = symbolModel;
        SlotIndex = index;
        completeIndicator.Initialize(this);

        if (SymbolInSlot.IsHideSymbol)
        {
            symbolImage.sprite = hideSymbol;
        }
        else
        {
            symbolImage.sprite = symbolConfig.GetSprite(SymbolInSlot.ThisSymbol);
        }
    }
}