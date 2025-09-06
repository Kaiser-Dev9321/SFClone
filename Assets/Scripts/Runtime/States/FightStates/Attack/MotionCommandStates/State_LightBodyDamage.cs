using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_LightBodyDamage : FighterState
{
    //TODO: Possibly don't need this state, and others like this

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterAttackManager.inputtedMotionCommand = false;
        entity.entityAnimator.PlayAnimation("LightBodyDamage");
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
