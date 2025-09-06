using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AirHitstun : FighterState
{
    //TODO: Every fighter needs this
    public FighterBasics_Air fighterBasics_Air;

    public FighterPushEntity fighterPushEntity;

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

    public bool disableGroundCheck;

    //TODO: All fighters need this now
    [Space]
    public HitstunData groundRecoveryHitstun;
    public State_GroundRecoveryHitstun state_GroundRecoveryHitstun;
    public State_AirRecovery state_AirRecovery;

    private AttackEffectData savedAttackEffect;

    [Space]
    public State_KnockedDown state_KnockedDown;

    public bool knockedDown;

    private bool checkedJuggle = false;
    private bool aboveMaxJuggle = false;

    //TODO: Fix stuff when attacks are performed on the right side

    private void MaxJuggleCheck()
    {
        aboveMaxJuggle = entity.fighterComboManager.attacker_Entity.fighterJuggleStateManager.FighterAboveMaxJuggle(entity.fighterComboManager.attacker_Entity);
    }

    public override void State_Enter()
    {
        entity.entityAnimator.PlayAnimation(animationName, layer, normalisedTime);

        hitstunCurveTime = 0;

        if (disableGroundCheck)
        {
            print("DISABLE G 1");

            entity.entityMovement.DisableGroundCheck(1);
            entity.fighterAttackManager.SetPerformNormals(0);
            entity.fighterAttackManager.SetPerformSpecials(0);
        }

        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
        }

        if (!entity)
        {
            entity = GetComponentInParent<EntityScript>();
        }

        savedAttackEffect = entity.fighterComboManager.attacker_Entity.fighterComboManager.currentPerformedAttackEffect;

        fighterBasics_Air.CheckAir_LandedOnGround(entity);
    }

    public override void State_Update()
    {
        if (!entity.entityMovement.disableGroundCheck)
        {
            disableGroundCheck = false;
        }

        if (!checkedJuggle)
        {
            checkedJuggle = true;
            MaxJuggleCheck();
        }

        if (aboveMaxJuggle)
        {
            if (entity.entityHealth.GetCurrentHealth() > 0 && savedAttackEffect.automaticallyRecoverFromAttack)
            {
                if (!entity.isKnockedDown)
                {
                    //print("<b><size=14><color=#f291f0>Above max juggle, air recovery state</color></size></b>");
                    entity.stateMachine.ChangeState(entity.entityHitstun.state_airRecovery);
                }
            }
        }

        if (entity.entityMovement.movement.velocity.y < -1 && !entity.entityMovement.disableGroundCheck && entity.entityMovement.groundChecker.grounded)
        {
            if (!knockedDown)
            {
                state_GroundRecoveryHitstun.animationName = groundRecoveryHitstun.animationName;
                state_GroundRecoveryHitstun.layer = -1;
                state_GroundRecoveryHitstun.normalisedTime = 0;

                state_GroundRecoveryHitstun.hitstunCurveX = groundRecoveryHitstun.hitstunAnimationCurveX;
                state_GroundRecoveryHitstun.hitstunCurveY = groundRecoveryHitstun.hitstunAnimationCurveY;

                state_GroundRecoveryHitstun.disableGroundCheck = groundRecoveryHitstun.disableGroundCheck;

                if (!entity.entityMovement.disableGroundCheck)
                {
                    stateMachine.ChangeState(state_GroundRecoveryHitstun);
                }
            }
            else
            {
                print("Knock down state activate");
                stateMachine.ChangeState(state_KnockedDown);
            }
        }
    }

    public override void State_PhysicsUpdate()
    {
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            hitstunCurveTime += Time.fixedDeltaTime;

            float hitstunXDirection = -entity.GetXDirection() * hitstunCurveX.Evaluate(hitstunCurveTime);

            entity.entityMovement.movement.NewVelocityX(hitstunXDirection);
            entity.entityMovement.movement.NewVelocityY(hitstunCurveY.Evaluate(hitstunCurveTime));
        }
    }

    public override void State_Exit()
    {
        checkedJuggle = false;
        savedAttackEffect = null;

        entity.entityMovement.DisableGroundCheck(0);
    }
}
