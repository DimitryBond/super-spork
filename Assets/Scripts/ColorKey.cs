using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorKey : MonoBehaviour
{
    [SerializeField] private Symbols symbol;
    
    [SerializeField] private Sprite buttonSprite;
    [SerializeField] private Sprite pressedButtonSprite;
    
    [SerializeField] private Image keyImage;
    [SerializeField] private Button button;
    
    public event Action<Symbols> OnKeyPressed;

    private void Start()
    {
        SetButtonSprites();
    }
    
    public void PressKey()
    { 
        OnKeyPressed?.Invoke(symbol);
    }

    private void SetButtonSprites()
    {
        keyImage.sprite = buttonSprite;
        
        var tempSpriteState = button.spriteState;
        tempSpriteState.pressedSprite = pressedButtonSprite;
        button.spriteState = tempSpriteState;
    }
}