using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_LightPunch : AttackFighterState
{
    private StateMachine_Mike stateMachine_Mike;

    public override void Start()
    {
        base.Start();

        stateMachine_Mike = GetComponentInParent<StateMachine_Mike>();
    }

    public override void State_Enter()
    {
        //TODO: All states need base enter now

        base.State_Enter();

        entity.fighterInput.button_lightPunch = false;

        entity.entityMovement.movement.velocity.x = 0;

        //TODO: All attacks must start at layer -1 and 0 normalised time

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
    }

    public override void State_Update()
    {
        //TODO: All states need base update now

        //TODO: Should override chain cancelling for now, though I should make forward + lp so that it does not interfere
        ButtonNormalsInTargetCombo(attackData, stateMachine_Mike.TC1_LightPunch2, attackData.targetComboData[0]);

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
