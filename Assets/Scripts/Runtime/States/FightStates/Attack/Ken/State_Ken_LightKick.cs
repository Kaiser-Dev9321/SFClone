using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_LightKick : CancellableAttackFighterState
{
    [HideInInspector]
    public StateMachine_Ken stateMachine_Ken;

    public override void Start()
    {
        base.Start();

        stateMachine_Ken = GetComponentInParent<StateMachine_Ken>();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;

        entity.fighterInput.button_lightKick = false;

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
