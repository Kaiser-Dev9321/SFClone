using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerninFighterState : FighterState
{
    [HideInInspector]
    public StateMachine_Pernin stateMachine_Pernin;

    public override void Start()
    {
        base.Start();

        stateMachine_Pernin = GetComponentInParent<StateMachine_Pernin>();
    }

    public override void CheckGrabs()
    {
        if (entity.fighterInput.button_lightPunch && entity.fighterInput.button_lightKick && entity.fighterInput.movement.x > 0)
        {
            stateMachine_Pernin.ChangeState(stateMachine_Pernin.state_Grab);
        }
    }
}
