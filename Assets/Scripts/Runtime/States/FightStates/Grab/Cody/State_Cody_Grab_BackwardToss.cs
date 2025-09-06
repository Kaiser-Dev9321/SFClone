using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_Grab_BackwardToss : CodyFighterState
{
    public GrabboxOnTrigger grabboxOnTrigger;

    [Space]
    public AttackEffectData grabAttackEffectData;
    public HitstunData grabHitstunData;

    //This would transition into this state assuming that the defender transform is already sets
    public Transform grabAnimationEmptyTransform;

    public Timer grabTimer;

    private bool grab_HitStunned = false;

    [Space(10)]
    [Header("Air attack data")]
    public AttackEffectData air_AttackEffectData;
    public AirRecoveryData air_airRecoveryData;

    private void Awake()
    {
        grabTimer = new Timer(0, grabboxOnTrigger.grabData.grabTimer);
    }

    public override void State_Enter()
    {
        grab_HitStunned = false;

        grabTimer.SetToTimer();
        entity.fighterGrabManager.AttackerGrabDefender(entity.transform, grabAnimationEmptyTransform);
        entity.fighterComboManager.AssignCurrentPerformedAttackEffectData(grabAttackEffectData);
        entity.fighterComboManager.attacked_Entity.entityHitstun.ActivateGroundHitstun(grabHitstunData);
        entity.fighterComboManager.attacked_Entity.entityHitstun.hitStunned = true;

        if (entity.GetXDirection() == 1)
        {
            entity.FlipXDirection(true);
            entity.fighterComboManager.attacked_Entity.FlipXDirection(false);
        }
        else
        {
            entity.FlipXDirection(false);
            entity.fighterComboManager.attacked_Entity.FlipXDirection(true);
        }

        entity.fighterComboManager.attacked_Entity.transform.localScale = new Vector3(-entity.fighterComboManager.attacked_Entity.GetXDirection(), entity.fighterComboManager.attacked_Entity.transform.localScale.y, entity.fighterComboManager.attacked_Entity.transform.localScale.z);

        entity.entityMovement.movement.velocity.y = 0;

        entity.entityAnimator.PlayAnimation("ForwardToss");
    }

    public override void State_Update()
    {
        if (!entity.fighterGameplayManager.freezeStopManager.freezeStopped)
        {
            grabTimer.currentTime -= Time.deltaTime;
        }

        grabTimer.currentTime = Mathf.Clamp(grabTimer.currentTime, 0f, grabTimer.currentTimer);

        if (grabTimer.currentTime == 0 && !grab_HitStunned)
        {
            grab_HitStunned = true;

            entity.fighterJuggleStateManager.SimpleSetJuggle(entity, air_AttackEffectData);

            EntityScript grabbedEntity = entity.fighterGrabManager.fighterGrabbed.GetComponent<EntityScript>();
            grabbedEntity.entityHitstun.ActivateHitstun(air_AttackEffectData.effect_Hitstun, true, air_airRecoveryData);

            entity.fighterGrabManager.fighterGrabbed = null;
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
