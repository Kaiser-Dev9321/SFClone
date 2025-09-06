using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Reduce the repetition of special move landing by making a base version in FighterBasics
public class State_Lee_LightSenkyuutai : State_MotionCommand
{
    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightKick = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
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
