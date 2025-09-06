using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Mike_DuckLowerStraight : AttackFighterState
{
    public bool hitInFront;

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterInput.button_lightPunch = false;
        entity.fighterInput.button_heavyPunch = false;
        entity.fighterInput.button_fiercePunch = false;
        entity.fighterInput.button_lightKick = false;
        entity.fighterInput.button_heavyKick = false;
        entity.fighterInput.button_fierceKick = false;

        if (!hitInFront)
        {
            if (entity.GetXDirection() == 1)
            {
                entity.FlipXDirection(true);
            }
            else
            {
                entity.FlipXDirection(false);
            }
        }

        entity.fighterAttackManager.inputtedMotionCommand = false;

        entity.entityMovement.ResetCollisionLayer();

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation, -1, 0);
    }

    public override void State_Update()
    {
        base.State_Update();

        entity.entityMovement.movement.velocity.x = 0;

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            //When doing flipping on each side of fighters
            entity.FlipXDirection(false);

            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
    }
}
