using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CancelListData_", menuName = "Cancel List Data/New Cancel List Data")]
public class CancelListData : ScriptableObject
{
    public AttackData[] cancellableAttacks;
}