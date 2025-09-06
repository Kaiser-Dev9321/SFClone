using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyOptions
{
    public bool blockStanding, blockCrouching;
    public bool useAIController;
    public bool counterHit;
    public bool techAllThrows;

    public enum NeutralPose
    {
        Standing,
        Crouching
    }
}
