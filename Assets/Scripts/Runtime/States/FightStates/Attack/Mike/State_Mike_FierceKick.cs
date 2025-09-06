using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_FierceKick : AttackFighterState
{
    private bool assignedPosition;

    public MotionCurveData motionCurveData;

    private float timeVelocityCurveX = 0;


    public override void Start()
    {

        base.Start();
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;

        entity.fighterInput.button_fierceKick = false;

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);

        entity.entityMovement.movement.canDoMotionCurveVelocityX = true;
        timeVelocityCurveX = 0;
    }

    public override void State_Update()
    {
        base.State_Update();

        timeVelocityCurveX += Time.deltaTime;

        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            if (!assignedPosition)
            {
                AssignNewPosition();
            }
        }

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_PhysicsUpdate()
    {
        entity.entityMovement.movement.doMotionCurveVelocity.x = entity.GetXDirection() * motionCurveData.animationCurve_velocityX.Evaluate(timeVelocityCurveX);
    }

    public override void State_Exit()
    {
        base.State_Exit();

        entity.entityMovement.movement.canDoMotionCurveVelocityX = false;
    }
}
