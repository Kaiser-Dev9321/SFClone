using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityGuardBoxOnTrigger : MonoBehaviour
{
    //Triggers proximity guard if in range of the proximity guard box

    private EntityScript attacker_Entity;
    private EntityScript attacked_Entity;

    private void Start()
    {
        attacker_Entity = GetComponentInParent<EntityScript>();
    }

    private void OnDisable()
    {
        if (attacked_Entity)
        {
            attacked_Entity.fighterBlockManager.proximityBlocking = false;

            attacked_Entity = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("ColliderTransform") && collider.gameObject.GetComponentInParent<EntityScript>() != attacker_Entity)
        {
            attacked_Entity = collider.GetComponentInParent<EntityScript>();

            if (attacked_Entity)
            {
                if (attacked_Entity.fighterInput.canAttack)
                {
                    //print($"Set the attacked entity proximity blocking to true from: {collider.gameObject.name}");

                    attacked_Entity.fighterBlockManager.proximityBlocking = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("ColliderTransform") && collider.gameObject.GetComponentInParent<EntityScript>() != attacker_Entity)
        {
            attacked_Entity = collider.GetComponentInParent<EntityScript>();

            if (attacked_Entity)
            {
                if (attacked_Entity.fighterInput.canAttack)
                {
                    //print($"Set the attacked entity proximity blocking to false from: {collider.gameObject.name}");

                    attacked_Entity.fighterBlockManager.proximityBlocking = false;
                }
            }
        }
    }
}
