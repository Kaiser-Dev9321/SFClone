using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcadeModeData_", menuName = "Arcade Mode Data/New Arcade Mode Data")]
public class ArcadeModeData : ScriptableObject
{
    public GameObject currentOpponent;
    public string stageName;

    public bool hasNextArcadeLadder;
    public ArcadeModeData nextArcadeLadder;
}