using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Ken stateMachine_Ken;

    public override void Start()
    {
        base.Start();

        stateMachine_Ken = GetComponentInParent<StateMachine_Ken>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick)
        {
            stateMachine_Ken.ChangeState(stateMachine_Ken.state_Grab);
        }
    }
}
