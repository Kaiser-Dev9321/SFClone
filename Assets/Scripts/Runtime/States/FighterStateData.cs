using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateData_", menuName = "Fighter Data/New State Data")]
public class FighterStateData : ScriptableObject
{
    public string stateAnimation;

    [Space]
    public bool chainCancellable = false;
    public bool specialCancellable = false;
    public bool superCancellable = false;
}
