using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_LightSuperFreeze : State_MotionCommand
{
    public FighterState superState;

    [Space]
    public FreezeStopData freezeStopData;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightPunch = false;
        entity.fighterInput.button_heavyPunch = false;
        entity.fighterInput.button_fiercePunch = false;
        entity.fighterInput.button_lightKick = false;
        entity.fighterInput.button_heavyKick = false;
        entity.fighterInput.button_fierceKick = false;

        entity.entityAnimator.PlayAnimation("SuperFreeze");

        entity.entityMovement.movement.velocity.x = 0;

        entity.entityHitstun.superFreezeStopData = freezeStopData;

        entity.fighterSuperManager.UseSuperMeter();

        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(attackData);
    }

    public override void State_Update()
    {
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopped && entity.entityHitstun.afterFreezeStop)
        {
            entity.entityHitstun.afterFreezeStop = false;

            print($"Not in super freeze any more: {superState}");
            entity.stateMachine.ChangeState(superState);
        }
    }

    public override void State_Exit()
    {
    }
}
