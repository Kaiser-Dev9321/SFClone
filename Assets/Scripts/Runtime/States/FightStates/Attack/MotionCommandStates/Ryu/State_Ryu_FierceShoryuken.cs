using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ryu_FierceShoryuken : State_MotionCommand
{
    public FighterBasics_Air fighterBasics_Air;

    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.fighterInput.button_fiercePunch = false;

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
        base.State_Exit();
    }
}
