using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAudio : MonoBehaviour
{
    public AudioSource entityAudio;

    public AudioClip lightHitAudio;
    public AudioClip heavyHitAudio;
    public AudioClip fierceHitAudio;

    public void PlayAudio(AudioClip audioClip)
    {
        entityAudio.clip = audioClip;
        entityAudio.Play();
    }
}
