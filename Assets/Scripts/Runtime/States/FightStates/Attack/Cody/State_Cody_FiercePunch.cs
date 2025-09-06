using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_FiercePunch : AttackFighterState
{
    public float closePunchDistance;

    public override void State_Enter()
    {
        //Check distance between the two entities, need a game manager for that

        base.State_Enter();

        entity.fighterInput.button_fiercePunch = false;

        entity.entityMovement.movement.velocity.x = 0;

        //float dist = Vector2.Distance(entity.gameManager.fighter1.transform.position, entity.gameManager.fighter2.transform.position);

        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
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
    }
}
