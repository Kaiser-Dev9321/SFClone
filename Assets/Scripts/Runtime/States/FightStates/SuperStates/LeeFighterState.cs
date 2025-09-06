using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeeFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Lee stateMachine_Lee;

    public override void Start()
    {
        base.Start();

        stateMachine_Lee = GetComponentInParent<StateMachine_Lee>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Lee.ChangeState(stateMachine_Lee.state_Grab);
        }
    }
}
