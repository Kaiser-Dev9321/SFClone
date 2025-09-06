using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAudioStatus : MonoBehaviour
{
    public AudioSource thisAudioSource;

    public bool isPlayingAudio = false;

    private void Start()
    {
        thisAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        isPlayingAudio = thisAudioSource.isPlaying;
    }
}
