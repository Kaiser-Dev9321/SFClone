using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine_Pernin : StateMachine_Entity, IEntityStateMachine
{
    public override void Start() //For beginning state transitions
    {
        base.Start();
        this.ChangeState(state_idle);
    }
}
