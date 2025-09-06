using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_CrackKick : State_MotionCommand
{
    //TODO: Fix to use translate y

    public override void State_Enter()
    {
        entity.fighterInput.button_fierceKick = false;
        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
