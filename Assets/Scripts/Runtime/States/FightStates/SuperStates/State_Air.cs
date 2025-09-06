using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: This seems to go completely unused so I will merge it into State_Jump
public class State_Air : FighterState
{
    public override void State_Enter()
    {
    }

    public override void State_Update()
    {
        if (entity.entityMovement.groundChecker.grounded && entity.entityMovement.movement.velocity.y < 0)
        {
            entity.entityMovement.movement.velocity.y = 0;

            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.fighterInput.canAttack = true;
            entity.entityAnimator.ReturnToNeutral();
        }

        entity.entityMovement.movement.NewVelocityY(Mathf.Lerp(entity.entityMovement.movement.velocity.y, entity.entityMovement.movement.movementData.fallSpeed, entity.entityMovement.movement.movementData.fallTimeLerp * Time.fixedDeltaTime));
    }

    public override void State_Exit()
    {
    }
}
