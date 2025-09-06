using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_LightHadoken : State_MotionCommand
{
    public override void State_Enter()
    {
        entity.fighterInput.button_lightPunch = false;

        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;
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
