using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectStage : MonoBehaviour
{
    public int activeStage;

    //Select stage
    public void AssignActiveStage(int stageToPick)
    {
        activeStage = stageToPick;
    }
}
