using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_SpecialMoveLand : FighterState
{
    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity = Vector2.zero;

        stateMachine.currentAttackStateData = null;

        entity.entityAnimator.PlayAnimation("SpecialMoveLand", -1, 0);
    }

    public override void State_Update()
    {
        if (entity.fighterInput.canAttack && entity.entityMovement.groundChecker.grounded)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        //base.State_Exit();
    }
}
