using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_LightKobokushi : State_MotionCommand
{
    public override void State_Enter()
    {
        entity.fighterInput.button_lightPunch = false;

        entity.entityMovement.movement.velocity.x = 0;

        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
