using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FierceHitboxOnTrigger : HitboxOnTrigger
{
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 7 && collider.gameObject.GetComponentInParent<EntityScript>() != attacker_Entity)
        {
            base.OnTriggerEnter2D(collider);

            if (!attacker_Entity.fighterJuggleStateManager.CheckJuggleAboveMaximum(attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount, attackEffectData.juggle_Potential) && currentHitLayer != lastHitLayer)
            {
                AssignCurrentAttackEffectData(attackEffectData, attackEffectData_stunMidair);

                GetEntityComponents(collider);

                if (defender_Entity.entityHitstun.hitStunned && !hitIfDefenderIsInHitStun && !attacker_Entity.fighterComboManager.currentAttackHit)
                {
                    return;
                }

                if (referencePreviousHitbox)
                {
                    if (previousHitboxToReference.previouslyHit)
                    {
                        //print("Last hitbox hit, ignore this hitbox")
                        return;
                    }
                }

                GetEntityAudio();

                TriggerPlayAudio(3);

                if (!defender_Entity.fighterBlockManager.holdingBackBlock)
                {
                    attacker_Entity.fighterSuperManager.GainMeter(attackEffectData.effect_meterGainOnHit);

                    if (attacker_Entity.fighterJuggleStateManager.CanJuggle(attackEffectData, stateMachine))
                    {
                        if (attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount <= 0)
                        {
                            if (attackEffectData.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                            {
                                return;
                            }

                            attacker_Entity.fighterJuggleStateManager.SetStartJuggle(attackEffectData.juggle_Start);
                        }
                        else
                        {
                            if (attackEffectData.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                            {
                                return;
                            }

                            attacker_Entity.fighterJuggleStateManager.IncreaseJuggle(attackEffectData.juggle_Increase);
                        }
                    }
                }
                else
                {
                    attacker_Entity.fighterSuperManager.GainMeter(attackEffectData.effect_meterGainOnBlock);
                }
            }
        }
    }
}
