using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button button;

    public void StartGame()
    {
        button.interactable = false;
        SceneManager.LoadScene("Game Dima2");
    }

    public void ExitGame()
    {
        button.interactable = false;
        Application.Quit();
    }
}