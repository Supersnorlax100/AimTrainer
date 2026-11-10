using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [SerializeField] AudioSource audioSource; 
    [SerializeField] AudioClip[] hitSounds;

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

    public void PlayHitSound()
    {
        audioSource.clip = hitSounds[0];
        // audioSource.clip = hitSounds[Random.Range(0, hitSounds.Length)];
        audioSource.Play();
    }
}
