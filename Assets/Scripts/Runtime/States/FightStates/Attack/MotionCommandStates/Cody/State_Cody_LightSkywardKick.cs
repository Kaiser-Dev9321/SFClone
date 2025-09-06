using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_LightSkywardKick : State_MotionCommand
{
    public FighterBasics_Air fighterBasics_Air;

    public AttackEffectData skywardKickAttackEffectData;

    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightKick = false;
        entity.fighterSuperManager.GainMeter(skywardKickAttackEffectData.effect_meterGainOnWhiff);

        base.State_Enter();
    }

    public override void State_Update()
    {
        fighterBasics_Air.CheckAir_LandedOnGround(entity);

        if (entity.entityMovement.groundChecker.grounded && timeVelocityCurveY > 0.6f)
        {
            stateMachine.ChangeState(specialMoveLandState);
        }

        base.State_Update();
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
