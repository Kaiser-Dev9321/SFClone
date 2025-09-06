using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_FierceRibcageSmasher : State_MotionCommand
{
    private bool lookingForNextLand = false;

    public FighterState ribcageSmasherLand;

    public override void State_Enter()
    {
        entity.fighterAttackManager.inputtedMotionCommand = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.entityMovement.disableGroundCheck && !lookingForNextLand)
        {
            lookingForNextLand = true;
            entity.entityMovement.groundChecker.grounded = false;
        }

        if (!entity.entityMovement.disableGroundCheck && entity.entityMovement.groundChecker.grounded && lookingForNextLand)
        {
            stateMachine.ChangeState(ribcageSmasherLand);
        }
    }

    public override void State_Exit()
    {
        lookingForNextLand = false;

        base.State_Exit();
    }
}
