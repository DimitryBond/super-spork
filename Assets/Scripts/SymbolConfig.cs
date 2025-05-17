using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SymbolConfig", menuName = "SymbolConfig")]
public class SymbolConfig : ScriptableObject
{
    public SerializedDictionary<Symbols, Sprite> Symbols;

    public Sprite GetSprite(Symbols symbol)
    {
        return Symbols[symbol];
    }
}