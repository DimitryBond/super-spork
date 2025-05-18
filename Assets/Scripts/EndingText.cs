using TMPro;
using UnityEngine;

public class EndingText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;

    private int endNumber;

    private void Start()
    {
        MusicManager.Instance.canSwitch = false;
        MusicManager.Instance.StopMusic();
        
        if (GameManager.Instance.AllTasksCompleted)
        {
            text1.text = "Концовка 1 из 3";
            return;
        }
        else if (GameManager.Instance.PlayerWasDestroy)
        {
            text1.text = "Концовка 2 из 3";
            return;
        }
        else if (GameManager.Instance.PlayerDestroyCommander)
        {
            text1.text = "Концовка 3 из 3";
            return;
        }
    }
}