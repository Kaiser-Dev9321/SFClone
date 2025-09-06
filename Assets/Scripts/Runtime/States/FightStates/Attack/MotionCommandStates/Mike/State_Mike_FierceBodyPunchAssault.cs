using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_FierceBodyPunchAssault : FighterState
{
    public AttackData attackData;

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterAttackManager.inputtedMotionCommand = false;
        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
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
