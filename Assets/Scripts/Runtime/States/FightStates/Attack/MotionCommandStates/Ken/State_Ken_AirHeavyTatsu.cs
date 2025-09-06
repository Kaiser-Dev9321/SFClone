using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_AirHeavyTatsu : State_AirTatsu
{
    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.fighterInput.button_heavyKick = false;
        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.entityMovement.groundChecker.grounded)
        {
            stateMachine.ChangeState(specialMoveLandState);
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
