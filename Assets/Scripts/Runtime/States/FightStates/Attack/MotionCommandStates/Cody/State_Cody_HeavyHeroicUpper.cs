using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_HeavyHeroicUpper : State_MotionCommand
{
    private float internalHeroicTimer = 0;

    public override void State_Enter()
    {
        entity.fighterInput.button_heavyPunch = false;

        base.State_Enter();
    }

    public override void State_Update()
    {
        if (entity.fighterGameplayManager.freezeStopManager.freezeStopped)
        {
            internalHeroicTimer += Time.deltaTime;
        }

        if (internalHeroicTimer > 0.4f)
        {
            if (!entity.entityMovement.disableWallPushback)
            {
                entity.entityMovement.DisableWallPushback(1);
            }
        }


        base.State_Update();
    }

    public override void State_Exit()
    {
        internalHeroicTimer = 0;

        base.State_Exit();
    }
}
