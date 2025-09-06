using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ground_Grab : GrabFighterState
{
    public bool hasNeutralGrab;

    public FighterState forwardGrabState;
    public FighterState backwardGrabState;
    public FighterState neutralGrabState;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightPunch = false;
        entity.fighterInput.button_lightKick = false;

        //TODO: Make a function for just setting velocity to static when in attacks/grabs
        entity.entityMovement.movement.velocity.x = 0;

        entity.fighterGrabManager.currentlyLookingForGrabs = true;

        base.State_Enter();
    }

    public override void State_Update()
    {
        //So it only works with a specific grab

        if (grabboxOnTrigger.hasGrabbed)
        {
            if (entity.fighterInput.movement.x * entity.GetXDirection() > 0)
            {
                print("Forward grab");
                stateMachine.ChangeState(forwardGrabState);
            }
            else if (entity.fighterInput.movement.x * entity.GetXDirection() < 0)
            {
                print("Back grab");
                stateMachine.ChangeState(backwardGrabState);
            }
            else if (entity.fighterInput.movement.x == 0 && hasNeutralGrab)
            {
                stateMachine.ChangeState(neutralGrabState);
            }
        }
        else if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        entity.fighterGrabManager.currentlyLookingForGrabs = false;
        base.State_Exit();
    }
}
