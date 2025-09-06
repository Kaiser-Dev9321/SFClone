using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Grabs opponent and attaches them to a transform, animation determines when they are let go through LetGoOfDefender
public class FighterGrabManager : MonoBehaviour
{
    public bool currentlyLookingForGrabs;
    public Transform fighterGrabbed;

    public LayerMask normalLayerMask;
    public LayerMask grabbedLayerMask;

    private Transform grab_AttackerTransform;
    private Transform grab_defenderTransform;

    public void DisableEntityMovement(EntityScript entity, bool disable)
    {
        if (disable)
        {
            entity.entityMovement.collisionChecker.collisionFilter.layerMask = grabbedLayerMask;
            entity.entityMovement.DisableGroundCheck(1);
            entity.entityMovement.DisableMovement(1);
        }
        else
        {
            entity.entityMovement.collisionChecker.collisionFilter.layerMask = normalLayerMask;
            entity.entityMovement.DisableGroundCheck(0);
            entity.entityMovement.DisableMovement(0);
        }
    }

    public void DefenderGrabDamage(AttackEffectData attackEffectData)
    {
        EntityScript defenderEntity = grab_defenderTransform.GetComponent<EntityScript>();

        defenderEntity.entityHealth.TakeDamage(attackEffectData.effect_hitDamage);
    }

    //This disables the attacker's ground checker and changes their collision checker properties to get rid of bugs
    public void AttackerGrabDefender(Transform attackerTransform, Transform grabAnimationEmptyTransform)
    {
        grab_AttackerTransform = attackerTransform;
        grab_defenderTransform = fighterGrabbed;
        
        EntityScript attackerEntity = grab_AttackerTransform.GetComponent<EntityScript>();
        EntityScript defenderEntity = grab_defenderTransform.GetComponent<EntityScript>();

        attackerEntity.fighterComboManager.attacker_Entity = attackerEntity;
        attackerEntity.fighterComboManager.attacked_Entity = defenderEntity;

        defenderEntity.fighterComboManager.attacker_Entity = attackerEntity;

        AttachDefenderToAttacker(grabAnimationEmptyTransform);

        DisableEntityMovement(attackerEntity, true);
    }

    public void EndGrabAnimation()
    {
        EntityScript defenderEntity = grab_defenderTransform.GetComponent<EntityScript>();
        defenderEntity.gameManager.PutFighterBackInFightersEmptyTransform(fighterGrabbed);

        EntityScript attackerEntity = grab_AttackerTransform.GetComponent<EntityScript>();

        grab_defenderTransform.SetParent(defenderEntity.gameManager.fightersEmptyTransform);

        DisableEntityMovement(attackerEntity, false);
        DisableEntityMovement(defenderEntity, false);
    }
    
    public void EndGrabAnimation2()
    {
        EntityScript defenderEntity = grab_defenderTransform.GetComponent<EntityScript>();
        defenderEntity.gameManager.PutFighterBackInFightersEmptyTransform(fighterGrabbed);

        EntityScript attackerEntity = grab_AttackerTransform.GetComponent<EntityScript>();

        grab_defenderTransform.SetParent(defenderEntity.gameManager.fightersEmptyTransform);

        DisableEntityMovement(attackerEntity, false);

        defenderEntity.entityMovement.collisionChecker.collisionFilter.layerMask = normalLayerMask;
        defenderEntity.entityMovement.DisableGroundCheck(1);
        defenderEntity.entityMovement.DisableMovement(0);
    }

    //This disables their ground check, disables movement and changes their collision checker properties to get rid of bugs
    private void AttachDefenderToAttacker(Transform grabAnimationEmptyTransform)
    {
        EntityScript defenderEntity = fighterGrabbed.GetComponent<EntityScript>();

        fighterGrabbed.SetParent(grabAnimationEmptyTransform);
        fighterGrabbed.localPosition = Vector3.zero;

        //Encapsulate somewhere else, same for other repeats
        DisableEntityMovement(defenderEntity, true);
    }
}
