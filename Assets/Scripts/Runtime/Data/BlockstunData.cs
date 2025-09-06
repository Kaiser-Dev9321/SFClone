using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockstunData_", menuName = "Blockstun Data/New Blockstun Data")]
public class BlockstunData : ScriptableObject
{
    public string animationName;

    public AnimationCurve blockstunAnimationCurveX;
    public AnimationCurve blockstunAnimationCurveY;
}
