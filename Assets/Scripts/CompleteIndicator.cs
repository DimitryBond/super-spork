using UnityEngine;
using UnityEngine.UI;

public class CompleteIndicator : MonoBehaviour
{
    [SerializeField] private Image image;
    
    [SerializeField] private Color complete;
    [SerializeField] private Color waiting;
    [SerializeField] private Color denied;
    [SerializeField] private Color offed;
    
    private SymbolSlot symbolSlot;
    private bool canSwitchState = true;

    public void Initialize(SymbolSlot slot)
    {
        symbolSlot = slot;
        UpdateState(GameManager.Instance.CurrentSymbolIndex);


        symbolSlot.SymbolInSlot.OnCompletedChanged += UpdateCompletedState;
        symbolSlot.SymbolInSlot.OnDenaidChanged += UpdateDeniedState;
        GameManager.Instance.OnCurrentSymbolIndexChanged += UpdateState;
    }

    private void OnDestroy()
    {
        symbolSlot.SymbolInSlot.OnCompletedChanged -= UpdateCompletedState;
        symbolSlot.SymbolInSlot.OnDenaidChanged -= UpdateDeniedState;
        GameManager.Instance.OnCurrentSymbolIndexChanged -= UpdateState;
    }

    public void UpdateState(int index)
    {
        if (!canSwitchState) return;
        
        if (index == GameManager.Instance.CurrentSymbolIndex)
        {
            image.color = waiting;
        }
        else if (index > GameManager.Instance.CurrentSymbolIndex)
        {
            image.color = offed;
        }
    }

    private void UpdateCompletedState(bool isCompleted)
    {
        if (isCompleted)
        {
            image.color = complete;
            canSwitchState = false;
        }
    }
    
    private void UpdateDeniedState(bool isDenied)
    {
        if (isDenied)
        {
            image.color = denied;
            canSwitchState = false;
        }
    }
}