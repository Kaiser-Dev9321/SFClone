using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_LightPunch : CancellableAttackFighterState
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

        entity.fighterInput.button_lightPunch = false;

        //TODO: Fix for all states
        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }

        ButtonNormalsInTargetCombo(attackData, stateMachine_Ken.TC2_HeavyKick3, attackData.targetComboData[0]);
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
