using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Jump : State_Air
{
    public FighterBasics_Air fighterBasics_Air;
    private float savedXValue;

    public override void State_Enter()
    {
        savedXValue = entity.fighterInput.movement.x;

        if (entity.fighterInput.movement.y > 0 && entity.entityMovement.groundChecker.grounded)
        {
            print("Prejump started");

            entity.entityMovement.movement.velocity.x = 0;

            entity.entityAnimator.PlayAnimation("Prejump");
            entity.inPrejump = true;
        }

        fighterBasics_Air.CheckAir_LandedOnGround(entity);
    }

    public override void State_Update()
    {
        if (entity.fighterInput.canAttack && entity.inPrejump)
        {
            print("Prejump ended");

            entity.inPrejump = false;

            entity.entityMovement.movement.velocity.x = entity.entityMovement.movement.movementData.horizontalSpeed * savedXValue;
            entity.entityMovement.movement.NewVelocityY(entity.entityMovement.movement.movementData.jumpHeight);
        }
        else
        {
            if (!entity.fighterInput.canAttack && entity.inPrejump)
            {
                if (savedXValue != 0)
                {
                    entity.entityMovement.movement.velocity.x = 0;
                }
            }
        }

        if (!entity.entityMovement.groundChecker.grounded && entity.entityMovement.movement.velocity.y > 0)
        {
            print("State air transition from jump");

            entity.stateMachine.ChangeState(stateMachine.state_air);
        }

        //base.State_Update();

        //TODO: Pre-jump frames

        //Check if pre-jump is done

        CheckAttacks();
    }

    public override void State_Exit()
    {
        //Just in case
        entity.inPrejump = false;
    }
}
