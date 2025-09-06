using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_Walk : State_Lee_Grounded
{
    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityAnimator.PlayAnimation("Walk");
    }

    public override void State_Update()
    {
        base.State_Update();

        //CheckGrabs();
        CheckAttacks();
    }

    public override void State_PhysicsUpdate()
    {
        entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.horizontalSpeed * entity.fighterInput.movement.x;
    }

    public override void State_Exit()
    {
    }
}
