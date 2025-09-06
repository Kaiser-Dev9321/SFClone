using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AirRecoveryData_", menuName = "Air Recovery Data/New Air Recovery Data")]
public class AirRecoveryData : ScriptableObject
{
    public string animationName;

    public AnimationCurve airRecoveryAnimationCurveX;
    public AnimationCurve airRecoveryAnimationCurveY;
}
