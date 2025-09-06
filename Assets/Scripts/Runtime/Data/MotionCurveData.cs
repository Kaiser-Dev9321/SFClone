using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MotionCurveData_", menuName = "Curve Data/New Motion Curve Data")]
public class MotionCurveData : ScriptableObject
{
    public AnimationCurve animationCurve_velocityX;
    public AnimationCurve animationCurve_velocityY;
    public AnimationCurve animationCurve_translateX;
    public AnimationCurve animationCurve_translateY;
}
