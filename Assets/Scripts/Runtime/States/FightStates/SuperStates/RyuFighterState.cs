using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RyuFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Ryu stateMachine_Ryu;

    public override void Start()
    {
        base.Start();

        stateMachine_Ryu = GetComponentInParent<StateMachine_Ryu>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Ryu.ChangeState(stateMachine_Ryu.state_Grab);
        }
    }
}
