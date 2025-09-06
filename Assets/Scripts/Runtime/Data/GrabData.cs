using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrabData_", menuName = "Grab Data/New Grab Data")]
public class GrabData : ScriptableObject
{
    public float grabTimer;

    public bool isAHold;
    public float minimumGrabSpamAmount;
}