using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        var a = FindObjectOfType<GameManager>();
        Destroy(a.gameObject);
    }

    public void StartGame()
    {
        button.interactable = false;
        MusicManager.Instance.SetFadeOutMusic(true);
        MusicManager.Instance.StopMusic();
        SceneManager.LoadScene("Game Dima23");
        MusicManager.Instance.SetFadeOutMusic(true);
    }

    
    public void ExitGame()
    {
        button.interactable = false;
        Application.Quit();
    }
}