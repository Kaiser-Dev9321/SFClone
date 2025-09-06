using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_MotionCommand : AttackFighterState, IMovementCurves
{
    public string motionCommandID;

    protected bool assignedPosition = false;

    public bool useVelocityX;
    public bool useVelocityY;
    public bool useTranslateX;
    public bool useTranslateY;

    public MotionCurveData motionCurveDataObject;

    public MotionCurveData motionCurveData { get { return motionCurveDataObject; } set { } }
    public AnimationCurve velocityCurveX { get { return motionCurveDataObject.animationCurve_velocityX; } set { } }
    public float timeVelocityCurveX { get; set; }
    public AnimationCurve velocityCurveY { get { return motionCurveDataObject.animationCurve_velocityY; } set { } }
    public float timeVelocityCurveY { get; set; }
    public AnimationCurve translateCurveX { get { return motionCurveDataObject.animationCurve_translateX; } set { } }
    public float timeTranslateCurveX { get; set; }
    public AnimationCurve translateCurveY { get { return motionCurveDataObject.animationCurve_translateY; } set { } }
    public float timeTranslateCurveY { get; set; }

    //TODO: Fix animation stuff minor stuff, disabling performing specials during attack and enabling when its over, etc.

    public override void State_Enter()
    {
        timeVelocityCurveX = 0;
        timeVelocityCurveY = 0;

        timeTranslateCurveX = 0;
        timeTranslateCurveY = 0;

        //TODO: Overrides current attack state data but probably needs replacing as this is clunky
        stateMachine.currentAttackStateData = attackData.attackStateData;

        entity.fighterInput.ChangeAttackState(0);
        entity.entityAnimator.PlayAnimation(stateMachine.currentAttackStateData.stateAnimation, -1, 0);

        entity.entityMovement.movement.canDoMotionCurveVelocityX = useVelocityX;
        entity.entityMovement.movement.canDoMotionCurveVelocityY = useVelocityY;
        entity.entityMovement.movement.canDoMotionCurveTranslateX = useTranslateX;
        entity.entityMovement.movement.canDoMotionCurveTranslateY = useTranslateY;

        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(attackData);

        entity.fighterBlockManager.ResetBlocking();
    }

    public override void State_PhysicsUpdate()
    {
        //Don't move if in hitstop
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            timeVelocityCurveX += Time.fixedDeltaTime;
            timeVelocityCurveY += Time.fixedDeltaTime;

            timeTranslateCurveX += Time.fixedDeltaTime;
            timeTranslateCurveY += Time.fixedDeltaTime;

            //All of the translate stuff was moved to Movement2D


            if (useVelocityY)
            {
                if (motionCurveDataObject)
                {
                    entity.entityMovement.movement.doMotionCurveVelocity.y = motionCurveData.animationCurve_velocityY.Evaluate(timeVelocityCurveY);
                }
            }

            if (useTranslateY)
            {
                if (motionCurveDataObject)
                {
                    entity.entityMovement.movement.doMotionCurveTranslate.y = motionCurveData.animationCurve_translateY.Evaluate(timeTranslateCurveY);
                }
            }

            if (!assignedPosition && stateMachine.currentAttackStateData)
            {
                AssignNewPosition();

                assignedPosition = true;
            }

            if (useVelocityX)
            {
                if (motionCurveDataObject)
                {
                    entity.entityMovement.movement.doMotionCurveVelocity.x = entity.GetXDirection() * motionCurveData.animationCurve_velocityX.Evaluate(timeVelocityCurveX);
                }
            }

            if (useTranslateX)
            {
                if (motionCurveDataObject)
                {
                    entity.entityMovement.movement.doMotionCurveTranslate.x = entity.GetXDirection() * motionCurveData.animationCurve_translateX.Evaluate(timeTranslateCurveX);
                }
            }
        }

        //print($"<color=#9f0439> Move velocity X: {entityMovement.movement.velocity.x} </color>");
        //print($"<color=#82ff94> Move velocity Y: {entityMovement.movement.velocity.y} </color>");

        //print($"<color=#9f0f39> Move Transform X: {translateMovementCurveX.Evaluate(timeTranslateCurveX)} </color>");
        //print($"<color=#82ff94> Move Transform Y: {translateMovementCurveY.Evaluate(timeTranslateCurveY)} </color>");

        CheckCancellableAttacks();

        if (entity.fighterInput.canAttack && entity.entityMovement.groundChecker.grounded)
        {
            entity.fighterAttackManager.inputtedMotionCommand = false;
            //print("Could attack so returned");
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        assignedPosition = false;

        //You can probably move these two to the combo manager and it probably wouldn't matter
        RegisterCurrentPerformedAttackToComboManager(null);

        entity.entityMovement.movement.shouldAssignPosition = false;
        entity.entityMovement.movement.movementAssignedPosition = false;

        entity.entityMovement.movement.canDoMotionCurveTranslateX = false;
        entity.entityMovement.movement.canDoMotionCurveTranslateY = false;
        entity.entityMovement.movement.canDoMotionCurveVelocityX = false;
        entity.entityMovement.movement.canDoMotionCurveVelocityY = false;

        entity.entityMovement.movement.doMotionCurveVelocity = Vector2.zero;
        entity.entityMovement.movement.doMotionCurveTranslate = Vector2.zero;
    }
}
