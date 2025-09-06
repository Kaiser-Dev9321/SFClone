using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_FierceSenkyuutai : State_MotionCommand
{
    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.fighterInput.button_fierceKick = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
        if (entity.entityMovement.groundChecker.grounded && timeVelocityCurveY > 0.7f)
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
