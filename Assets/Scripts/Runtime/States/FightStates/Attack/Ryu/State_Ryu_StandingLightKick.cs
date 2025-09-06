using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ryu_StandingLightKick : CancellableAttackFighterState
{
    [HideInInspector]
    public StateMachine_Ryu stateMachine_Ryu;

    public override void Start()
    {
        base.Start();

        stateMachine_Ryu = GetComponentInParent<StateMachine_Ryu>();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;

        entity.fighterInput.button_lightKick = false;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
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
