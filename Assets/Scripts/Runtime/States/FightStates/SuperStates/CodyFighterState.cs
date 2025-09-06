using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodyFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Cody stateMachine_Cody;

    public override void Start()
    {
        base.Start();

        stateMachine_Cody = GetComponentInParent<StateMachine_Cody>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Cody.ChangeState(stateMachine_Cody.state_Grab);
        }
    }
}
