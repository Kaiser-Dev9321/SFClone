using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementCurves
{
    public MotionCurveData motionCurveData { get; set; }

    public float timeVelocityCurveX { get; set; }
    public float timeVelocityCurveY { get; set; }

    public float timeTranslateCurveX { get; set; }
    public float timeTranslateCurveY { get; set; }
}
