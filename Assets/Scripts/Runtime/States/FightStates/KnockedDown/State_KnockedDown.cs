using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_KnockedDown : FighterState
{
    public FighterBasics_Ground fighterBasics_Ground;

    public HitstunData knockdownedHitstunData;

    //TODO: Fighters need new knocked down stuff

    public override void State_Enter()
    {
        entity.entityAnimator.PlayAnimation(knockdownedHitstunData.animationName, -1, 0);

        fighterBasics_Ground.SetGroundColliderTransform(entity);
        fighterBasics_Ground.SetGroundColliderActive(entity, true);
    }

    public override void State_Update()
    {
    }

    public override void State_Exit()
    {
        entity.entityMovement.DisableGroundCheck(0);
    }
}
