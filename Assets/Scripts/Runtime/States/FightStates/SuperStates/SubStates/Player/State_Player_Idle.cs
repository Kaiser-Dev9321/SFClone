using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_Idle : State_Idle
{
    public override void State_Enter()
    {
        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();

        entity.entityMovement.movement.velocity.x = 0;

        CheckAttacks();
    }

    public override void State_Exit()
    {
    }
}
