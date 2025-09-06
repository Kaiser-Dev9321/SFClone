using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_LightShoryuken : State_MotionCommand
{
    public FighterBasics_Air fighterBasics_Air;

    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity = Vector2.zero;

        entity.fighterInput.button_lightPunch = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
        fighterBasics_Air.CheckAir_LandedOnGround(entity);

        if (entity.entityMovement.groundChecker.grounded && timeVelocityCurveY > 0.5f)
        {
            stateMachine.ChangeState(specialMoveLandState);
        }

        base.State_Update();
    }

    public override void State_Exit()
    {
        entity.entityMovement.movement.ResetOffsets();
        base.State_Exit();
    }
}
