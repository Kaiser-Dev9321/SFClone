using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToMainMenu : MonoBehaviour
{
    private SceneLoader sceneLoader;
    public CheckAudioStatus audioStatus;

    private void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void LateUpdate()
    {
        if (!audioStatus.isPlayingAudio)
        {
            sceneLoader.LoadScene("MainMenu");
        }
    }
}
