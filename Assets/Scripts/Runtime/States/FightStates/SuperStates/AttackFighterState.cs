using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterAttackButtons
{
    LightPunch,
    LightKick,
    HeavyPunch,
    HeavyKick,
    FiercePunch,
    FierceKick
}

//TODO: Seems to be kinda useless now
[DefaultExecutionOrder(-100)]
public class AttackFighterState : FighterState
{
    public AttackData attackData; //assigned by each attack state, separated by each fighter

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterBlockManager.ResetBlocking();

        //TODO: This overrides current attack state data, but is useful for target combos, needs to be fixed
        stateMachine.currentAttackStateData = attackData.attackStateData;

        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(attackData);
    }

    public override void State_Update()
    {
        base.State_Update();

        //TODO: Check special cancel attacks for each button, but check only on the event it was pressed

        CheckCancellableAttacks();

        CheckChainCancelAttacks();
    }

    public override void State_Exit()
    {
        base.State_Exit();

        stateMachine.currentAttackStateData = null;

        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(null);
        RegisterLastPerformedAttackToComboManager(attackData);
    }
}
