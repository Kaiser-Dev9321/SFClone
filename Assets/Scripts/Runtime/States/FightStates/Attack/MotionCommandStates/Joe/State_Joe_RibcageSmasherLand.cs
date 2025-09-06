using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_RibcageSmasherLand : FighterState
{
    public override void State_Enter()
    {
        base.State_Enter();

        print("Joe super land");

        entity.entityAnimator.PlayAnimation("RibcageSmasherLand", -1, 0);
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterInput.canAttack && entity.entityMovement.groundChecker.grounded)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
