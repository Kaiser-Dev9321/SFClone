using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManuallyPlayAudio : MonoBehaviour
{
    public GameObject audioVoicelinesParent;

    public void PlayManualAudio(int audioInt)
    {
        audioVoicelinesParent.transform.GetChild(audioInt).GetComponent<AudioSource>().Play();
    }
}
