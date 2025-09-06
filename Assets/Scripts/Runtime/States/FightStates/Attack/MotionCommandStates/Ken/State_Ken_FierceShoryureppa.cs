using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_FierceShoryureppa : State_MotionCommand
{
    public FighterState specialMoveLandState;

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterAttackManager.inputtedMotionCommand = false;
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            print("<color=#23f245>Return from fierce shoryureppa</color>");
            entity.entityAnimator.ReturnToNeutral();
        }

        //Don't move if in hitstop
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            if (!entity.entityMovement.disableGroundCheck && entity.entityMovement.groundChecker.grounded && timeVelocityCurveY >= 1.35f)
            {
                stateMachine.ChangeState(specialMoveLandState);
            }
        }
    }

    public override void State_Exit()
    {
        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(null);

        entity.entityMovement.movement.canDoMotionCurveVelocityX = false;
        entity.entityMovement.movement.canDoMotionCurveVelocityY = false;

        base.State_Exit();
    }
}
