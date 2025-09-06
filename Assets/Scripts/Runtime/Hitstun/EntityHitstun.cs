using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHitstun : MonoBehaviour
{
    [HideInInspector]
    public State_Hitstun state_Hitstun;

    [HideInInspector]
    public State_AirHitstun state_airHitstun;

    [HideInInspector]
    public State_AirRecovery state_airRecovery;

    public bool hitStunned;

    private EntityScript entity;

    [HideInInspector]
    public FreezeStopData superFreezeStopData;

    public bool duringFreezeStop = false;
    public bool afterFreezeStop = false;

    private void Awake()
    {
        entity = GetComponent<EntityScript>();
        state_Hitstun = GetComponentInChildren<State_Hitstun>();
        state_airHitstun = GetComponentInChildren<State_AirHitstun>();
        state_airRecovery = GetComponentInChildren<State_AirRecovery>();
    }

    public void ChangeHitstunState(int value)
    {
        if (value == 0)
        {
            hitStunned = false;
        }
        else
        {
            hitStunned = true;
        }
    }

    public void ActivateGroundHitstun(HitstunData hitstunData)
    {
        //TODO: Can I put this into a function instead?
        state_Hitstun.animationName = hitstunData.animationName;
        state_Hitstun.layer = -1;
        state_Hitstun.normalisedTime = 0;

        state_Hitstun.hitstunCurveX = hitstunData.hitstunAnimationCurveX;
        state_Hitstun.hitstunCurveY = hitstunData.hitstunAnimationCurveY;

        state_Hitstun.disableGroundCheck = hitstunData.disableGroundCheck;

        //print("Ground hitstun");

        entity.stateMachine.ChangeState(state_Hitstun);
    }

    public void ActivateAirHitstun(HitstunData hitstunData, AirRecoveryData airRecoveryData)
    {
        state_airHitstun.animationName = hitstunData.animationName;
        state_airHitstun.layer = -1;
        state_airHitstun.normalisedTime = 0;

        state_airHitstun.hitstunCurveX = hitstunData.hitstunAnimationCurveX;
        state_airHitstun.hitstunCurveY = hitstunData.hitstunAnimationCurveY;

        state_airHitstun.disableGroundCheck = hitstunData.disableGroundCheck;


        state_airRecovery.airRecoveryData = airRecoveryData;
        state_airRecovery.animationName = airRecoveryData.animationName;
        state_airRecovery.layer = -1;
        state_airRecovery.normalisedTime = 0;

        state_airRecovery.airRecoveryCurveX = airRecoveryData.airRecoveryAnimationCurveX;
        state_airRecovery.airRecoveryCurveY = airRecoveryData.airRecoveryAnimationCurveY;

        //print("Air hitstun");

        entity.stateMachine.ChangeState(state_airHitstun);
    }
    
    private void ActivateSpecificHitstun(HitstunData hitstunData, bool airHitstun, AirRecoveryData airRecoveryData = null)
    {
        if (!airHitstun)
        {
            ActivateGroundHitstun(hitstunData);
        }
        else
        {
            ActivateAirHitstun(hitstunData, airRecoveryData);
        }
    }

    public void ActivateHitstun(HitstunData hitstunData, bool airHitstun, AirRecoveryData airRecoveryData = null, bool overrideAnimationName = false, string overrideAnimationString = "")
    {
        if (!hitstunData)
        {
            return;
        }

        if (!overrideAnimationName)
        {
            ActivateSpecificHitstun(hitstunData, airHitstun, airRecoveryData);
        }
        else
        {
            HitstunData tmpNewHitstunData = ScriptableObject.CreateInstance<HitstunData>();
            tmpNewHitstunData.animationName = overrideAnimationString;
            tmpNewHitstunData.disableGroundCheck = hitstunData.disableGroundCheck;
            tmpNewHitstunData.hitstunAnimationCurveX = hitstunData.hitstunAnimationCurveX;
            tmpNewHitstunData.hitstunAnimationCurveY = hitstunData.hitstunAnimationCurveY;

            ActivateSpecificHitstun(tmpNewHitstunData, airHitstun, airRecoveryData);
        }
    }

    public void SuperFreezeStopFighters()
    {
        entity.fighterGameplayManager.freezeStopManager.AssignFreezeStopTime(superFreezeStopData);
        entity.fighterGameplayManager.freezeStopManager.QuickSuperFreezeStop(entity);
    }
}
