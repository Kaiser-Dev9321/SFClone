using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterPushEntity : MonoBehaviour
{
    //Uses the collision so it can be pushed around
    //[HideInInspector]
    public EntityScript entity; //AKA: Attacked Entity

    //[HideInInspector]
    public EntityScript entityToPush;

    public bool pushingFighter;

    //TODO: Fix entities phasing through each other when attacking

    private void Start()
    {
        entity = GetComponentInParent<EntityScript>();
    }

    public float DistanceBetweenColliderX()
    {
        float entityColliderX = transform.position.x;
        float attackedEntityColliderX = entityToPush.transform.position.x;

        float distanceBetweenEntityX = entityColliderX - attackedEntityColliderX;

        //Needs something for left walls too
        return distanceBetweenEntityX;
    }

    //TODO: Doesn't always push around for whatever reason

    //Push entities around
    private void Update()
    {
        if (!entity.entityMovement.disablePushFighters)
        {
            if (entity.entityMovement.collisionChecker.colliderTouching)
            {
                //Get script from the collider
                entityToPush = entity.entityMovement.collisionChecker.colliderTouching.transform.GetComponentInParent<EntityScript>();

                if (entityToPush)
                {
                    if (!entityToPush.entityMovement.disablePushFighters)
                    {
                        if (entityToPush.entityMovement.pushAgainstWallDuringHitstun || entityToPush.entityMovement.collidingAgainstWall)
                        {
                            //print("Can't push");
                        }
                        else
                        {
                            entityToPush.entityMovement.pushVelocityX = -DistanceBetweenColliderX() * Time.deltaTime;

                            //print($"Pushing: {entityToPush.entityMovement.pushAgainstWall} {entityToPush.entityMovement.collidingAgainstWall} {entityToPush.entityMovement.pushVelocityX}");

                            pushingFighter = true;
                        }
                    }
                    else
                    {
                        entityToPush = null;
                    }
                }
            }
            else
            {
                if (entityToPush && !pushingFighter)
                {
                    //print("Back push vel x");

                    entityToPush.entityMovement.pushVelocityX = 0;

                    entityToPush = null;
                }

                pushingFighter = false;
            }
        }
        else
        {
            entityToPush = null;
            pushingFighter = false;
        }

        if (!entityToPush)
        {
            //print("Back push vel x 2");

            entity.entityMovement.pushVelocityX = 0;
            pushingFighter = false;
        }
    }
}
