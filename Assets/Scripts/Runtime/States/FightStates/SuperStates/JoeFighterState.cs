using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoeFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Joe stateMachine_Joe;

    public override void Start()
    {
        base.Start();

        stateMachine_Joe = GetComponentInParent<StateMachine_Joe>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Joe.ChangeState(stateMachine_Joe.state_Grab);
        }
    }
}
