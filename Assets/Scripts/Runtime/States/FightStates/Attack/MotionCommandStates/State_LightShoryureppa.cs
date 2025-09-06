using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_LightShoryureppa : FighterState
{
    //TODO: Possibly don't need this state, and others like this

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterAttackManager.inputtedMotionCommand = false;
        entity.entityAnimator.PlayAnimation("LightShoryureppa");
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            print("<color=#23f245>Return from light shoryureppa</color>");
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
