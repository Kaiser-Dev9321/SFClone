using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHitboxOnTrigger : HitboxOnTrigger
{
    public AttackEffectData airAttackEffectData;
    public int normalAudio;


    private void CustomGetEntityComponents(Collider2D collider)
    {
        defender_Entity = collider.GetComponentInParent<EntityScript>();
        defender_EntityMovement = defender_Entity.entityMovement;

        attacker_AttackManager = attacker_Entity.fighterAttackManager;
        attacker_ComboManager = attacker_Entity.fighterComboManager; //To use in other classes

        if (currentHitLayer != lastHitLayer && !thisHitboxHit)
        {
            //Put these ifs in a separate function?
            if (defender_Entity != null)
            {
                if (defender_Entity.entityHitstun.hitStunned && !hitIfDefenderIsInHitStun)
                {
                    //print("Return, opponent was in hitstun");
                    return;
                }

                thisHitboxHit = true;
                CustomActivateEntityComponents();
            }
            else
            {
                Debug.LogWarning("Entity is null");
            }

            if (defender_EntityMovement)
            {
                //entityMovement.movement.NewVelocityX(attackData.attack_hitPushback);
            }

            lastHitLayer = currentHitLayer;
        }
        else
        {
            Debug.LogWarning("Already have the layer");
        }
    }

    private void CustomActivateEntityComponents()
    {
        attacker_Entity.fighterComboManager.AssignCurrentPerformedAttackEffectData(currentAttackEffectData);

        defender_Entity.fighterComboManager.attacker_Entity = attacker_Entity;

        //print($"<color=#f0f9bc>Inputted blocking: {entity.fighterBlockManager.holdingBackBlock}</color>");

        if (!defender_Entity.fighterBlockManager.holdingBackBlock)
        {
            attacker_Entity.fighterSuperManager.GainMeter(attackEffectData.effect_meterGainOnHit);

            ActivateAttackHitEffects();

            FighterFreezeStopHitManagement();
            CustomHitManagement();
            FighterFreezeStopHitManagement2();
        }
        else
        {
            attacker_Entity.fighterSuperManager.GainMeter(attackEffectData.effect_meterGainOnBlock);

            //TODO: Blockstop should be here as well

            defender_Entity.fighterBlockManager.hitDuringStun = true;
            defender_Entity.fighterBlockManager.AssignBlockstun(blockstunData);

            ActivateBlockHitEffects();
        }
    }

    private void CustomHitManagement()
    {
        if (!defender_Entity.isKnockedDown)
        {
            if (currentAttackEffectData.attackEffectType == AttackEffectType.Hit)
            {
                if (defender_Entity.entityMovement.groundChecker.grounded)
                {
                    defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData.effect_Hitstun, false);
                }
                else
                {
                    defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData.effect_Hitstun, true, airRecoveryData, true, airAttackEffectData.effect_Hitstun.animationName);
                }
            }
            else if (currentAttackEffectData.attackEffectType == AttackEffectType.Blow)
            {
                defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData.effect_Hitstun, true, airRecoveryData);
            }
        }
        else
        {
            defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData_stunMidair.effect_Hitstun, true, airRecoveryData);
        }
    }

    public void PerformAttackTypeInGroundOrAir(Collider2D collider, AttackEffectData attackEffectDataToUse, AttackEffectData attackEffectDataToUse_stunMidair)
    {
        if (!attacker_Entity.fighterJuggleStateManager.CheckJuggleAboveMaximum(attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount, attackEffectDataToUse.juggle_Potential) && currentHitLayer != lastHitLayer)
        {
            AssignCurrentAttackEffectData(attackEffectDataToUse, attackEffectData_stunMidair);

            CustomGetEntityComponents(collider);

            if (defender_Entity.entityHitstun.hitStunned && !hitIfDefenderIsInHitStun && !attacker_Entity.fighterComboManager.currentAttackHit)
            {
                return;
            }

            GetEntityAudio();

            TriggerPlayAudio(normalAudio);

            if (attacker_Entity.fighterJuggleStateManager.CanJuggle(attackEffectDataToUse, stateMachine))
            {
                if (attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount <= 0)
                {
                    if (attackEffectDataToUse.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                    {
                        return;
                    }

                    attacker_Entity.fighterJuggleStateManager.SetStartJuggle(attackEffectDataToUse.juggle_Start);
                }
                else
                {
                    if (attackEffectDataToUse.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                    {
                        return;
                    }

                    attacker_Entity.fighterJuggleStateManager.IncreaseJuggle(attackEffectDataToUse.juggle_Increase);
                }
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7)
        {
            base.OnTriggerEnter2D(collider);

            EntityScript colEntity = collider.transform.GetComponentInParent<EntityScript>();

            if (colEntity.entityMovement.groundChecker.grounded)
            {
                //print($"<color=#29f042><i>Ground attack effect hit</i></color>");
                PerformAttackTypeInGroundOrAir(collider, attackEffectData, attackEffectData_stunMidair);
            }
            else
            {
                //print($"<color=#2920ff><i>Air attack effect hit</i></color>");
                PerformAttackTypeInGroundOrAir(collider, airAttackEffectData, attackEffectData_stunMidair);
            }
        }
    }
}
