using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField] private Symbols firstSymbol;
    [SerializeField] private Symbols secondSymbol;
    
    [SerializeField] private Sprite firstButtonSprite;
    [SerializeField] private Sprite firstPressedButtonSprite;
    [SerializeField] private Sprite secondButtonSprite;
    [SerializeField] private Sprite secondPressedButtonSprite;
    
    [SerializeField] private Image keyImage;
    [SerializeField] private Button button;
    
    private KeyBoard keyBoard;
    public event Action<Symbols> OnKeyPressed; 
    private bool shiftIsActive;

    public void Initialize(KeyBoard keyBoardIns)
    {
        keyBoard = keyBoardIns;

        keyBoard.OnShiftIsActiveChanged += SwitchShift;
    }
    
    public void PressKey()
    {
        if (shiftIsActive)
        {
            OnKeyPressed?.Invoke(secondSymbol);
        }
        else
        {
            OnKeyPressed?.Invoke(firstSymbol);
        }
    }

    private void SwitchShift(bool isActive)
    {
        shiftIsActive = isActive;
        UpdateButtonSprites();
    }

    private void UpdateButtonSprites()
    {
        if (!shiftIsActive)
        {
            keyImage.sprite = firstButtonSprite;
            
            var tempSpriteState = button.spriteState;
            tempSpriteState.pressedSprite = firstPressedButtonSprite;
            button.spriteState = tempSpriteState;
        }
        else
        {
            keyImage.sprite = secondButtonSprite;
            
            var tempSpriteState = button.spriteState;
            tempSpriteState.pressedSprite = secondPressedButtonSprite;
            button.spriteState = tempSpriteState;
        }
    }

    public void StartGame()
    {
        button.interactable = false;
        //SceneManager.LoadScene("Game 1");
    }
    
    public void ExitGame()
    {
        button.interactable = false;
        //Application.Quit();
    }
}