using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine_Lee : StateMachine_Entity, IEntityStateMachine
{
    [Space]
    public AttackFighterState TC1_LightKick2;
    public AttackFighterState TC1_FiercePunch2;

    public override void Start() //For beginning state transitions
    {
        base.Start();
        this.ChangeState(state_idle);
    }
}
