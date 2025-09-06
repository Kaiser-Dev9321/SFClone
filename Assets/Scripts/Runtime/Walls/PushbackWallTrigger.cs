using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushbackWallTrigger : MonoBehaviour
{
    private GameManager gameManager;

    public CollisionWallTrigger collisionWallTrigger;
    public BoxCollider2D pushbackWallTrigger;

    public bool wallCrossup = false;

    private EntityScript entityClosestToWall;

    public List<EntityScript> triggeredPushbackWallEntities;

    private EntityScript CheckIfEntityExistsInList(EntityScript entityChecking)
    {
        foreach (var entity in triggeredPushbackWallEntities)
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
        gameManager = FindObjectOfType<GameManager>();
    }

    /*Fighter distance and the closest wall
     * Left fighter, right wall
     * Right Fighter, left wall
     * ETC
    */
    //TODO: Inverted even if it doesn't make sense really, probably should fix
    private float GetFighterDistanceComparedToNearestWall(EntityScript fighter, bool useLeftWall)
    {
        //Left wall
        if (useLeftWall)
        {
            //print($"{fighter.tag} | Left wall");
            return Mathf.Abs(fighter.transform.position.x - collisionWallTrigger.collisionWallCollider.bounds.max.x);
        }
        //Right wall
        else
        {
            //print($"{fighter.tag} | Right wall");
            return Mathf.Abs(fighter.transform.position.x - collisionWallTrigger.collisionWallCollider.bounds.min.x);
        }
    }

    private void DebugCrossup(string fighter)
    {
        print($"<b>Entity getting crossed up</b>: {fighter}");
    }

    //Slide velocity, slide fighter out of the wall, used for crossups
    private void VelocitySlide(EntityScript entityToPush, bool useLeftWall)
    {
        float fighterDistance = GetFighterDistanceComparedToNearestWall(entityToPush, useLeftWall);
        float baseVelocityPower = Mathf.Pow(30, fighterDistance / 10);

        float finalVelocityX = 10.5f / baseVelocityPower;

        float amountToPushFromWall = -finalVelocityX;

        entityToPush.entityMovement.amountToPushFromWall = amountToPushFromWall * Time.deltaTime;

        //print($"Use left wall? {useLeftWall} Velocity slide: {entityToPush.tag} {entityToPush.entityMovement.amountToPushFromWall}");
    }

    private void BackwardsVelocitySlide(EntityScript entityToPush, bool useLeftWall)
    {
        float fighterDistance = GetFighterDistanceComparedToNearestWall(entityToPush, useLeftWall);
        float amountToPushFromWall = -fighterDistance * Time.deltaTime;

        entityToPush.entityMovement.movement.NewVelocityX(0);
        entityToPush.entityMovement.amountToPushFromWall = amountToPushFromWall;

        //print($"<size=15>Backwards wall slide {amountToPushFromWall}</size>");
    }

    private void PushFighters_CrossupSetVelocity(EntityScript fighter, EntityScript entityToPush, bool isLeft)
    {
        print($"{fighter} Crossup velocity set on: {entityToPush}");

        if (isLeft)
        {            
            fighter.entityMovement.amountToPushFromWall = -(entityToPush.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x * 25 * Time.deltaTime);
        }
        else
        {
            fighter.entityMovement.amountToPushFromWall = (entityToPush.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x * 25 * Time.deltaTime);
        }
    }

    //Pushes based on distance and velocity
    private void PushFighters_PushOutOfWall(EntityScript entity, string fighterTag, bool isLeft)
    {
        float fighterDistance = 0;

        if (fighterTag == "Fighter1")
        {
            fighterDistance = GetFighterDistanceComparedToNearestWall(gameManager.fighter1, !isLeft);
        }
        else if (fighterTag == "Fighter2")
        {
            fighterDistance = GetFighterDistanceComparedToNearestWall(gameManager.fighter2, !isLeft);
        }

        //Pushes away attacking entity so they can't loop the attacked entity in the corners

        EntityScript attacked_Entity = entity.fighterComboManager.attacked_Entity;

        float baseVelocityPower = Mathf.Pow(50, fighterDistance * 0.0675f);
        float baseVelocityX = -1.5f * (20 / baseVelocityPower);

        float fighterDistanceAndVelocity = attacked_Entity.entityMovement.movement.velocity.x * (fighterDistance * 0.05f); //velocity * (distance / length)

        //Right wall
        if (!isLeft)
        {
            float amountToPushFromWall = -entity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x - fighterDistanceAndVelocity;

            if (!entity.fighterInput.canAttack)
            {
                entity.entityMovement.amountToPushFromWall = amountToPushFromWall * Time.deltaTime;
                entity.entityMovement.amountToPushFromWall -= baseVelocityX * Time.deltaTime;
            }
            else
            {
                entity.entityMovement.amountToPushFromWall = -baseVelocityX * Time.deltaTime;
            }
        }
        //Left wall
        else
        {
            float amountToPushFromWall = entity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x + fighterDistanceAndVelocity;

            if (!entity.fighterInput.canAttack)
            {
                entity.entityMovement.amountToPushFromWall = amountToPushFromWall * Time.deltaTime;
                entity.entityMovement.amountToPushFromWall += baseVelocityX * Time.deltaTime;
            }
            else
            {
                entity.entityMovement.amountToPushFromWall = baseVelocityX * Time.deltaTime;
            }
        }

        /*

        print($"<color=#0f9fff>BVP: {baseVelocityPower}</color>");
        print($"<color=#9987f0>BVX: {baseVelocityX}</color>");

        print($"<color=#3a8da4>DIST: {fighterDistance}</color>");

        print($"<color=#f90ff5>A VEL: {attacked_Entity.entityMovement.movement.velocity.x} FDAX: {fighterDistanceAndVelocity}</color>");
        
        print($"<color=#ef4421>Push {fighterTag} from wall: {entity.entityMovement.amountToPushFromWall}</color>");
        */
    }

    //Push entities out of wall in crossup
    private void PushEntityInDirection(EntityScript entityToPush, bool leftWall)
    {
        //Left wall
        if (leftWall)
        {
            if (entityToPush.transform.CompareTag("Fighter1"))
            {
                if (entityToPush.transform.position.x < gameManager.fighter2.transform.position.x)
                {
                    DebugCrossup(gameManager.fighter1.tag);

                    gameManager.fighter1.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_CrossupSetVelocity(gameManager.fighter1, entityToPush, true);
                }
            }
            else if (entityToPush.transform.CompareTag("Fighter2"))
            {
                if (entityToPush.transform.position.x < gameManager.fighter2.transform.position.x)
                {
                    DebugCrossup(gameManager.fighter2.tag);

                    gameManager.fighter2.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_CrossupSetVelocity(gameManager.fighter2, entityToPush, true);
                }
            }
        }
        //Right wall
        else
        {
            if (entityToPush.transform.CompareTag("Fighter1"))
            {
                if (entityToPush.transform.position.x > gameManager.fighter2.transform.position.x)
                {
                    DebugCrossup(gameManager.fighter2.tag);

                    gameManager.fighter2.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_CrossupSetVelocity(gameManager.fighter2, entityToPush, false);
                }
            }
            else if (entityClosestToWall.transform.CompareTag("Fighter2"))
            {
                if (entityClosestToWall.transform.position.x > gameManager.fighter2.transform.position.x)
                {
                    DebugCrossup(gameManager.fighter2.tag);

                    gameManager.fighter1.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_CrossupSetVelocity(gameManager.fighter2, entityToPush, false);
                }
            }
        }
    }

    //Allows for crossup depending on the fighters' collider check transforms
    private void FightersCrossupContest(EntityScript crossupEntity)
    {
        if (crossupEntity.CompareTag("Fighter1"))
        {
            //Invert to reduce code size
            if (!collisionWallTrigger.isLeftWall)
            {
                if (crossupEntity.transform.position.x > gameManager.fighter2.transform.position.x)
                {
                    //Compare collider sizes, only succeeds in crossups if the crossup entity's collider size is bigger
                    if (crossupEntity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x < gameManager.fighter2.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x)
                    {
                        //Push fighter 2 as fighter 1 is crossing up
                        //print($"1 [RIGHT WALL] Move over fighter 2 !!");

                        gameManager.fighter2.entityMovement.pushAgainstWallDuringHitstun = true;
                        //VelocitySlide(gameManager.fighter2, false);
                    }
                    else
                    {
                        //Push fighter 1 as fighter 2 is crossing up
                        //print($"1 [RIGHT WALL] Move over fighter 1 !!");

                        gameManager.fighter1.entityMovement.pushAgainstWallDuringHitstun = true;
                        //BackwardsVelocitySlide(gameManager.fighter1, false);
                    }
                }
            }
            else
            {
                if (crossupEntity.transform.position.x > gameManager.fighter2.transform.position.x)
                {
                    //Compare collider sizes, only succeeds in crossups if the crossup entity's collider size is bigger
                    if (crossupEntity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x < gameManager.fighter2.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x)
                    {
                        //print("1 [LEFT WALL] Pushed in a direction from the right wall");

                        gameManager.fighter2.entityMovement.pushAgainstWallDuringHitstun = true;
                        //VelocitySlide(crossupEntity, true);
                    }
                }
            }
        }

        if (crossupEntity.CompareTag("Fighter2"))
        {
            //Invert to reduce code size
            if (!collisionWallTrigger.isLeftWall)
            {
                if (crossupEntity.transform.position.x > gameManager.fighter1.transform.position.x)
                {
                    //Compare collider sizes, only succeeds in crossups if the crossup entity's collider size is bigger
                    if (crossupEntity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x < gameManager.fighter1.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x)
                    {
                        //Push fighter 1 as fighter 2 is crossing up
                        //print("2 [RIGHT WALL] Move over fighter 1 !!");

                        gameManager.fighter1.entityMovement.pushAgainstWallDuringHitstun = true;
                        //VelocitySlide(gameManager.fighter1, false);
                    }
                    else
                    {
                        //Push fighter 2 as fighter 1 is crossing up
                        //print("2 [RIGHT WALL] Move over fighter 2 !!");

                        gameManager.fighter2.entityMovement.pushAgainstWallDuringHitstun = true;
                        //BackwardsVelocitySlide(gameManager.fighter2, false);
                    }
                }
            }
            else
            {
                if (crossupEntity.transform.position.x > gameManager.fighter2.transform.position.x)
                {
                    //Compare collider sizes, only succeeds in crossups if the crossup entity's collider size is biggers
                    if (crossupEntity.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x < gameManager.fighter1.entityMovement.collisionChecker.SizeDifferenceBetweenColliderAndColliderCheckDistanceTransforms().x)
                    {
                        //print("2 [LEFT WALL] Pushed in a direction from the left wall");

                        gameManager.fighter1.entityMovement.pushAgainstWallDuringHitstun = true;
                        //VelocitySlide(crossupEntity, true);
                    }
                }
            }
        }
    }

    //Push entities out left
    private void PushEntitiesOut_Left()
    {
        foreach (EntityScript entity in triggeredPushbackWallEntities)
        {
            if (!triggeredPushbackWallEntities.Contains(entity))
            {
                entity.entityMovement.amountToPushFromWall = 0;

                return;
            }

            if (entity.GetXDirection() == 1)
            {
                if (entity.fighterComboManager.attacked_Entity)
                {
                    //print($"Push entities out left from the wall: {entity}");

                    entity.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_PushOutOfWall(entity, entity.tag, true);
                }
            }

            PushEntityInDirection(entityClosestToWall, true);
        }
    }

    //Push entities out right
    private void PushEntitiesOut_Right()
    {
        foreach (EntityScript entity in triggeredPushbackWallEntities)
        {
            if (!triggeredPushbackWallEntities.Contains(entity))
            {
                entity.entityMovement.amountToPushFromWall = 0;

                return;
            }

            if (entity.GetXDirection() == -1)
            {
                if (entity.fighterComboManager.attacked_Entity)
                {
                    //print($"Push entities out right from the wall: {entity}");

                    entity.entityMovement.pushAgainstWallDuringHitstun = true;
                    PushFighters_PushOutOfWall(entity, entity.tag, false);
                }
            }


            PushEntityInDirection(entityClosestToWall, false);
        }
    }

    private void RemoveEntitiesIfOut()
    {
        foreach (EntityScript entity in triggeredPushbackWallEntities)
        {
            if (!triggeredPushbackWallEntities.Contains(entity))
            {
                return;
            }

            if (!collisionWallTrigger.collisionWallCollider.IsTouching(entity.entityMovement.collisionChecker.boxCollider))
            {
                print("Exit from wall");
                triggeredPushbackWallEntities.Remove(entity);
            }
        }
    }

    private void UpdateClosestToWall()
    {
        foreach (EntityScript entity in triggeredPushbackWallEntities)
        {
            if (!entityClosestToWall)
            {
                entityClosestToWall = entity;
            }
            else
            {
                if (collisionWallTrigger.isLeftWall)
                {
                    if (entity.transform.position.x < entityClosestToWall.transform.position.x)
                    {
                        if (!entity.entityMovement.groundChecker.grounded)
                        {
                            if (collisionWallTrigger.triggeredWallEntities.Contains(entity))
                            {
                                wallCrossup = true;

                                FightersCrossupContest(entity);
                            }
                            else
                            {
                                wallCrossup = false;
                            }
                        }
                        else
                        {
                            print($"<size=17>New entity closest to left wall</size>");

                            entityClosestToWall = entity;
                        }
                    }
                }

                if (!collisionWallTrigger.isLeftWall)
                {
                    if (entity.transform.position.x > entityClosestToWall.transform.position.x)
                    {
                        if (!entity.entityMovement.groundChecker.grounded)
                        {
                            if (collisionWallTrigger.triggeredWallEntities.Contains(entity))
                            {
                                wallCrossup = true;

                                FightersCrossupContest(entity);
                            }
                            else
                            {
                                wallCrossup = false;
                            }
                        }
                        else
                        {
                            print($"<size=17>New entity closest to right wall</size>");

                            entityClosestToWall = entity;
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        UpdateClosestToWall();

        if (!collisionWallTrigger.isLeftWall)
        {
            PushEntitiesOut_Left();
        }
        else
        {
            PushEntitiesOut_Right();
        }

        //TODO: Revamp this function
        //RemoveEntitiesIfOut();
    }

    //TODO: This function and the CollisionWallTrigger function should only change the list if the functions above are completed, or else it gives errors, it should work like a feedback loop
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("ColliderTransform"))
        {
            EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

            if (!triggeredPushbackWallEntities.Contains(entityToCheck))
            {
                triggeredPushbackWallEntities.Add(entityToCheck);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("ColliderTransform"))
        {
            EntityScript entityToCheck = collider.transform.GetComponentInParent<EntityScript>();

            if (triggeredPushbackWallEntities.Contains(entityToCheck))
            {
                triggeredPushbackWallEntities.Remove(entityToCheck);
            }
        }
    }
}
