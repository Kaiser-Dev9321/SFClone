using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Ken_Grab_HellWheel : KenFighterState
{
    //This would transition into this state assuming that the defender transform is already sets
    public Transform grabAnimationEmptyTransform;

    private float grabbedTime;

    private bool hellWheel_Hitstunned = false;

    public AttackEffectData air_AttackEffectData;

    public override void State_Enter()
    {
        hellWheel_Hitstunned = false;

        entity.fighterGrabManager.AttackerGrabDefender(entity.transform, grabAnimationEmptyTransform);

        grabbedTime = Time.time;
    }

    public override void State_Update()
    {
        float currentTime = Time.time;

        //TODO: Spins around backwards and throws the opponent
        entity.entityAnimator.PlayAnimation("HellWheel");



        if (currentTime - grabbedTime > 0.92f && !hellWheel_Hitstunned)
        {
            hellWheel_Hitstunned = true;

            EntityScript grabbedEntity = entity.fighterGrabManager.fighterGrabbed.GetComponent<EntityScript>();
            grabbedEntity.entityHitstun.ActivateHitstun(air_AttackEffectData.effect_Hitstun, true);

            grabbedEntity.entityHitstun.hitStunned = true;
        }

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
