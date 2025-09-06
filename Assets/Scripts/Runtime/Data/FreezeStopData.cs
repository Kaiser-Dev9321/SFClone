using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Stop Data", menuName = "Freeze Stop Data/New Freeze Stop Data")]
public class FreezeStopData : ScriptableObject
{
    public float freezeStop_Milliseconds;

    public bool freezeAttacker;
}
