using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FighterMovementData_", menuName = "Fighter Movement Data/Fighter Movement Data")]
public class FighterMovementData : ScriptableObject
{
    [Range(0f, 100f)]
    public float horizontalSpeed = 5;

    [Range(0f, 100f)]
    public float slowHorizontalSpeed = 2;

    [Range(4f, 100f)]
    public float jumpHeight = 8;

    [Range(-40f, -1f)]
    public float fallSpeed = -8;

    [Range(0.1f, 40f)]
    public float fallTimeLerp = 2.5f;
}
