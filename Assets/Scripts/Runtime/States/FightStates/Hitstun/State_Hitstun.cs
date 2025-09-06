using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Hitstun : FighterState
{
    //TODO: Every fighter needs this
    public FighterBasics_Ground fighterBasics_Ground;

    public FighterPushEntity fighterPushEntity;

    [Space]
    public HitstunData groundRecoveryHitstun;
    public State_GroundRecoveryHitstun state_GroundRecoveryHitstun;
    public State_AirRecovery state_AirRecovery;

    [Space]
    public State_KnockedDown state_KnockedDown;

    public bool knockedDown;


    [HideInInspector]
    public string animationName;

    [HideInInspector]
    public int layer;

    [HideInInspector]
    public float normalisedTime;

    [HideInInspector]
    public AnimationCurve hitstunCurveX;

    [HideInInspector]
    public AnimationCurve hitstunCurveY;

    private float hitstunCurveTime;

    //[HideInInspector]
    public bool disableGroundCheck;

    private bool inheritlyAKnockdownState = false;
    private bool aboveMaxJuggle = false;

    private AttackData tmpStoredAttackData;
    private AttackEffectData tmpStoredAttackEffectData;

    public override void State_Enter()
    {
        if (!disableGroundCheck)
        {
            entity.entityMovement.DisableGroundCheck(0);
        }
        else
        {
            print("Ground hitstun knockdown state");

            inheritlyAKnockdownState = true;
            entity.entityMovement.DisableGroundCheck(1);
        }

        entity.entityAnimator.PlayAnimation(animationName, layer, normalisedTime);

        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.fighterAttackManager.SetPerformNormals(0);
            entity.fighterAttackManager.SetPerformSpecials(0);
        }

        aboveMaxJuggle = entity.fighterComboManager.attacker_Entity.fighterJuggleStateManager.FighterAboveMaxJuggle(entity.fighterComboManager.attacker_Entity);

        hitstunCurveTime = 0;
    }

    public override void State_Update()
    {
        if (!entity.entityMovement.disableGroundCheck)
        {
            disableGroundCheck = false;
        }

        if (!entity.entityHitstun.hitStunned)
        {
            if (entity.fighterComboManager.attacker_Entity)
            {
                if (entity.entityMovement.groundChecker.grounded)
                {
                    entity.entityAnimator.ReturnToNeutral();
                }
            }
            else
            {
                entity.entityAnimator.ReturnToNeutral();
            }
        }
        else
        {
            if (aboveMaxJuggle)
            {
                if (entity.entityHealth.GetCurrentHealth() > 0 && entity.fighterComboManager.attacker_Entity.fighterComboManager.currentPerformedAttackEffect.automaticallyRecoverFromAttack)
                {
                    if (!entity.isKnockedDown)
                    {
                        print("<b><size=14><color=#f291f0>Above max juggle, air recovery state</color></size></b>");
                        entity.stateMachine.ChangeState(entity.entityHitstun.state_airRecovery);
                    }
                }
            }
            else
            {
                if (entity.entityMovement.movement.velocity.y < -1 && !disableGroundCheck && entity.entityMovement.groundChecker.grounded)
                {
                    if (inheritlyAKnockdownState)
                    {
                        state_GroundRecoveryHitstun.animationName = groundRecoveryHitstun.animationName;
                        state_GroundRecoveryHitstun.layer = -1;
                        state_GroundRecoveryHitstun.normalisedTime = 0;

                        state_GroundRecoveryHitstun.hitstunCurveX = groundRecoveryHitstun.hitstunAnimationCurveX;
                        state_GroundRecoveryHitstun.hitstunCurveY = groundRecoveryHitstun.hitstunAnimationCurveY;

                        state_GroundRecoveryHitstun.disableGroundCheck = groundRecoveryHitstun.disableGroundCheck;

                        print($"Looking to the ground check now: {inheritlyAKnockdownState}");
                        entity.stateMachine.ChangeState(state_GroundRecoveryHitstun);
                    }
                }
            }
        }
    }

    public override void State_PhysicsUpdate()
    {
        float hitstunXDirection = -entity.GetXDirection() * hitstunCurveX.Evaluate(hitstunCurveTime);

        //Don't move if in hitstop
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            hitstunCurveTime += Time.fixedDeltaTime;

            entity.entityMovement.movement.NewVelocityX(hitstunXDirection);
            entity.entityMovement.movement.NewVelocityY(hitstunCurveY.Evaluate(hitstunCurveTime));
        }
    }

    public override void State_Exit()
    {
        inheritlyAKnockdownState = false;

        entity.entityMovement.DisableGroundCheck(0);
    }
}
