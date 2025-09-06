using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_LightPunch2 : CancellableAttackFighterState
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
        entity.fighterInput.button_lightPunch = false;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }

        ButtonNormalsInTargetCombo(attackData, stateMachine.state_groundHeavyKick, attackData.targetComboData[0]);
    }

    public override void State_Exit()
    {
    }
}
