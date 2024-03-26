using System.Collections.Generic;
using UnityEngine;

// this class manages the audio playback for the game
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private Queue<AudioClip> musicQueue = new Queue<AudioClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        StartCoroutine(PlayMusicQueue());
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void EnqueueMusic(AudioClip clip)
    {
        musicQueue.Enqueue(clip);
    }

    private IEnumerator<WaitForSeconds> PlayMusicQueue()
    {
        while (true)
        {
            if (musicQueue.Count > 0 && !musicSource.isPlaying)
            {
                musicSource.clip = musicQueue.Dequeue();
                musicSource.Play();
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
