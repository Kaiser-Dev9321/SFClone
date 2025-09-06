using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabFighterState : FighterState
{
    //For now just use hitstun animations, most viewers and the players are only looking at the grabber

    public GrabboxOnTrigger grabboxOnTrigger;

    public override void State_Enter()
    {
        entity.entityAnimator.PlayAnimation("Grab");
    }

    public override void State_Update()
    {
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
