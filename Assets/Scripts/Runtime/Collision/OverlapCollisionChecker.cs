using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//TODO: Different ColliderTransforms for standing, jumping, crouching, etc, incorporate into ground and air super states

public class OverlapCollisionChecker : MonoBehaviour
{
    //TODO: Update this for fighters
    public Movement2D movement2D;

    public BoxCollider2D boxCollider;

    public bool drawGizmos = false;

    [Header("Collision Variables")]
    public ContactFilter2D collisionFilter;

    public float colliderDownOffset;

    public BoxCollider2D checkDistanceCollider;

    public Collider2D colliderTouching;
    public Vector2 closestPoint, penetrationVector;
    public Vector3 velocityProjected, normalizedVelocity;

    private RaycastHit2D currentHit;

    private Collider2D checkCol;

    //Test distance between box collider and the closest point
    public Vector2 distanceBetweenColliderAndPoint;

    //Accessible to only EntityMovement
    public Vector3 lastPoint;

    public Vector2 SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms()
    {
        return new Vector2(checkDistanceCollider.size.x - boxCollider.size.x, checkDistanceCollider.size.y - boxCollider.size.y);
    }

    private void DebugLookForCol(Collider2D lookForCol)
    {
        print($"Look for col colliding? {lookForCol != null}");
    }

    private void DebugPenetrationXAmount()
    {
        print($"Penetration X amount set: {movement2D.moveableEntity.tag} {movement2D.moveableEntity.entityMovement.penetrationTranslateX}");
    }

    private void DebugGetColliderHit(RaycastHit2D hit)
    {
        print($"<color=#f2a298> This hit's name is:</color> {hit.transform.name} {hit.transform.tag} {hit.transform.gameObject.layer}");
    }

    public void ResetPenetrationAmount()
    {
        //print("Penetration X amount reset");

        movement2D.moveableEntity.entityMovement.penetrationTranslateX = 0;
    }

    private void ColliderPenetrationCheck()
    {
        EntityScript entity = GetComponentInParent<EntityScript>();

        List<Collider2D> results = new List<Collider2D>();

        int colliderAmount = Physics2D.OverlapBox(checkDistanceCollider.transform.position, checkDistanceCollider.size, 0, collisionFilter, results);

        if (colliderAmount > 0)
        {
            foreach (Collider2D collider in results)
            {
                if (collider.transform.CompareTag("ColliderTransform") && collider.GetComponentInParent<EntityScript>() != entity)
                {
                    EntityScript targetEntity = collider.GetComponentInParent<EntityScript>();

                    //DebugLookForCol(collider);

                    Collider2D targetDistanceCollider = collider.transform.GetChild(0).GetComponent<BoxCollider2D>();

                    //Does it exist and not part of this entity
                    if (targetDistanceCollider)
                    {
                        checkCol = targetDistanceCollider;

                        Debug.DrawLine(targetDistanceCollider.bounds.center + Vector3.up * targetDistanceCollider.bounds.size.y / 2, targetDistanceCollider.bounds.center + Vector3.down * targetDistanceCollider.bounds.size.y / 2, new Color(1, 0.45f, 0.32f, 1));

                        //For when the entity is completely stuck inside another collider
                        ColliderDistance2D colDist = boxCollider.Distance(checkCol);

                        float distanceX = colDist.pointA.x - colDist.pointB.x;
                        float distanceY = colDist.pointA.y - colDist.pointB.y;

                        //Check their distances, move accordingly
                        if (distanceX != 0)
                        {
                            if (boxCollider.bounds.center.x < targetDistanceCollider.bounds.center.x)
                            {
                                movement2D.moveableEntity.entityMovement.penetrationTranslateX = (-distanceX - SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x) * Physics2D.defaultContactOffset * Movement2D.defaultContactOffsetMultiplier;
                            }
                            else
                            {
                                movement2D.moveableEntity.entityMovement.penetrationTranslateX = -1 * (-distanceX - SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x) * Physics2D.defaultContactOffset * Movement2D.defaultContactOffsetMultiplier;
                            }

                            //print($"<color=#328f9c>Current fighter: {entity.tag} {checkDistanceCollider.name}\nCollider result: {targetEntity.tag} {collider.name}</color>");
                            //DebugPenetrationXAmount();
                        }

                        //NOT USED IN THIS GAME.
                        /*
                        if (distanceY != 0)
                        {
                            movement2D.moveableEntity.entityMovement.penetrationTranslateY = (-distanceY - SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().y);
                        }
                        */
                    }
                }
                else
                {
                    checkCol = null;

                    if (movement2D.moveableEntity.entityMovement.penetrationTranslateX != 0)
                    {
                        ResetPenetrationAmount();
                    }

                    /*
                    NOT USED IN THIS GAME.

                    if (movement2D.moveableEntity.entityMovement.penetrationTranslateY != 0)
                    {
                        //movement2D.moveableEntity.entityMovement.penetrationTranslateY = 0;
                    }
                    */
                }
            }
        }
        else
        {
            ResetPenetrationAmount();
        }
    }

