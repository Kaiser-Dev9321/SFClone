using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData_", menuName = "Attack Data/New Attack Data")]
public class AttackData : ScriptableObject
{
    public FighterStateData attackStateData;
    public TargetComboData[] targetComboData;
    public CancelListData specialsCancelListData;
    public CancelListData superCancelListData;

    [Space]
    public FighterBaseMotionInputButtonData[] motionInputsAttachedToAttackData;

    public FighterAttackButtons fighterAttackButton;

    public bool unrestrictedChainCancelTC = true;
    public bool unrestrictedComboTC = true;
}