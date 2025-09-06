using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_SpecialMoveLand : FighterState
{
    public FighterBasics_Air fighterBasics_Air;

    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity = Vector2.zero;

        stateMachine.currentAttackStateData = null;

        entity.entityAnimator.PlayAnimation("SpecialMoveAirRecovery", -1, 0);
    }

    public override void State_Update()
    {
        fighterBasics_Air.CheckAir_LandedOnGround(entity);

        if (entity.fighterInput.canAttack && entity.entityMovement.groundChecker.grounded)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
    }
}
