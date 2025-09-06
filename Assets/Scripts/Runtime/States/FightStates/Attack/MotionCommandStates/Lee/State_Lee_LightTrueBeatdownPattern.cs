using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_LightTrueBeatdownPattern : FighterState
{
    public AttackData attackData;

    public HitstunData hitstunData;

    StateMachine_Lee stateMachine_Lee;

    public HitboxOnTrigger lungeHitbox;

    private int attackPatternCount = 0;

    private bool lungeEnd, hitboxDidHit, returnToNeutral, snapToOpponent = false;

    public override void Start()
    {
        base.Start();

        stateMachine_Lee = GetComponentInParent<StateMachine_Lee>();
    }

    private bool Lunge_CheckReturnToNeutral()
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

        return false;
    }

    public override void State_Enter()
    {
        base.State_Enter();

        entity.fighterAttackManager.inputtedMotionCommand = false;
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
            entity.fighterAttackManager.canPerformNormals = true;
            entity.fighterInput.canAttack = true;
        }
        else
        {
            returnToNeutral = Lunge_CheckReturnToNeutral();
        }

        //If hitbox hit
        if (hitboxDidHit)
        {
            if (!snapToOpponent)
            {
                snapToOpponent = true;

                EntityScript attacked_Entity = entity.fighterComboManager.attacked_Entity;

                attacked_Entity.transform.position = entity.transform.position + (Vector3.right * 2) * entity.GetXDirection();
                entity.fighterJuggleStateManager.ResetJuggle();
                attacked_Entity.entityHitstun.ActivateGroundHitstun(hitstunData);
            }

            if (entity.fighterAttackManager.CanPerformNormalsAndCanAttack() && attackPatternCount < 4)
            {
                entity.entityAnimator.PlayAnimation("TrueBeatdownPattern_Attacks", -1, 0);

                attackPatternCount++;

                attackPatternCount = Mathf.Clamp(attackPatternCount, 0, 4);
            }

            returnToNeutral = Lunge_CheckReturnToNeutral();
        }

        if (returnToNeutral)
        {
            entity.entityAnimator.ReturnToNeutral();
        }
    }

    public override void State_Exit()
    {
        hitboxDidHit = false;
        returnToNeutral = false;
        snapToOpponent = false;
        lungeEnd = false;

        attackPatternCount = 0;

        entity.entityMovement.DisableWallPushback(0);

        base.State_Exit();
    }
}
