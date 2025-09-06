using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_TC1_LightPunch2 : AttackFighterState
{
    private StateMachine_Mike stateMachine_Mike;

    public override void Start()
    {
        base.Start();

        stateMachine_Mike = GetComponentInParent<StateMachine_Mike>();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterInput.button_lightPunch = false;

        entity.entityMovement.movement.velocity.x = 0;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
    }

    public override void State_Update()
    {
        ButtonNormalsInTargetCombo(attackData, stateMachine_Mike.TC1_HeavyPunch2, attackData.targetComboData[0]);

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
