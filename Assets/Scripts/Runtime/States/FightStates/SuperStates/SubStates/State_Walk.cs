using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Walk : State_Grounded
{
    public override void State_Enter()
    {
        //playerEntityMove.movement.velocity.y = 0;
        entity.entityAnimator.PlayAnimation("Walk");
    }

    public override void State_Update()
    {
        base.State_Update();
    }

    public override void State_Exit()
    {
    }
}
