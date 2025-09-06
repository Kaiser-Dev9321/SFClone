using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_LightKick : AttackFighterState
{
    private StateMachine_Lee stateMachine_Lee;

    public override void Start()
    {
        base.Start();

        stateMachine_Lee = GetComponentInParent<StateMachine_Lee>();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterInput.button_lightKick = false;

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
