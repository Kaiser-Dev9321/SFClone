using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AirRecovery : FighterState
{
    //TODO: Combine repeated elements into an interface

    [HideInInspector]
    public string animationName;

    [HideInInspector]
    public int layer;

    [HideInInspector]
    public float normalisedTime;

    [Space]
    public AirRecoveryData airRecoveryData;

    [HideInInspector]
    public AnimationCurve airRecoveryCurveX;

    [HideInInspector]
    public AnimationCurve airRecoveryCurveY;

    private float airRecoveryCurveTime;

    [Space]
    public HitstunData groundRecoveryHitstun;
    public State_GroundRecoveryHitstun state_GroundRecoveryHitstun;

    [Space]
    public State_KnockedDown state_KnockedDown;

    public bool knockedDown;

    public override void State_Enter()
    {
        print("<b>Play air recovery animation</b>");
        entity.entityAnimator.PlayAnimation(animationName, layer, normalisedTime);

        airRecoveryCurveTime = 0;

        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.fighterAttackManager.SetPerformNormals(0);
            entity.fighterAttackManager.SetPerformSpecials(0);
        }
    }

    public override void State_Update()
    {
        airRecoveryCurveTime += Time.deltaTime;

        //TODO: Perhaps actual direction x would be better
        entity.entityMovement.movement.NewVelocityX(-entity.GetXDirection() * airRecoveryCurveX.Evaluate(airRecoveryCurveTime));
        entity.entityMovement.movement.NewVelocityY(airRecoveryCurveY.Evaluate(airRecoveryCurveTime));

        if (entity.entityMovement.groundChecker.grounded)
        {
            entity.entityHitstun.hitStunned = false;

            print("<b><size=14><color=#f292f6>Recover from air recovery</color></size></b>");
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        entity.entityMovement.DisableGroundCheck(0);
        entity.fighterAttackManager.SetPerformNormals(1);
        entity.fighterAttackManager.SetPerformSpecials(1);
    }
}
