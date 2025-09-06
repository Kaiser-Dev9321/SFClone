using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_GroundRecoveryHitstun : FighterState
{
    [HideInInspector]
    public string animationName;

    [HideInInspector]
    public int layer;

    [HideInInspector]
    public float normalisedTime;

    //TODO: Every fighter needs this
    public FighterBasics_Ground fighterBasics_Ground;

    [HideInInspector]
    public AnimationCurve hitstunCurveX;

    [HideInInspector]
    public AnimationCurve hitstunCurveY;

    private float hitstunCurveTime;

    [HideInInspector]
    public bool disableGroundCheck;

    public override void State_Enter()
    {
        entity.onTheGround = true;

        entity.entityAnimator.PlayAnimation(animationName, layer, normalisedTime);

        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.fighterAttackManager.SetPerformNormals(0);
            entity.fighterAttackManager.SetPerformSpecials(0);
        }

        hitstunCurveTime = 0;

        fighterBasics_Ground.SetGroundColliderTransform(entity);
        fighterBasics_Ground.SetGroundColliderActive(entity, true);
    }

    public override void State_Update()
    {
        if (!entity.entityHitstun.hitStunned)
        {
            entity.entityAnimator.ReturnToNeutral();
        }

        if (disableGroundCheck)
        {
            entity.entityMovement.DisableGroundCheck(1);
        }

        hitstunCurveTime += Time.deltaTime;

        entity.entityMovement.movement.NewVelocityX(hitstunCurveX.Evaluate(hitstunCurveTime));
        entity.entityMovement.movement.NewVelocityY(hitstunCurveY.Evaluate(hitstunCurveTime));
    }

    public override void State_Exit()
    {
        entity.onTheGround = false;

        entity.entityMovement.DisableGroundCheck(0);
        entity.fighterAttackManager.SetPerformNormals(1);
        entity.fighterAttackManager.SetPerformSpecials(1);
    }
}
