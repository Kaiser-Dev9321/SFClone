using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityHitstop : MonoBehaviour
{
    [HideInInspector]
    public bool hitStopped = false;

    //Used for when hitting another entity
    [HideInInspector]
    protected EntityScript entity;

    protected StateMachine_Entity entityStateMachine;

    public float hitstopTime;
    //This entity's hitstop data
    public HitstopData hitstopData;

    public GameObject hitstunnedParticles;
    public UnityEvent onEntityIsHitstopped;

    private void Start()
    {
        entity = GetComponent<EntityScript>();
        entityStateMachine = GetComponent<StateMachine_Entity>();
    }

    private void Update()
    {
        if (hitstopTime > 0)
        {
            hitstopTime -= Time.deltaTime;
        }
        else if (hitstopTime < 0)
        {
            hitStopped = false;
            hitstopTime = 0;

            entity.entityMovement.DisableMovement(0);

            if (!entity.entityMovement.overrideDisableWallPushback)
            {
                entity.entityMovement.DisableWallPushback(0);
            }

            entity.entityAnimator.animator.speed = 1;
        }
    }

    public void DoAttackerHitstop()
    {
        hitstopTime = hitstopData.hitstop_milliseconds;
        hitStopped = true;

        entity.entityMovement.DisableMovement(1);
        entity.entityMovement.DisableWallPushback(1);
        entity.entityAnimator.animator.speed = 0;
    }

    public void DoDefenderHitstop()
    {
        hitstopTime = hitstopData.hitstop_milliseconds;
        hitStopped = true;

        //print($"Defender: {transform.name}");

        //Instantiate(hitstunnedParticles, transform.position, Quaternion.identity);

        entity.entityMovement.DisableMovement(1);
        entity.entityMovement.DisableWallPushback(1);
        entity.entityAnimator.animator.speed = 0;

        onEntityIsHitstopped.Invoke();
    }
}
