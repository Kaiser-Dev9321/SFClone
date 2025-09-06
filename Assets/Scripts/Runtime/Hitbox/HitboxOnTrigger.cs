using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

//Type: One-hit hitbox (reset by re-enabling the gameobject)
public class HitboxOnTrigger : MonoBehaviour
{
    //TODO: Hitbox stuff needs cleaning up

    #region Hitbox Variables
    public bool previouslyHit = false;
    public bool thisHitboxHit;

    public bool useWhiffOnThisHitbox = true;
    #endregion

    #region Attack Variables
    [Header("References")]
    //The damage, pushback, hitstop and stun the attack should do
    public AttackEffectData attackEffectData;
    public AttackEffectData attackEffectData_stunMidair;

    protected EntityScript defender_Entity;
    protected EntityScript attacker_Entity;
    protected FighterAttackManager attacker_AttackManager;
    protected FighterComboManager attacker_ComboManager;
    protected EntityMovement defender_EntityMovement;
    protected EntityAudio attacker_EntityAudio;
    protected InstanceAudio attacker_InstanceAudioPrefab;
    //If hitIfDefenderIsInHitstun is true, should it reference a previous hitbox?
    public bool referencePreviousHitbox;
    //Which hitbox to reference
    public HitboxOnTrigger previousHitboxToReference;

    protected StateMachine_Entity stateMachine;

    [Space]
    [Header("Recovery variables")]
    public AirRecoveryData airRecoveryData;

    [Space]
    [Header("Freeze Stop variables")]
    public FreezeStopData blockstopData;
    public FreezeStopData hitstopData;

    [Space]
    [Header("Stun variables")]
    //Use for either multi-hit normals or combos
    public bool hitIfDefenderIsInHitStun = true;
    public BlockstunData blockstunData;

    [Space]
    [Header("Audio variables")]
    public bool useEntityAudio;
    public AudioSource entityAudioToPlay;
    public bool useInstanceAudio;
    public GameObject instanceAudioToPlay;

    protected AttackEffectData currentAttackEffectData;
    protected AttackEffectData currentAttackEffectData_stunMidair;
    #endregion

    #region Effect Variables
    [Space(15)]
    [Header("Effects")]
    public GameObject blockHitEffect;
    public GameObject attackHitEffect;
    #endregion

    protected int currentHitLayer = 0;
    protected int lastHitLayer = 0;

    protected void DebugComboAndVariables(Collider2D collider)
    {
        if (collider.transform)
        {
            if (collider.gameObject.GetComponentInParent<EntityScript>())
            {
                print($"Colliding with: {collider.transform.name} {collider.transform.parent.name} {collider.gameObject.GetComponentInParent<EntityScript>().gameObject.tag} {collider.gameObject.layer}");
                print($"Entity is self: {collider.gameObject.GetComponentInParent<EntityScript>() == attacker_Entity}");
            }
        }
    }

    public void AssignCurrentAttackEffectData(AttackEffectData assignedAttackEffectData, AttackEffectData assignedAttackEffectData_StunMidair)
    {
        currentAttackEffectData = assignedAttackEffectData;
        currentAttackEffectData_stunMidair = assignedAttackEffectData_StunMidair;

        //print($"Current attack effect data: {currentAttackEffectData}");
    }

    public void OnDisable()
    {
        lastHitLayer = 0;

        previouslyHit = thisHitboxHit;
        thisHitboxHit = false;

        if (attacker_Entity)
        {
            if (attacker_Entity.fighterComboManager.currentAttackHit)
            {
                attacker_Entity.fighterComboManager.currentAttackHit = false;
                attacker_Entity.fighterComboManager.lastAttackHit = true;
            }
        }

        AssignCurrentAttackEffectData(null, null);
    }

    protected void UseWhiffOnHitbox()
    {
        if (useWhiffOnThisHitbox)
        {
            //print($"<size=17>Whiff on hitbox: {transform.parent.name}</size>");
            attacker_Entity.fighterSuperManager.GainMeter(attackEffectData.effect_meterGainOnWhiff);
        }
    }

