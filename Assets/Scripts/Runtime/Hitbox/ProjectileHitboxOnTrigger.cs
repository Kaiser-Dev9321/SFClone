using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHitboxOnTrigger : HitboxOnTrigger
{
    [SerializeField]
    private ProjectileManager projectileManager;

    public GameObject audioPrefab;
    public bool isALightHitbox;
    public bool isAHeavyHitbox;
    public bool isAFierceHitbox;

    public bool playInstanceAudioOnTrigger;
    
    /*
    public void SetAttackerClass(EntityScript newEntity)
    {
        attacker_Entity = newEntity;
    }
    */

    public void SetAttackComboManager(FighterComboManager newFighterComboManager)
    {
        attacker_ComboManager = newFighterComboManager;
    }

    protected override void LoadOnEnable()
    {
        if (useEntityAudio)
        {
            PlayEntityAudio();
        }

        if (useInstanceAudio)
        {
            PlayInstanceAudio(instanceAudioToPlay.GetComponent<InstanceAudio>());
        }
    }

    protected override void LoadAttackerEntity()
    {
        print("Should load attacker entity");

        attacker_Entity = projectileManager.attacker_Entity;
        UseWhiffOnHitbox();
    }

    private void Awake()
    {
        projectileManager.LoadProjectileReferencesEvent += LoadOnEnable;
        projectileManager.LoadProjectileReferencesEvent += LoadAttackerEntity;
    }

    private void OnDestroy()
    {
        projectileManager.LoadProjectileReferencesEvent -= LoadOnEnable;
        projectileManager.LoadProjectileReferencesEvent -= LoadAttackerEntity;
    }

    public void UseEntityAudio()
    {
        attacker_EntityAudio = defender_Entity.entityAudio;

        if (isALightHitbox)
        {
            TriggerPlayAudio(1);
        }

        if (isAHeavyHitbox)
        {
            TriggerPlayAudio(2);
        }

        if (isAFierceHitbox)
        {
            TriggerPlayAudio(3);
        }
    }

    public void UseInstanceAudio()
    {
        GetInstanceAudio();
        PlayInstanceAudio(attacker_InstanceAudioPrefab);
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        //TODO: This hitbox trigger is becoming an if burger, needs to be separated

        //There is no attacker so return
        if (!attacker_Entity)
        {
            print("No projectile attacker");

            return;
        }

        if (collider.gameObject.layer == 7 && collider.gameObject.GetComponentInParent<EntityScript>() != attacker_Entity)
        {
            base.OnTriggerEnter2D(collider);

            //print($"Attacker entity: {attacker_Entity}");
            //print($"Attack effect data: {attackEffectData}");

            if (!attacker_Entity.fighterJuggleStateManager.CheckJuggleAboveMaximum(attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount, attackEffectData.juggle_Potential) && currentHitLayer != lastHitLayer)
            {
                AssignCurrentAttackEffectData(attackEffectData, attackEffectData_stunMidair);
                projectileManager.SubtractHitsLeft(1);

                GetEntityComponents(collider);

                if (!stateMachine)
                {
                    stateMachine = defender_Entity.GetComponent<StateMachine_Entity>();
                }

                if (useEntityAudio)
                {
                    UseEntityAudio();
                }

                if (useInstanceAudio && playInstanceAudioOnTrigger)
                {
                    UseInstanceAudio();
                }

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
                            //print($"<color=#298333>Projectile juggle: {attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount}</color>");
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
