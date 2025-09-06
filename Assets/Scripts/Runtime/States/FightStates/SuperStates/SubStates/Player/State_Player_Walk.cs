using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Player_Walk : State_Walk
{
    public override void State_Enter()
    {
        //playerEntityMove.movement.velocity.y = 0;
        entity.entityAnimator.PlayAnimation("Walk");
    }

    public override void State_Update()
    {
        base.State_Update();

        entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.horizontalSpeed * entity.fighterInput.movement.x;

        CheckAttacks();
    }

    public override void State_Exit()
    {
    }
}
