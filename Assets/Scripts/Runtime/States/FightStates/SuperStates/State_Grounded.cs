using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Grounded : FighterState
{
    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity.y = 0;
    }

    public override void State_Update()
    {
        if (entity.fighterInput.canAttack)
        {
            if (entity.entityMovement.movement.velocity.y != 0)
            {
                entity.entityMovement.movement.velocity.y = 0;
            }

            if (entity.fighterInput.movement.x != 0)
            {
                this.stateMachine.ChangeState(stateMachine.state_walk);
            }
            else
            {
                this.stateMachine.ChangeState(stateMachine.state_idle);
            }

            if (entity.fighterInput.movement.y > 0.5f)
            {
                this.stateMachine.ChangeState(stateMachine.state_jump);
            }

            if (!entity.entityMovement.groundChecker.grounded)
            {
                this.stateMachine.ChangeState(stateMachine.state_jump);
            }
        }
    }

    public override void State_Exit()
    {
    }
}
