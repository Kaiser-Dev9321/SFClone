using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceAudio : MonoBehaviour
{
    [HideInInspector]
    public AudioSource instanceAudio;

    public bool playOnAwake;

    private void Awake()
    {
        instanceAudio = GetComponent<AudioSource>();

        if (playOnAwake)
        {
            instanceAudio.Play();
        }
    }

    //Could be better to use an event?

    private void Update()
    {
        if (instanceAudio.time > instanceAudio.clip.length)
        {
            print("Probably finished");
        }

        /*
        if (!hasPlayed)
        {
            Destroy(instanceAudio);
        }*/
    }
}
