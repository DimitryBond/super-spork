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
        UpdateState();


        symbolSlot.SymbolInSlot.OnCompletedChanged += UpdateCompletedState;
    }

    public void UpdateState()
    {
        if (symbolSlot.SlotIndex == GameManager.Instance.CurrentTaskSymbolIndex)
        {
            image.sprite = waiting;
        }
        else
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
        else
        {
            image.sprite = denied;
        }
    }
}