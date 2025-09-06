using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Idle : State_Grounded
{
    public override void State_Enter()
    {
        entity.entityAnimator.PlayAnimation("Idle");
    }

    public override void State_Update()
    {
        base.State_Update();
    }

    public override void State_Exit()
    {
    }
}
