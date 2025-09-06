using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Pernin_SlowArcThornRecovery : FighterState
{
    public override void Start()
    {
        base.Start();
    }

    public override void State_Enter()
    {
        entity.entityMovement.movement.velocity.x = 0;

        entity.entityAnimator.PlayAnimation("SlowArcThornRecovery", -1, 0);
    }

    public override void State_Update()
    {
        if (entity.entityMovement.movement.velocity.x != 0)
        {
            entity.entityMovement.movement.velocity.x = 0;
        }

        if (entity.fighterAttackManager.canPerformNormals && entity.fighterInput.canAttack)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
    }
}
