using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

enum TrackNumber
{
    TitleTheme = 0,
    BattleTheme = 1,
    VictoryTheme = 2,
    DefeatTheme = 3,
    MapTheme = 4
}

public class MusicPlayerController : MonoBehaviour
{    
    // Singleton instance
    public static MusicPlayerController Instance { get; private set; }


    [Header("Audio Components")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip titleTrack;
    [SerializeField] private AudioClip battleTrack;
    [SerializeField] private AudioClip victoryTrack;


    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float fadeSpeed = 0.2f;

    private bool isPlaying = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeMusicPlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        //PlayMusic((int) TrackNumber.BattleTheme);
    }
    private void InitializeMusicPlayer()
    {
        // Get or add AudioSource component
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        audioSource.playOnAwake = false;
        //audioSource.loop = loopCurrentTrack;
        //audioSource.volume = masterVolume;
    }

    #region Public Methods
    public void PlayMusic(int index)
    {
        if (isPlaying)
        {
            StartCoroutine(CrossFadeCoroutine(index, fadeSpeed));
        }
        else
        {
            LoadTrack(index);
            audioSource.Play();
        }

        isPlaying = true;
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPlaying = false;
        }
    }
    public void StopMusic()
    {
        audioSource.Stop();
        isPlaying = false;
    }
    #endregion

    #region Prviate Methods

    private void LoadTrack(int index)
    {
        switch (index)
        {
            case 0:
                audioSource.clip = titleTrack;
                break;

            case 1:
                audioSource.clip = battleTrack;
                break;

            case 2:
                audioSource.clip = victoryTrack;
                break;

            default:
                audioSource.clip = titleTrack;
                break;
        }
    }

    private IEnumerator CrossFadeCoroutine(int newTrackIndex, float duration)
    {
        // Fade out current track
        yield return StartCoroutine(FadeVolume(audioSource.volume, 0f, duration * 0.5f));

        LoadTrack(newTrackIndex);

        audioSource.Play();
        yield return StartCoroutine(FadeVolume(0f, masterVolume, duration * 0.5f));
    }

    #endregion

    #region Coroutines

    private IEnumerator FadeVolume(float startVolume, float targetVolume, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0f && isPlaying)
        {
            StopMusic();
        }
    }
    #endregion

}
