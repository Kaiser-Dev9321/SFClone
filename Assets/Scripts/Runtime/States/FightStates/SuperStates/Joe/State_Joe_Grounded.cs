using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_Grounded : JoeFighterState
{
    public FighterBasics_Ground fighterBasics_Ground;

    public override void State_Enter()
    {
        //Clear state data when attack ends
        stateMachine.currentAttackStateData = null;

        entity.CompletelyEnableAttacking();
        entity.fighterMotionInputManager.DisableInputCancels();

        fighterBasics_Ground.SetGroundColliderTransform(entity);
        fighterBasics_Ground.SetGroundColliderActive(entity, true);

        entity.entityMovement.movement.velocity.y = 0;
    }

    public override void State_Update()
    {
        {
            if (entity.fighterInput.canAttack)
            {
                if (entity.fighterInput.movement.y > 0.5f && !entity.inPrejump)
                {
                    this.stateMachine.ChangeState(stateMachine.state_jump);
                }

                if (!entity.entityMovement.groundChecker.grounded && !entity.inPrejump)
                {
                    print("Try to jump");
                    this.stateMachine.ChangeState(stateMachine.state_air);
                }

                if (entity.entityMovement.groundChecker.grounded)
                {
                    if (entity.fighterInput.movement.x != 0)
                    {
                        if (stateMachine.currentState != stateMachine.state_walk)
                        {
                            this.stateMachine.ChangeState(stateMachine.state_walk);
                        }
                    }
                    else
                    {
                        if (stateMachine.currentState != stateMachine.state_idle)
                        {
                            this.stateMachine.ChangeState(stateMachine.state_idle);
                        }
                    }

                    if (entity.fighterInput.movement.y < -0.5f)
                    {
                    }
                }

                entity.fighterBlockManager.holdingBackBlock = entity.fighterBlockManager.CheckBlock();

                if (entity.fighterBlockManager.holdingBackBlock && entity.fighterBlockManager.proximityBlocking)
                {
                    entity.fighterBlockManager.PerformBlock();
                }
            }
        }
    }

    public override void State_Exit()
    {
    }
}
