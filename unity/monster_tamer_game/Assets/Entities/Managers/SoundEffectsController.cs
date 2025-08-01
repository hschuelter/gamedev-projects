using UnityEngine;
using System.Collections.Generic;

public class SoundEffectsController : MonoBehaviour
{
    public static SoundEffectsController Instance { get; private set; }

    [Header("SFX Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private int maxAudioSources = 5;

    [Header("Static SFX")]
    public AudioClip sfxHit;
    public AudioClip sfxDodge;
    public AudioClip sfxHeal;
    public AudioClip sfxGuard;
    private List<AudioSource> audioSources = new List<AudioSource>();
    private int currentSourceIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSFXManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSFXManager()
    {
        // Create multiple AudioSources for overlapping sounds
        for (int i = 0; i < maxAudioSources; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = masterVolume;
            audioSources.Add(source);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.volume = masterVolume;
        source.Play();
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null) return;

        AudioSource source = GetAvailableSource();
        source.clip = clip;
        source.volume = masterVolume * Mathf.Clamp01(volume);
        source.Play();
    }

    public void PlaySFXAtPosition(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, position, masterVolume);
    }

    public void SetVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);

        // Update all existing sources
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
                source.volume = masterVolume;
        }
    }

    public void StopAllSFX()
    {
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
    }

    private AudioSource GetAvailableSource()
    {
        // Find a source that's not playing
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
                return source;
        }

        // If all sources are busy, use the next one in rotation
        AudioSource nextSource = audioSources[currentSourceIndex];
        currentSourceIndex = (currentSourceIndex + 1) % audioSources.Count;
        return nextSource;
    }

    public float Volume => masterVolume;
}