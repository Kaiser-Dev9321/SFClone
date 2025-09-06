using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CommandType
{
    CommandNormal,
    MotionInput
}

[System.Serializable]
public class MotionInputValues
{
    [Range(0.01f, 3)]
    public float timeLeniency = 0.1f;
    public FighterMotionInputData[] previousMotionInputData;
    public AttackData attackData;
    public VoicelineData voicelineData;

    public CommandType commandType;
    public FighterAttackButtons buttonToPress;
    public string motionCommandID;
    public bool mustPerformOnGround;
    public bool mustPerformInAir;
    public bool conditionFulfilled;
}

public class FighterBaseMotionInputButtonData : ScriptableObject
{
    public EntityScript entity;
    public MotionInputValues motionInputValues;

    public string testMessageToPrint;

    public virtual bool RequireCondition()
    {
        motionInputValues.conditionFulfilled = true;
        return motionInputValues.conditionFulfilled;
    }
}
