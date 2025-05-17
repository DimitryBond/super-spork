using UnityEngine;
using UnityEngine.UI;

public class CompleteIndicator : MonoBehaviour
{
    [SerializeField] private Image image;
    
    [SerializeField] private Sprite complete;
    [SerializeField] private Sprite waiting;
    [SerializeField] private Sprite denied;
    [SerializeField] private Sprite offed;
    
    private SymbolSlot symbolSlot;

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
        if (index <= GameManager.Instance.CurrentSymbolIndex && symbolSlot.SymbolInSlot.IsCompleted)
        {
        }
        else if (index == GameManager.Instance.CurrentSymbolIndex)
        {
            image.sprite = waiting;
        }
        else if (index > GameManager.Instance.CurrentSymbolIndex)
        {
            image.sprite = offed;
        }
    }

    private void UpdateCompletedState(bool isCompleted)
    {
        if (isCompleted)
        {
            image.sprite = complete;
        }
    }
    
    private void UpdateDeniedState(bool isDenied)
    {
        if (isDenied)
        {
            image.sprite = denied;
        }
    }
}