    protected virtual void LoadAttackerEntity()
    {
        attacker_Entity = GetComponentInParent<EntityScript>();
    }

    protected virtual void LoadOnEnable()
    {
        if (!attacker_Entity)
        {
            LoadAttackerEntity();
        }

        UseWhiffOnHitbox();

        if (useEntityAudio)
        {
            PlayEntityAudio();
        }

        if (useInstanceAudio)
        {
            PlayInstanceAudio(instanceAudioToPlay.GetComponent<InstanceAudio>());
        }
    }

    protected void OnEnable()
    {
        //print("On enable load hitbox");

        LoadOnEnable();
    }

    public void Start()
    {
        stateMachine = GetComponentInParent<StateMachine_Entity>();
    }

    public void ActivateAttackHitEffects()
    {
        if (attackHitEffect)
        {
            //print("<size=32><color=#033f24>Attack stun effects</color></size>");

            Collider2D col = GetComponent<Collider2D>();

            GameObject attackEffect = Instantiate(attackHitEffect, col.bounds.center, Quaternion.identity);

            //print($"Entity direction x: {attacker_Entity.GetXDirection()} {attacker_Entity.tag}");

            attackEffect.transform.localScale = new Vector3(attacker_Entity.GetXDirection(), attackHitEffect.transform.localScale.y, attackHitEffect.transform.localScale.z);

            ParticleSystem attackEffectParticles = attackEffect.GetComponent<ParticleSystem>();
            attackEffectParticles.Play();
        }
        else
        {
            Debug.LogWarning("There is no attack hit effect on this hitbox");
        }
    }

    public void ActivateBlockHitEffects()
    {
        if (blockHitEffect)
        {
            //print("<size=32><color=#3031a4>Block stun effects</color></size>");

            Collider2D col = GetComponent<Collider2D>();

            GameObject blockEffect = Instantiate(blockHitEffect, col.bounds.center, Quaternion.identity);

            //print($"Entity direction x: {attacker_Entity.GetXDirection()} {attacker_Entity.tag}");

            blockEffect.transform.localScale = new Vector3(attacker_Entity.GetXDirection(), blockHitEffect.transform.localScale.y, blockHitEffect.transform.localScale.z);

            ParticleSystem blockEffectParticles = blockEffect.GetComponent<ParticleSystem>();
            blockEffectParticles.Play();
        }
        else
        {
            Debug.LogWarning("There is no block hit effect on this hitbox");
        }
    }

    protected void FighterFreezeStopBlockManagement()
    {
        defender_Entity.fighterGameplayManager.freezeStopManager.AssignFreezeStopTime(blockstopData);

        attacker_Entity.fighterGameplayManager.freezeStopManager.DoFreezeStop(attacker_Entity, true);
        defender_Entity.fighterGameplayManager.freezeStopManager.DoFreezeStop(defender_Entity, false);
    }

    protected void FighterFreezeStopHitManagement()
    {
        defender_Entity.fighterGameplayManager.freezeStopManager.AssignFreezeStopTime(hitstopData);

        attacker_Entity.fighterGameplayManager.freezeStopManager.DoFreezeStop(attacker_Entity, true);
        defender_Entity.fighterGameplayManager.freezeStopManager.DoFreezeStop(defender_Entity, false);
    }

    protected void FighterFreezeStopHitManagement2()
    {
        defender_Entity.entityHitstun.hitStunned = true;
        attacker_Entity.fighterComboManager.currentAttackHit = true;

        attacker_ComboManager.AddCombo(defender_Entity);

        int netDamage = attacker_Entity.fighterAttackManager.DamageScaling(defender_Entity.entityHealth.GetCurrentHealth(), currentAttackEffectData.effect_hitDamage, currentAttackEffectData.damageScalingPoints);

        defender_Entity.entityHealth.TakeDamage(netDamage);
    }

