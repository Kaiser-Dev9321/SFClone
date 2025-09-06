using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Joe_AirFierceKick : AttackFighterState
{
    public override void Start()
    {
        base.Start();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterInput.button_fierceKick = false;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.entityMovement.groundChecker.grounded)
        {
            entity.entityAnimator.ReturnToNeutral();
        }

        entity.entityMovement.movement.NewVelocityY(Mathf.Lerp(entity.entityMovement.movement.velocity.y, entity.entityMovement.movement.movementData.fallSpeed, entity.entityMovement.movement.movementData.fallTimeLerp * Time.deltaTime));
    }

    public override void State_Exit()
    {
    }
}
