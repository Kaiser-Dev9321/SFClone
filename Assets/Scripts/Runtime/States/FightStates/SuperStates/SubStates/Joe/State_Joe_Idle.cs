using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_Idle : State_Joe_Grounded
{
    public override void State_Enter()
    {
        //TODO: Add CompletelyEnableAttacking on grounded and air states

        base.State_Enter();

        entity.entityAnimator.PlayAnimation("Idle");
    }

    public override void State_Update()
    {
        base.State_Update();

        entity.entityMovement.movement.velocity.x = 0;

        CheckGrabs();
        CheckAttacks();
    }

    public override void State_Exit()
    {
    }
}
