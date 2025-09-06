using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HitstunData_", menuName = "Hitstun Data/New Hitstun Data")]
public class HitstunData : ScriptableObject
{
    public string animationName;

    public AnimationCurve hitstunAnimationCurveX;
    public AnimationCurve hitstunAnimationCurveY;

    public bool disableGroundCheck = false; //Just by default
}
