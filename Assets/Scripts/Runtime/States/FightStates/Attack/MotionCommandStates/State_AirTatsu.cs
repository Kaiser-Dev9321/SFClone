using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_AirTatsu : State_MotionCommand
{
    protected float airTatsuCooldownTime;
    public float airTatsuCooldownTimer;

    public override void State_Enter()
    {
        base.State_Enter();

        airTatsuCooldownTime = 0;
    }

    private bool WasLastStateGrounded()
    {
        if (entity.stateMachine.previousState.GetType() == typeof(State_Grounded))
        {
            return true;
        }

        if (entity.stateMachine.previousState.GetType() == typeof(State_Idle))
        {
            return true;
        }

        if (entity.stateMachine.previousState.GetType() == typeof(State_Walk))
        {
            return true;
        }

        return false;
    }

    public override void State_Update()
    {
        if (entity.entityMovement.movement.velocity.x * entity.entityDirectionX  < 0)
        {
            entity.entityMovement.movement.velocity.x = Mathf.Lerp(entity.entityMovement.movement.velocity.x, 0, entity.entityMovement.movement.movementData.fallTimeLerp * Time.deltaTime);
        }

        if (airTatsuCooldownTime < airTatsuCooldownTimer)
        {
            entity.entityMovement.movement.velocity.y = Mathf.Lerp(entity.entityMovement.movement.velocity.y, entity.entityMovement.movement.movementData.fallSpeed, entity.entityMovement.movement.movementData.fallTimeLerp * Time.deltaTime);

            airTatsuCooldownTime += Time.deltaTime;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.state_air);
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
