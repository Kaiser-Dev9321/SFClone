using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_SuperFreeze : State_MotionCommand
{
    public FighterState superState;

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

        //entity.fighterSuperManager.DisableSuperState(0);

        print("Super freeze start");
    }

    public override void State_Update()
    {
        if (!entity.fighterSuperManager.inSuperFreeze)
        {
            print($"Not in super freeze any more: {superState}");
            entity.stateMachine.ChangeState(superState);
        }
    }

    public override void State_Exit()
    {
    }
}
