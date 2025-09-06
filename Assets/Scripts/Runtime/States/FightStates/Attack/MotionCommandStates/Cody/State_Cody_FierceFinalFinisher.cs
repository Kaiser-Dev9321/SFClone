using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Cody_FierceFinalFinisher : FighterState
{
    public AttackData attackData;

    public HitboxOnTrigger lungeHitbox;
    public FighterState skywardKickState;

    [Space]
    public HitstunData groundHitstunWhenSnappedInAir;

    [SerializeField]
    private int attackPatternCount = 0;

    [SerializeField]
    private int currentAttackPattern = 1;

    [SerializeField]
    private bool hitboxDidHit, lungeEnd, snapToOpponent, returnToNeutral = false;

    public override void Start()
    {
        base.Start();
    }

    private bool Super_CheckReturnToNeutral()
    {
        //If it did not hit and lunge is ended
        if (!hitboxDidHit && !lungeHitbox.gameObject.activeInHierarchy && lungeEnd)
        {
            return true;
        }

        //If it hit and attack patterns are complete
        if (attackPatternCount == 4 && entity.fighterAttackManager.CanPerformNormalsAndCanAttack())
        {
            return true;
        }

        //If hitbox did hit but opponent was (probably) blocking
        if (hitboxDidHit && !entity.fighterComboManager.attacked_Entity)
        {
            return true;
        }

        return false;
    }

    private void ApplyEffectsWhenInAir()
    {
        //Clear air juggle so it works
        entity.fighterJuggleStateManager.ResetJuggle();

        entity.fighterComboManager.attacked_Entity.stateMachine.ChangeState(entity.fighterComboManager.attacked_Entity.entityHitstun.state_Hitstun);
        entity.fighterComboManager.attacked_Entity.entityHitstun.ActivateGroundHitstun(groundHitstunWhenSnappedInAir);
    }

    private void AttackPattern1Count()
    {
        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack() && attackPatternCount < 7)
        {
            print("Attack pattern 1");

            entity.entityAnimator.PlayAnimation("LightFinalFinisher_Attacks1", -1, 0);

            attackPatternCount++;

            attackPatternCount = Mathf.Clamp(attackPatternCount, 0, 7);
        }

        if (attackPatternCount >= 7)
        {
            print("Ended attack pattern");

            attackPatternCount = 0;

            currentAttackPattern = 2;
        }
    }

    private void AttackPattern2Count()
    {
        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack() && attackPatternCount < 4)
        {
            print("Attack pattern 2");

            entity.entityAnimator.PlayAnimation("LightFinalFinisher_Attacks2", -1, 0);

            attackPatternCount++;

            attackPatternCount = Mathf.Clamp(attackPatternCount, 0, 4);
        }

        returnToNeutral = Super_CheckReturnToNeutral();

        if (attackPatternCount >= 4)
        {
            print("Skyward kick to finish super");

            //attackPatternCount = 0;

            entity.stateMachine.ChangeState(skywardKickState);
        }
    }

    public override void State_Enter()
    {
        entity.fighterAttackManager.inputtedMotionCommand = false;

        base.State_Enter();
        entity.entityAnimator.PlayAnimation(attackData.attackStateData.stateAnimation);
    }

    public override void State_Update()
    {
        base.State_Update();

        if (!entity.entityMovement.disableWallPushback)
        {
            entity.entityMovement.DisableWallPushback(1);
        }

        //If lunge ended
        if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack() && !lungeEnd)
        {
            lungeEnd = true;
        }

        if (lungeHitbox.thisHitboxHit)
        {
            hitboxDidHit = true;
        }

        if (hitboxDidHit && entity.fighterComboManager.attacked_Entity)
        {
            if (!snapToOpponent)
            {
                if (!entity.fighterComboManager.attacked_Entity.entityMovement.groundChecker.grounded)
                {
                    ApplyEffectsWhenInAir();
                }

                entity.fighterComboManager.attacked_Entity.transform.position = entity.transform.position + (Vector3.right * 2) * entity.GetXDirection();

                snapToOpponent = true;
            }

            if (currentAttackPattern == 1)
            {
                AttackPattern1Count();
            }

            if (currentAttackPattern == 2)
            {
                AttackPattern2Count();
            }
        }
        else
        {
            //If lunge hitbox did not hit
            returnToNeutral = Super_CheckReturnToNeutral();
        }

        if (returnToNeutral)
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        hitboxDidHit = false;
        lungeEnd = false;
        snapToOpponent = false;
        returnToNeutral = false;

        attackPatternCount = 0;
        currentAttackPattern = 1;
    }
}
