using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputData_", menuName = "Input Data/New Motion Input Data")]
public class FighterMotionInputData : ScriptableObject
{
    public int motionInputIndex;
    public InputCardinalDirection directionToPress;

    [Range(0.001f, 2)] //0.1 is least lenient time, 2 is the most lenient time
    public float timeLeniency = 0.1f;
}
