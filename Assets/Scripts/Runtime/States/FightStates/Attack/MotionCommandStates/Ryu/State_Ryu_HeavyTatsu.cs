using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ryu_HeavyTatsu : State_MotionCommand
{
    public FighterBasics_Air fighterBasics_Air;

    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity = Vector2.zero;

        entity.fighterInput.button_heavyKick = false;
        entity.entityMovement.movement.AssignOriginalTransformPosition();

        base.State_Enter();
    }

    public override void State_Update()
    {
        fighterBasics_Air.CheckAir_LandedOnGround(entity);

        if (entity.entityMovement.groundChecker.grounded)
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
