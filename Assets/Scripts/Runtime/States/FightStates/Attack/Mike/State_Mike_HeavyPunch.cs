using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_HeavyPunch : AttackFighterState
{
    public override void Start()
    {
        base.Start();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterInput.button_heavyPunch = false;

        entity.entityMovement.movement.velocity.x = 0;

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
