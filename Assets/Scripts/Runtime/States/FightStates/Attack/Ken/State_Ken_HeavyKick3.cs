using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_HeavyKick3 : AttackFighterState
{
    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;

        entity.fighterInput.button_heavyKick = false;
        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
    }

    public override void State_Update()
    {
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
