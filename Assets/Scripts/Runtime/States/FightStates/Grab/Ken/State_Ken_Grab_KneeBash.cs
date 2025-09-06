using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_Grab_KneeBash : KenFighterState
{
    //This would transitioned into assuming that the defender transform is already sets
    public Transform grabAnimationEmptyTransform;

    public override void State_Enter()
    {
        entity.fighterGrabManager.AttackerGrabDefender(entity.transform, grabAnimationEmptyTransform);
    }

    public override void State_Update()
    {
        //TODO: Spam to increase grab speed, so the opponent has a chance of escaping
        entity.entityAnimator.PlayAnimation("KneeBash");

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
