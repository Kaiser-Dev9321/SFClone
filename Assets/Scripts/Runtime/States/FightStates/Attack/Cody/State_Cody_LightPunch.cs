using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_LightPunch : AttackFighterState
{
    public override void Start()
    {
        base.Start();
    }

    public override void State_Enter()
    {
        //TODO: All states need base enter now

        base.State_Enter();

        entity.fighterInput.button_lightPunch = false;

        entity.entityMovement.movement.velocity.x = 0;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
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
    }
}
