using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_LightDuckLowerForward : State_MotionCommand
{
    public State_Mike_DuckLowerStraight duckLowerStraightState;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightKick = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
        if (entity.fighterAttackManager.canPerformNormals)
        {
            if (entity.fighterInput.button_lightPunch || entity.fighterInput.button_heavyPunch || entity.fighterInput.button_fiercePunch)
            {
                duckLowerStraightState.hitInFront = true;
                stateMachine.ChangeState(duckLowerStraightState);
            }
            else if (entity.fighterInput.button_lightKick || entity.fighterInput.button_heavyKick || entity.fighterInput.button_fierceKick)
            {
                duckLowerStraightState.hitInFront = false;
                stateMachine.ChangeState(duckLowerStraightState);
            }
        }

        base.State_Update();
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
