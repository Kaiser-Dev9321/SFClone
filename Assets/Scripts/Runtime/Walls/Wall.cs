using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Collider2D wallCollider;
    private Collider2D wallEndCollider;

    public List<EntityScript> collidedWallEntities;

    public bool isLeftWall;

    private EntityScript CheckIfEntityExistsInList(EntityScript entityChecking)
    {
        foreach (var entity in collidedWallEntities)
        {
            if (entity == entityChecking)
            {
                return entity;
            }
        }

        return null;
    }

    private void Start()
    {
        wallCollider = GetComponent<Collider2D>();
        wallEndCollider = GetComponentInChildren<Collider2D>();
    }

    private void PushEntitiesOutLeft()
    {
        foreach (EntityScript entity in collidedWallEntities)
        {
            if (!collidedWallEntities.Contains(entity))
            {
                return;
            }

            //If entity is not facing the wall but is pushed backwards, still collide with wall
            //If entity is facing the wall and performing a special, still collide with wall

            //If entity is not facing the wall and is performing a special, do not collide with the wall
            if (entity.entityDirectionX == -1 && !entity.fighterAttackManager.CanPerformSpecialsAndCanAttack() && !entity.entityHitstun.hitStunned)
            {
                return;
            }


            if (entity.entityMovement.movement.velocity.x >= 0)
            {
                print($"<b><color=#f0f23><size=14>Auto-correct entity: {entity}</size></color></b>");

                float colliderMaxX = entity.entityMovement.collisionChecker.boxCollider.bounds.max.x;
                float wallColliderX = wallCollider.bounds.min.x;

                Debug.DrawLine(new Vector3(colliderMaxX, entity.transform.position.y, 0), new Vector3(wallColliderX, wallCollider.bounds.min.y, 0), Color.cyan);
                Debug.DrawLine(new Vector3(colliderMaxX, entity.transform.position.y, 0), new Vector3(colliderMaxX, entity.transform.position.y + 1.5f, 0), new Color(0.23f, 0.42f, 0.4f));

                entity.entityMovement.pushAgainstWallDuringHitstun = true;
                //entity.entityMovement.amountToPushFromWall = -Mathf.Abs(colliderX - wallColliderX) - entity.entityMovement.collisionChecker.colliderCheckDistance;
                entity.entityMovement.amountToPushFromWall = wallColliderX - (entity.entityMovement.collisionChecker.boxCollider.size.x / 2);

                //entity.entityMovement.wallCollider = wallCollider;

                Debug.DrawLine(new Vector3(wallCollider.bounds.min.x, 4, 0), new Vector3(entity.transform.position.x, 4, 0), Color.grey);
            }
        }
    }

    private void PushEntitiesOutRight()
    {
        foreach (EntityScript entity in collidedWallEntities)
        {
            if (!collidedWallEntities.Contains(entity))
            {
                return;
            }

            //If entity is not facing the wall but is pushed backwards, still collide with wall
            //If entity is facing the wall and performing a special, still collide with wall

            //If entity is not facing the wall and is performing a special, do not collide with the wall
            if (entity.entityDirectionX == 1 && !entity.fighterAttackManager.CanPerformSpecialsAndCanAttack() && !entity.entityHitstun.hitStunned)
            {
                return;
            }

            if (entity.entityMovement.movement.velocity.x <= 0 && entity.fighterAttackManager.CanPerformSpecialsAndCanAttack())
            {
                print($"<b><color=#f0f23><size=14>Auto-correct entity: {entity}</size></color></b>");

                float colliderMinX = entity.entityMovement.collisionChecker.boxCollider.bounds.min.x;
                float wallColliderX = wallCollider.bounds.max.x;

                Debug.DrawLine(new Vector3(colliderMinX, entity.transform.position.y, 0), new Vector3(wallColliderX, wallCollider.bounds.min.y, 0), Color.cyan);
                Debug.DrawLine(new Vector3(colliderMinX, entity.transform.position.y, 0), new Vector3(colliderMinX, entity.transform.position.y + 1.5f, 0), new Color(0.23f, 0.42f, 0.4f));

                entity.entityMovement.pushAgainstWallDuringHitstun = true;
                //entity.entityMovement.amountToPushFromWall = -Mathf.Abs(colliderX - wallColliderX) - entity.entityMovement.collisionChecker.colliderCheckDistance;
                entity.entityMovement.amountToPushFromWall = wallColliderX + (entity.entityMovement.collisionChecker.boxCollider.size.x / 2);

                //entity.entityMovement.wallCollider = wallCollider;

                Debug.DrawLine(new Vector3(wallCollider.bounds.max.x, 4, 0), new Vector3(entity.transform.position.x, 4, 0), Color.grey);
            }
        }
    }

    private void RemoveEntitiesIfOut()
    {
        foreach (EntityScript entity in collidedWallEntities)
        {
            if (!collidedWallEntities.Contains(entity))
            {
                return;
            }

            if (!wallEndCollider.IsTouching(entity.entityMovement.collisionChecker.boxCollider))
            {
                print("Exit from wall");
                collidedWallEntities.Remove(entity);
            }
        }
    }

    private void Update()
    {
        //Move collider outside of bounds, check if left wall or right wall

        if (!isLeftWall)
        {
            PushEntitiesOutLeft();
        }

        if (isLeftWall)
        {
            PushEntitiesOutRight();
        }

        RemoveEntitiesIfOut();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

        if (collider.CompareTag("ColliderTransform"))
        {
            print($"<i><size=18>Checking new wall entity</size></i>");

            if (entityToCheck)
            {
                if (!CheckIfEntityExistsInList(entityToCheck))
                {
                    collidedWallEntities.Add(entityToCheck);
                }
            }
        }
    }

    /*
    private void OnTriggerExit2D(Collider2D collider)
    {
        EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

        if (CheckIfEntityExistsInList(entityToCheck))
        {
            collidedWallEntities.Remove(entityToCheck);
        }
    }
    */
}
