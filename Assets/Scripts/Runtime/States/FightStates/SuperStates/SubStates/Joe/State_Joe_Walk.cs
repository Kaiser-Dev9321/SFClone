using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_Walk : State_Joe_Grounded
{
    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityAnimator.PlayAnimation("Walk");
    }

    public override void State_Update()
    {
        base.State_Update();

        //entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.horizontalSpeed * entity.fighterInput.movement.x;

        CheckGrabs();
        CheckAttacks();
    }

    public override void State_PhysicsUpdate()
    {
        //Slow down movement when touching other fighter

        if (!entity.entityMovement.collisionChecker.colliderTouching)
        {
            entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.horizontalSpeed * entity.fighterInput.movement.x;
        }
        else
        {
            entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.slowHorizontalSpeed * entity.fighterInput.movement.x;
        }

    }

    public override void State_Exit()
    {
    }
}
