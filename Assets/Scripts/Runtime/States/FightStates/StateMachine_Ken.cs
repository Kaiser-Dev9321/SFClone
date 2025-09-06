using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine_Ken : StateMachine_Entity, IEntityStateMachine
{
    [Space]
    public State_Ken_HeavyKick2 TC1_HeavyKick2;

    [Space]
    public State_Ken_HeavyKick3 TC2_HeavyKick3;

    public override void Start() //For beginning state transitions
    {
        base.Start();
        this.ChangeState(state_idle);
    }
}
