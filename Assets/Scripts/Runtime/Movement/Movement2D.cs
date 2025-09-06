using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [HideInInspector]
    public EntityScript moveableEntity;

    public FighterMovementData movementData;

    public Vector2 velocity;

    public Transform transformToOffset;

    private Vector3 originalPosition;

    private float offsetX;
    private float offsetY;

    public bool shouldAssignPosition = false;
    public bool movementAssignedPosition = false;


    public bool canDoMotionCurveVelocityX, canDoMotionCurveVelocityY, canDoMotionCurveTranslateX, canDoMotionCurveTranslateY;

    public Vector2 doMotionCurveVelocity, doMotionCurveTranslate;

    public static float defaultContactOffsetMultiplier = 4;

    private void Awake()
    {
        moveableEntity = transform.GetComponentInParent<EntityScript>();
    }

    public void NewPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void NewVelocityX(float velocityX)
    {
        velocity.x = velocityX;
    }

    public void NewVelocityX_RelativeToEntityDirection(float velocityX)
    {
        velocity.x = velocityX * moveableEntity.GetXDirection();

        originalPosition.x += velocity.x * Time.fixedDeltaTime;
    }

    public void NewVelocityY(float velocityY)
    {
        velocity.y = velocityY;
    }

    public void MoveTranslate(Vector2 newVector)
    {
       transform.Translate(newVector);
    }

    public void MoveTranslateX(float newX)
    {
        transform.position = transformToOffset.position + new Vector3(newX, 0, 0);
    }

    public void AssignOriginalTransformPosition()
    {
        originalPosition = transformToOffset.position;
    }

    public void ResetOffsets()
    {
        offsetX = 0;
        offsetY = 0;
    }
    
    public void MoveCurveTranslateX_RelativeToEntityDirection(float newX)
    {
        offsetY = transform.position.y - originalPosition.y;

        //Translate y offset
        transform.position = originalPosition + new Vector3(newX * moveableEntity.GetXDirection(), offsetY, 0);

        offsetX = newX;
    }

    public void MoveTranslateY(float newY)
    {
        offsetX = transform.position.x - originalPosition.x;

        transform.position = originalPosition + new Vector3(offsetX, newY, 0);

        offsetY = newY;
    }

    public void MoveTranslateY2(float newY)
    {
        offsetX = transform.position.x - originalPosition.x;

        //Translate x offset
        transform.position = originalPosition + new Vector3(offsetX, newY, 0);

        offsetY = newY;
    }

    public void MoveVelocity()
    {
        transform.Translate(velocity * Time.fixedDeltaTime); 
    }

    public void ColliderSetBounds()
    {
        FighterPushEntity fighterPushEntity = moveableEntity.GetComponent<FighterPushEntity>();

        //Push fighter out from the attacker
        if (fighterPushEntity.entityToPush)
        {
            float newXPosition = 0;

            OverlapCollisionChecker thisCollisionChecker = fighterPushEntity.entity.entityMovement.collisionChecker;
            OverlapCollisionChecker otherCollisionChecker = fighterPushEntity.entityToPush.entityMovement.collisionChecker;

            //Push away the entity being attacked
            if (thisCollisionChecker.boxCollider.bounds.center.x < otherCollisionChecker.boxCollider.bounds.center.x)
            {
                //Behind

                if (otherCollisionChecker.checkDistanceCollider.size.x > thisCollisionChecker.checkDistanceCollider.size.x)
                {
                    //print($"Move {fighterPushEntity.entity.name} {fighterPushEntity.entity.tag} behind {fighterPushEntity.entityToPush.name} {fighterPushEntity.entityToPush.tag}, other collider is bigger");

                    newXPosition = thisCollisionChecker.boxCollider.bounds.center.x + otherCollisionChecker.checkDistanceCollider.size.x + otherCollisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x + Physics2D.defaultContactOffset * defaultContactOffsetMultiplier;
                }
                else
                {
                    //print($"Move {fighterPushEntity.entity.name} {fighterPushEntity.entity.tag} behind {fighterPushEntity.entityToPush.name} {fighterPushEntity.entityToPush.tag}, other collider is smaller");

                    newXPosition = thisCollisionChecker.boxCollider.bounds.center.x + thisCollisionChecker.checkDistanceCollider.size.x + otherCollisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x + Physics2D.defaultContactOffset * defaultContactOffsetMultiplier;
                }

                //Fix distance so they are closer together
                float xDist = Vector3.Distance(new Vector3(newXPosition, fighterPushEntity.entityToPush.transform.position.y, 0), otherCollisionChecker.checkDistanceCollider.transform.position);

                Debug.DrawLine(new Vector3(newXPosition, 4.1f, 0), new Vector3(otherCollisionChecker.checkDistanceCollider.transform.position.x, 4.1f, 0), new Color(0.8f, 0.12f, 0.2f), 0.2f);

                newXPosition = newXPosition - xDist + (Physics.defaultContactOffset * defaultContactOffsetMultiplier);
            }
            else
            {
                //In front of

                if (otherCollisionChecker.checkDistanceCollider.size.x > thisCollisionChecker.checkDistanceCollider.size.x)
                {
                    //print($"Move {fighterPushEntity.entity.name} {fighterPushEntity.entity.tag} in front of {fighterPushEntity.entityToPush.name}, {fighterPushEntity.entityToPush.name} {fighterPushEntity.entityToPush.tag} collider is bigger");

                    newXPosition = (thisCollisionChecker.boxCollider.bounds.center.x - otherCollisionChecker.checkDistanceCollider.size.x - otherCollisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x) - Physics2D.defaultContactOffset * defaultContactOffsetMultiplier;
                }
                else
                {
                    //print($"Move {fighterPushEntity.entity.name} {fighterPushEntity.entity.tag} in front of {fighterPushEntity.entityToPush.name}, {fighterPushEntity.entityToPush.name} {fighterPushEntity.entityToPush.tag} collider is smaller");

                    newXPosition = (thisCollisionChecker.boxCollider.bounds.center.x - thisCollisionChecker.checkDistanceCollider.size.x - thisCollisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x) - Physics2D.defaultContactOffset * defaultContactOffsetMultiplier;
                }

                //Fix distance so they are closer together
                float xDist = Vector3.Distance(new Vector3(newXPosition, fighterPushEntity.entityToPush.transform.position.y, 0), otherCollisionChecker.checkDistanceCollider.transform.position);

                Debug.DrawLine(new Vector3(newXPosition, 4, 0), new Vector3(otherCollisionChecker.checkDistanceCollider.transform.position.x, 4, 0), Color.cyan, 0.2f);

                newXPosition = newXPosition + xDist - (Physics.defaultContactOffset * defaultContactOffsetMultiplier);
            }

            //Move entity away from attacking entity
            if (newXPosition != 0)
            {
                fighterPushEntity.entityToPush.entityMovement.movement.NewPosition(new Vector3(newXPosition, fighterPushEntity.entityToPush.transform.position.y, 0));
            }
        }
    }
}
