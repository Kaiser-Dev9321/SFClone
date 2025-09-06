using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Pernin_LightSlowArcThorn : State_MotionCommand
{
    public FighterBasics_Air fighterBasics_Air;
    public State_Pernin_SlowArcThornRecovery slowArcThornRecovery;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightPunch = false;
        base.State_Enter();
    }

    public override void State_Update()
    {
        fighterBasics_Air.CheckAir_LandedOnGround(entity);

        base.State_Update();

        if (entity.fighterInput.canAttack && entity.entityMovement.groundChecker.grounded)
        {
            entity.stateMachine.ChangeState(slowArcThornRecovery);
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
