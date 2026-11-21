using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    GENERAL,
    MUSIC,
    SFX
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // [SerializeField] AudioSource audioSource; 
    public List<AudioSource> audioSources = new List<AudioSource>();

    public int[] soundVolume = new int[3]; // Should match soundtype, and changed in settings manager
    
    [Header("Sounds")]
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip uiOpenSound;
    [SerializeField] AudioClip uiCloseSound;
    [SerializeField] AudioClip purchaseSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Looping sounds may cause issues since creating audiosources
    void Sound(AudioClip audioClip, bool isPlaying, bool isLooping, SoundType soundType)
    {
        AudioSource audioSource = GetAudioSource();
        audioSource.loop = isLooping;
        if (isPlaying)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
            return;
        }

        audioSource.volume = soundVolume[(int)soundType];
    }

    AudioSource GetAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource == null) continue;

            if (! audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        // if did not find a free audiosource
        AudioSource _audioSource = gameObject.AddComponent<AudioSource>();
        audioSources.Add(_audioSource);
        return _audioSource;
    }


    #region For Other Scripts
    public void PlayHitSound()
    {
        Sound(hitSounds[0], true, false, SoundType.SFX);
        // audioSourceSFX.clip = hitSounds[Random.Range(0, hitSounds.Length)];
    }

    public void UiOpenSound()
    {
        Sound(uiOpenSound, true, false, SoundType.SFX);
    }

    public void UiCloseSound()
    {
        Sound(uiCloseSound, true, false, SoundType.SFX);
    }

    public void PurchaseSound()
    {
        Sound(purchaseSound, true, false, SoundType.SFX);
    }

    #endregion
}
