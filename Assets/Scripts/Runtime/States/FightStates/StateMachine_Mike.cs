using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine_Mike : StateMachine_Entity, IEntityStateMachine
{
    [Space]
    public AttackFighterState TC1_LightPunch2;
    public AttackFighterState TC1_HeavyPunch2;
    public AttackFighterState TC1_FiercePunch2;

    public override void Start() //For beginning state transitions
    {
        base.Start();
        this.ChangeState(state_idle);
    }
}