    protected void HitManagement()
    {
        if (!defender_Entity.isKnockedDown)
        {
            if (currentAttackEffectData.attackEffectType == AttackEffectType.Hit)
            {
                if (defender_Entity.entityMovement.groundChecker.grounded)
                {
                    //print($"Hit attack connected with the defender on the ground: {defender_Entity.tag} {defender_Entity.collisionsEmpty.transform.GetChild(0).gameObject.activeSelf}");

                    //Ground transform
                    if (!defender_Entity.collisionsEmpty.transform.GetChild(0).gameObject.activeSelf)
                    {
                        //print("Hit attack check ground");
                        defender_Entity.entityMovement.DisableGroundCheck(0);
                    }

                    defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData.effect_Hitstun, false);
                }
                else
                {
                    //print("Hit attack connected with the defender in the air");

                    //Hit attack connected with defender, but this does not juggle
                    if (!currentAttackEffectData.canAirJuggle)
                    {

                        print("Hit in air, no juggle");
                        defender_Entity.entityHitstun.state_airRecovery.airRecoveryData = airRecoveryData;
                        defender_Entity.entityHitstun.state_airRecovery.animationName = airRecoveryData.animationName;
                        defender_Entity.entityHitstun.state_airRecovery.layer = -1;
                        defender_Entity.entityHitstun.state_airRecovery.normalisedTime = 0;

                        defender_Entity.entityHitstun.state_airRecovery.airRecoveryCurveX = airRecoveryData.airRecoveryAnimationCurveX;
                        defender_Entity.entityHitstun.state_airRecovery.airRecoveryCurveY = airRecoveryData.airRecoveryAnimationCurveY;

                        defender_Entity.stateMachine.ChangeState(defender_Entity.entityHitstun.state_airRecovery);
                    }
                    else
                    {
                        defender_Entity.entityHitstun.ActivateHitstun(currentAttackEffectData.effect_Hitstun, true, airRecoveryData, true, "AirFallHitstun");
                    }
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

    public void ActivateEntityComponents()
    {
        attacker_Entity.fighterComboManager.AssignCurrentPerformedAttackEffectData(currentAttackEffectData);

        defender_Entity.fighterComboManager.attacker_Entity = attacker_Entity;

        //print($"<color=#f0f9bc>Inputted blocking: {entity.fighterBlockManager.holdingBackBlock}</color>");

        if (!defender_Entity.fighterBlockManager.holdingBackBlock)
        {
            ActivateAttackHitEffects();

            FighterFreezeStopHitManagement();
            HitManagement();
            FighterFreezeStopHitManagement2();
        }
        else
        {
            //TODO: Blockstop should be here as well

            defender_Entity.fighterBlockManager.hitDuringStun = true;
            defender_Entity.fighterBlockManager.AssignBlockstun(blockstunData);

            FighterFreezeStopBlockManagement();

            ActivateBlockHitEffects();
        }
    }

    public void GetEntityComponents(Collider2D collider)
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

                if (referencePreviousHitbox)
                {
                    if (previousHitboxToReference.previouslyHit)
                    {
                        //print("Last hitbox hit, ignore this hitbox")
                        return;
                    }
                }

                thisHitboxHit = true;
                ActivateEntityComponents();
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

    public void GetEntityAudio()
    {
        attacker_EntityAudio = GetComponentInParent<EntityAudio>();
    }

    public void GetInstanceAudio()
    {
        attacker_InstanceAudioPrefab = GetComponent<InstanceAudio>();
    }

    public void TriggerPlayAudio(int typeOfAudio)
    {
        switch (typeOfAudio) //punch and kick audio
        {
            case 1:
                attacker_EntityAudio.PlayAudio(attacker_EntityAudio.lightHitAudio);
                break;
            case 2:
                attacker_EntityAudio.PlayAudio(attacker_EntityAudio.heavyHitAudio);
                break;
            case 3:
                attacker_EntityAudio.PlayAudio(attacker_EntityAudio.fierceHitAudio);
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }

    public void PlayEntityAudio()
    {
        entityAudioToPlay.Play();
    }

    public void PlayInstanceAudio(InstanceAudio instanceAudio)
    {
        instanceAudio.instanceAudio.Play();
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) //This should be just hurtboxes, and only damage one at a time
    {
        currentHitLayer = collider.gameObject.layer;

        //print($"{collider.gameObject.name} {collider.transform.parent.name} {currentHitLayer}");
    }
}
