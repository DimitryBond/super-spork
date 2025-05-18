using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button button;

    public void StartGame()
    {
        button.interactable = false;
        MusicManager.Instance.SetFadeOutMusic(true);
        MusicManager.Instance.StopMusic();
        Invoke("LoadScene", 2f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("Game Dima2");
        MusicManager.Instance.SetFadeOutMusic(true);
    }
    
    public void ExitGame()
    {
        button.interactable = false;
        Application.Quit();
    }
}