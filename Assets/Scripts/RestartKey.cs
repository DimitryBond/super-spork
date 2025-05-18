using System;
using UnityEngine;
using UnityEngine.UI;

public class RestartKey : MonoBehaviour
{
    [SerializeField] private Button button;

    private bool canTouch;
    
    public event Action OnKeyPressed;

    public void Initialize()
    {
        GameManager.Instance.OnStringTaskFinished += LockButton;
        GameManager.Instance.OnTaskStarted += UnlockButton;
    }

    private void LockButton()
    {
        canTouch = false;
    }

    private void UnlockButton()
    {
        canTouch = true;
    }
    
    public void PressKey()
    {
        if (canTouch)
            OnKeyPressed?.Invoke();
    }
}