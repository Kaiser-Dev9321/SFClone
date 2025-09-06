using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWallTrigger : MonoBehaviour
{
    public Collider2D collisionWallCollider;

    public List<EntityScript> triggeredWallEntities;

    public bool isLeftWall;

    //TODO: See how crossup stuff works in here instead, new position overrides the translate, so either move it in here, or do some hacky stuff with position in the original script

    private EntityScript CheckIfEntityExistsInList(EntityScript entityChecking)
    {
        foreach (var entity in triggeredWallEntities)
        {
            if (entity == entityChecking)
            {
                return entity;
            }
        }

        return null;
    }

    private void DebugAutocorrect(EntityScript entity, bool isLeft)
    {
        if (isLeft)
        {
            print($"<b><color=#f0f23><size=14>Auto-correct entity [Push Left]: {entity}</size></color></b>");
        }
        else
        {
            print($"<b><color=#f0f23><size=14>Auto-correct entity [Push Right]: {entity}</size></color></b>");
        }
    }

    private void ClipEntitiesOut_Left()
    {
        foreach (EntityScript entity in triggeredWallEntities)
        {
            if (!triggeredWallEntities.Contains(entity))
            {
                return;
            }

            print("No vel 0 left");

            entity.entityMovement.movement.NewVelocityX(0);

            float colliderSizeX = entity.entityMovement.collisionChecker.boxCollider.size.x / 2;
            float wallColliderX = collisionWallCollider.bounds.min.x;

            //DebugAutocorrect(entity, true);

            entity.entityMovement.movement.NewPosition(new Vector3((wallColliderX - colliderSizeX) - entity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x, entity.transform.position.y, 0));
        }
    }

    private void ClipEntitiesOut_Right()
    {
        foreach (EntityScript entity in triggeredWallEntities)
        {
            if (!triggeredWallEntities.Contains(entity))
            {
                return;
            }

            print("No vel 0 right");

            entity.entityMovement.movement.NewVelocityX(0);

            float colliderSizeX = entity.entityMovement.collisionChecker.boxCollider.size.x / 2;
            float wallColliderX = collisionWallCollider.bounds.max.x;

            //DebugAutocorrect(entity, false);

            entity.entityMovement.movement.NewPosition(new Vector3((wallColliderX + colliderSizeX) + entity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x, entity.transform.position.y, 0));
        }
    }

    private void RemoveEntitiesIfOut()
    {
        foreach (EntityScript entity in triggeredWallEntities)
        {
            if (!triggeredWallEntities.Contains(entity))
            {
                return;
            }

            if (!collisionWallCollider.IsTouching(entity.entityMovement.collisionChecker.boxCollider))
            {
                print("Exit from wall");
                triggeredWallEntities.Remove(entity);
            }
        }
    }

    private void Update()
    {
        //Move collider outside of collider bounds, check if left wall or right wall, entities' position cannot exceed the bounds' position

        if (!isLeftWall)
        {
            ClipEntitiesOut_Left();
        }
        else
        {
            ClipEntitiesOut_Right();
        }

        RemoveEntitiesIfOut();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ColliderTransform"))
        {
            EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

            if (!triggeredWallEntities.Contains(entityToCheck))
            {
                entityToCheck.entityMovement.collidingAgainstWall = true;

                triggeredWallEntities.Add(entityToCheck);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("ColliderTransform"))
        {
            EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

            if (triggeredWallEntities.Contains(entityToCheck))
            {
                entityToCheck.entityMovement.collidingAgainstWall = false;

                triggeredWallEntities.Remove(entityToCheck);
            }
        }
    }
}
