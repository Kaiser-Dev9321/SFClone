using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_FierceHadoken : State_MotionCommand
{
    public override void State_Enter()
    {
        entity.fighterInput.button_fiercePunch = false;

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
