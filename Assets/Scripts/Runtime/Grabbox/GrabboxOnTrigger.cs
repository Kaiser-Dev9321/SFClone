using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabboxOnTrigger : MonoBehaviour
{
    //The grab data, damage, effects and hitstun a grab should do
    public GrabData grabData;

    protected EntityScript entity;
    protected EntityScript attacker_Entity;

    protected EntityAudio attacker_EntityAudio;
    protected InstanceAudio attacker_InstanceAudioPrefab;

    public bool useEntityAudio;
    public bool useInstanceAudio;

    protected int currentHitLayer = 0;
    protected int lastHitLayer = 0;

    [HideInInspector]
    public bool hasGrabbed;

    public void OnDisable()
    {
        lastHitLayer = 0;
        hasGrabbed = false;
    }

    public void ActivateEntityComponents()
    {
        attacker_Entity.fighterGrabManager.fighterGrabbed = entity.transform;
    }

    public void GetEntityComponents(Collider2D collider)
    {
        entity = collider.GetComponentInParent<EntityScript>();

        if (currentHitLayer != lastHitLayer)
        {
            if (entity != null)
            {
                ActivateEntityComponents();
            }
            else
            {
                Debug.LogWarning("Entity is null");
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

    public void PlayInstanceAudio(InstanceAudio instanceAudio)
    {
        instanceAudio.instanceAudio.Play();
    }

    public void OnTriggerEnter2D(Collider2D collider) //This should be just hurtboxes, and only damage one at a time
    {
        if (collider.gameObject.layer == 11) //Throwbox layer, also should check if grounded
        {
            if (!attacker_Entity)
            {
                attacker_Entity = GetComponentInParent<EntityScript>();
            }

            currentHitLayer = collider.gameObject.layer;

            GetEntityComponents(collider);

            hasGrabbed = true;
        }
    }
}
