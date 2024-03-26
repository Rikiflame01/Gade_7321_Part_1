using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to enqueue music tracks for playback in the game.

public class MusicEnqueuer : MonoBehaviour
{
    [SerializeField] private AudioClip level1MusicTrack1;
    [SerializeField] private AudioClip level1MusicTrack2;
    [SerializeField] private AudioClip level1MusicTrack3;
    [SerializeField] private AudioClip level1MusicTrack4;

    void Start()
    {
        EnqueueMusicTracks();
    }

    private void EnqueueMusicTracks()
    {
        AudioManager.Instance.EnqueueMusic(level1MusicTrack1);
        AudioManager.Instance.EnqueueMusic(level1MusicTrack2);
        AudioManager.Instance.EnqueueMusic(level1MusicTrack3);
        AudioManager.Instance.EnqueueMusic(level1MusicTrack4);
        

    }
}

