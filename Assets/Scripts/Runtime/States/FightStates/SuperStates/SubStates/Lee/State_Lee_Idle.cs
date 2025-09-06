using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_Idle : State_Lee_Grounded
{
    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityAnimator.PlayAnimation("Idle");
    }

    public override void State_Update()
    {
        base.State_Update();

        entity.entityMovement.movement.velocity.x = 0;

        //CheckGrabs();
        CheckAttacks();
    }

    public override void State_Exit()
    {
    }
}
