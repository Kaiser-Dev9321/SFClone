using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikeFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Mike stateMachine_Mike;

    public override void Start()
    {
        base.Start();

        stateMachine_Mike = GetComponentInParent<StateMachine_Mike>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Mike.ChangeState(stateMachine_Mike.state_Grab);
        }
    }
}
