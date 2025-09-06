using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_Air : CodyFighterState
{
    public FighterBasics_Air fighterBasics_Air;

    public override void State_Enter()
    {
        base.State_Enter();

        //Clear state data when attack ends
        stateMachine.currentAttackStateData = null;

        entity.CompletelyEnableAttacking();
        entity.fighterMotionInputManager.DisableInputCancels();

        fighterBasics_Air.CheckAir_LandedOnGround(entity);
    }

    public override void State_Update()
    {
        base.State_Update();

        CheckAttacks();
    }

    public override void State_PhysicsUpdate()
    {
        if (entity.entityMovement.groundChecker.grounded && entity.entityMovement.movement.velocity.y < 0)
        {
            entity.entityMovement.movement.velocity.y = 0;

            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.fighterInput.canAttack = true;
            entity.entityAnimator.ReturnToNeutral();
        }
        else
        {
            entity.entityMovement.movement.NewVelocityY(Mathf.Lerp(entity.entityMovement.movement.velocity.y, entity.entityMovement.movement.movementData.fallSpeed, entity.entityMovement.movement.movementData.fallTimeLerp * Time.deltaTime));
        }
    }

    public override void State_Exit()
    {
    }
}
