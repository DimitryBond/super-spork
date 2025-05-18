using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance; 
    
    [Header("Музыка интро")]
    public AudioClip introMusic; // Музыка для сцены интро
    
    [Header("Музыка для главного меню")]
    public AudioClip[] mainMenuMusic; // Массив треков для главного меню
    private int currentMainMenuTrack = 0; // Индекс текущего трека для главного меню

    [Header("Музыка для игры")]
    public AudioClip[] gameMusic; // Массив треков для игры
    private int currentGameTrack = 0; // Индекс текущего трека для игры

    private bool isPlayingIntro = true;
    private bool isPlayingMainMenu = false;
    
    private AudioSource audioSource;
    private bool isFadingOut = false;
    private float fadeSpeed = 0.2f;

    private void Awake()
    {
        // Создаем синглтон
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Если текущий трек закончился
        if (!audioSource.isPlaying)
        {
            if (isPlayingIntro)
            {
                // После окончания интро переходим к списку главного меню
                SwitchToMainMenuMusic();
            }
            else
            {
                // Воспроизводим следующий трек из текущего списка
                PlayNextTrack();
            }
        }
        
        // Плавное затухание музыки
        if (isFadingOut)
        {
            audioSource.volume -= fadeSpeed * Time.deltaTime;

            // Если громкость достигла нуля, останавливаем музыку
            if (audioSource.volume <= 0f)
            {
                audioSource.Stop();
                audioSource.volume = 0f; // Обнуляем громкость
                isFadingOut = false;     // Останавливаем затухание
            }
        }
    }
    
    
    // Метод для воспроизведения следующего трека
    private void PlayNextTrack()
    {
        audioSource.volume = 1f;
        if (isPlayingMainMenu)
        {
            // Воспроизводим следующий трек из списка для главного меню
            audioSource.clip = mainMenuMusic[currentMainMenuTrack];
            currentMainMenuTrack = (currentMainMenuTrack + 1) % mainMenuMusic.Length; // Циклический переход
        }
        else
        {
            // Воспроизводим следующий трек из списка для игры
            audioSource.clip = gameMusic[currentGameTrack];
            currentGameTrack = (currentGameTrack + 1) % gameMusic.Length; // Циклический переход
        }

        audioSource.Play();
    }
    
    // Метод для воспроизведения музыки интро
    public void PlayIntroMusic()
    {
        isPlayingIntro = true;
        audioSource.clip = introMusic;
        audioSource.Play();
    }
    
    // Метод для переключения на музыку главного меню
    public void SwitchToMainMenuMusic()
    {
        isPlayingIntro = false;
        isPlayingMainMenu = true;
        PlayNextTrack(); 
    }
    
    // Метод для переключения на музыку игры
    public void SwitchToGameMusic()
    {
        isPlayingMainMenu = false;
        PlayNextTrack(); 
    }

    // Метод для воспроизведения новой музыки
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    // Метод для остановки текущей музыки
    public void StopMusic()
    {
        audioSource.Stop();
    }
    
    // Метод для запуска плавного затухания
    public void SetFadeOutMusic(bool isFadeOut)
    {
        isFadingOut = isFadeOut; // Включаем флаг затухания
    }

    // Метод для изменения громкости
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Метод для уничтожения менеджера музыки
    public void DestroyMusicManager()
    {
        Destroy(gameObject);
    }
}
