using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_HeavyPunch : AttackFighterState
{
    public float closePunchDistance;

    public override void State_Enter()
    {
        base.State_Enter();

        entity.entityMovement.movement.velocity.x = 0;

        //Check distance between the two entities, need a game manager for that

        entity.fighterInput.button_heavyPunch = false;

        float dist = Vector2.Distance(entity.gameManager.fighter1.transform.position, entity.gameManager.fighter2.transform.position);

        //Should be a separate state or use separate attack data
        if (dist < closePunchDistance)
        {
            entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
        }
        else
        {
            //TODO: Fix distance normal state animations

            entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
        }
    }

    public override void State_Update()
    {
        base.State_Update();

        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
