using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{
    public GameObject mainMenuEmpty;
    public GameObject trainingScenesEmpty;

    public void TransitionToTrainingScenes(int enable)
    {
        if (enable == 0)
        {
            mainMenuEmpty.SetActive(true);
            trainingScenesEmpty.SetActive(false);
        }
        else
        {

            mainMenuEmpty.SetActive(false);
            trainingScenesEmpty.SetActive(true);
        }
    }
}
