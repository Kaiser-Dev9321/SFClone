using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Blockstun : FighterState
{
    public string blockAnimationName;
    [HideInInspector]
    public AnimationCurve blockStunCurveX;

    [HideInInspector]
    public AnimationCurve blockStunCurveY;

    private float blockStunCurveTime;

    private bool ShouldLeaveBlockstun()
    {
        if (!entity.fighterBlockManager.hitDuringStun)
        {
            if (entity.GetXDirection() == 1 && entity.fighterInput.movement.x == 1)
            {
                return true;
            }

            if (entity.GetXDirection() == -1 && entity.fighterInput.movement.x == -1)
            {
                return true;
            }

            if (entity.fighterInput.movement.y == 1)
            {
                return true;
            }
        }

        return false;
    }

    public override void State_Enter()
    {
        entity.entityAnimator.PlayAnimation("StandingBlock");

        blockStunCurveTime = 0;

        entity.entityMovement.movement.NewVelocityX(0);
    }

    public override void State_Update()
    {
        if (!entity.fighterBlockManager.proximityBlocking && !entity.fighterBlockManager.inBlockStun)
        {
            //print("Out of block stun");

            entity.fighterBlockManager.ResetBlocking();
            entity.entityAnimator.ReturnToNeutral();
        }

        //Not locking you in blockstun as long as you're not hit
        //TODO: Take controllers into account with deadzones as well
        if (ShouldLeaveBlockstun())
        {
            entity.fighterBlockManager.ResetBlocking();
            entity.entityAnimator.ReturnToNeutral();
        }

        if (entity.fighterBlockManager.hitDuringStun)
        {
            //print($"<color=#90009d>Block state: {entity.fighterBlockManager.blockState.ToString()} {entity.fighterBlockManager.inBlockStun}</color>");

            //Each attack is given a value on if they should be blocked mid, high, or low
            if (entity.fighterBlockManager.blockState == BlockStates.HoldingBlock && !entity.fighterBlockManager.inBlockStun)
            {
                //print("Body block stun");
                //Blockstun controlled by animation
                entity.fighterBlockManager.blockState = BlockStates.StandBodyBlocking;

                entity.entityAnimator.PlayAnimation(blockAnimationName, -1, 0);
            }
        }

        blockStunCurveTime += Time.deltaTime;

        entity.entityMovement.movement.NewVelocityX(-entity.GetXDirection() * blockStunCurveX.Evaluate(blockStunCurveTime));
        entity.entityMovement.movement.NewVelocityY(blockStunCurveY.Evaluate(blockStunCurveTime));
    }

    public override void State_Exit()
    {
        entity.fighterBlockManager.proximityBlocking = false;

        if (entity.fighterBlockManager.hitDuringStun)
        {
            entity.fighterBlockManager.hitDuringStun = false;
        }
    }
}