    //TODO: Raycast checking that it doesn't just go through an object
    public void RaycastLastPoint()
    {
        Debug.DrawLine(lastPoint, transform.position, new Color(0.05f, 0.9f, 0.14f), 0.05f); //Last point debug
        Debug.DrawRay(transform.position, Vector3.up * 0.2f, new Color(0.18f, 0.9f, 0.003f), 0.05f); //current position checking
    }
    
    public void CollisionCheck() //Check collisions
    {
        //Could improve this by casting a raycast from the last point and checking if in a wall or something like Mario 64

        //Velocity delta amount
        float normalizeX = movement2D.velocity.x * Time.deltaTime;
        float normalizeY = movement2D.velocity.y * Time.deltaTime;

        normalizedVelocity = new Vector3(normalizeX, normalizeY);

        colliderTouching = GetCollider(checkDistanceCollider, normalizedVelocity, SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x, out currentHit);

        if (colliderTouching)
        {
            //print($"Collider touching: {colliderTouching}");

            closestPoint = boxCollider.ClosestPoint(boxCollider.bounds.center + Vector3.down * colliderDownOffset);

            float distanceBetweenColliderAndPointX = boxCollider.bounds.center.x - currentHit.point.x;
            float distanceBetweenColliderAndPointY = boxCollider.bounds.center.y - currentHit.point.y;

            distanceBetweenColliderAndPoint = new Vector2(distanceBetweenColliderAndPointX, distanceBetweenColliderAndPointY);

            //Uses current hit to get direction
            penetrationVector = (currentHit.normal * normalizedVelocity);
            velocityProjected = Vector3.Project(normalizedVelocity, -currentHit.normal);

            //Reverse to fix for left-side collision
            if (distanceBetweenColliderAndPoint.x > 0)
            {
                penetrationVector.x = -penetrationVector.x;
            }

            /*
            NOT USED IN THIS GAME.

            //Reverse to fix for down-side collision
            if (distanceBetweenColliderAndPoint.y > 0)
            {
                penetrationVector.y = -penetrationVector.y;
            }
            */

            Debug.DrawRay(closestPoint + Vector2.up * 0.2f, currentHit.normal * 2.5f, new Color(0.4f, 0.8f, 0.3f));

            movement2D.transform.position += (Vector3)penetrationVector;
            movement2D.velocity -= (Vector2)velocityProjected;

            //Put into a debug function
            //print($"Collider is colliding");

            Debug.DrawRay(closestPoint, velocityProjected * 7, new Color(0.95f, 0.5f + (velocityProjected.magnitude * 0.025f), 0.03f + (velocityProjected.magnitude * 0.0165f)));

            //print("Collision check is done");
        }

        ColliderPenetrationCheck();
    }

    private Collider2D GetCollider(Collider2D moveCollider, Vector2 direction, float distance, out RaycastHit2D hit)
    {
        if (moveCollider != null)
        {
            RaycastHit2D[] hits = new RaycastHit2D[10];

            int numHits = moveCollider.Cast(direction, collisionFilter, hits, distance);

            for (int i = 0; i < numHits; i++)
            {
                //DebugGetColliderHit(hits[i]);

                if (hits[i].collider)
                {
                    //print("Is hitting something");
                    hit = hits[i];
                    return hits[i].collider;
                }
            }
        }

        //print("Is not hitting something");
        hit = new RaycastHit2D();
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawWireCube(boxCollider.bounds.center, new Vector3(boxCollider.size.x, boxCollider.size.y, 0));

            Gizmos.color = new Color(1, 0.3f, 0);
            Gizmos.DrawWireCube(boxCollider.bounds.center, new Vector3(checkDistanceCollider.size.x, checkDistanceCollider.size.y, 0));

            Gizmos.color = new Color(0.3f, 0.9f, 0.4f);
            Gizmos.DrawSphere(boxCollider.bounds.center + Vector3.down * colliderDownOffset, 0.2f);

            Gizmos.color = new Color(1, 0.5f, 0.02f);
            Gizmos.DrawCube(closestPoint, Vector3.one * 0.25f);
        }
    }
}
