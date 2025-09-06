using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    //TODO: I was thinking about how my current setup works with using scripts to manually change the transform, unsure of how it would work if I added new variables here for the collision stuff
    //though I should do that later, I should really be finishing up the fighting game aspect, details should come later

    #region Hidden References
    
    [HideInInspector]
    public EntityScript entity;

    public Transform colliderTransform;

    //[HideInInspector]
    public GroundChecker groundChecker;

    //[HideInInspector]
    public OverlapCollisionChecker collisionChecker;

    [HideInInspector]
    public FighterPushEntity fighterPushEntity;
    
    [HideInInspector]
    public Movement2D movement;

    private LayerMask originalCollisionLayerMask;


    [HideInInspector]
    public Vector3 motionCurvePosition;
    #endregion

    #region Public References
    [Header("Variables")]
    public bool disableMovement = false;
    public bool disableGroundCheck = false;
    public bool disableCollisionCheck = false;
    public bool disableWallPushback = false;
    public bool disablePenetrationCheck = false;
    public bool disablePushFighters = false;

    public bool overrideDisableWallPushback, overrideDisablePushFighters = false; //Overrides when other functions enable wall pushback, this doesn't enable those functions if true
    public float wallPushbackEffect, pushbackFightersEffect = 1;

    [Header("Wall variables")]

    public bool collidingAgainstWall = false;
    public bool pushAgainstWallDuringHitstun, pushAginstWallDuringWallClip = false;
    public float amountToPushFromWall;

    [Header("Penetration variables")]

    public float penetrationTranslateX;
    public float penetrationTranslateY;

    [Header("Push variables")]

    public float pushVelocityX;
    #endregion

    private bool entityMovementLoaded;

    private void Awake()
    {
        entity = GetComponent<EntityScript>();
        groundChecker = colliderTransform.GetComponent<GroundChecker>();
        collisionChecker = colliderTransform.GetComponent<OverlapCollisionChecker>();

        movement = GetComponent<Movement2D>();
        fighterPushEntity = GetComponent<FighterPushEntity>();

        originalCollisionLayerMask = collisionChecker.collisionFilter.layerMask;

        entity.entityEventLoader.entityEssentialsLoadedEvent += EntityEventLoader_entityEssentialsLoadedEvent;

        motionCurvePosition = transform.position;
    }

    private void EntityEventLoader_entityEssentialsLoadedEvent()
    {
        entityMovementLoaded = true;
    }

    public void DisableMovement(int disable)
    {
        if (disable == 0)
        {
            disableMovement = false;
        }
        else
        {
            disableMovement = true;
        }
    }

    public void DisableGroundCheck(int disable)
    {
        if (disable == 0)
        {
            disableGroundCheck = false;
        }
        else
        {
            disableGroundCheck = true;
        }
    }

    public void DisableCollisionCheck(int disable)
    {
        if (disable == 0)
        {
            disableCollisionCheck = false;
        }
        else
        {
            disableCollisionCheck = true;
        }
    }

    public void DisablePushFighters(int disable)
    {
        if (disable == 0)
        {
            disablePushFighters = false;
        }
        else
        {
            disablePushFighters = true;
        }
    }

    public void DisableWallPushback(int disable)
    {
        if (disable == 0)
        {
            disableWallPushback = false;
        }
        else
        {
            disableWallPushback = true;
        }
    }

    public void DisablePenetrationCheck(int disable)
    {
        if (disable == 0)
        {
            disablePenetrationCheck = false;
        }
        else
        {
            disablePenetrationCheck = true;
        }
    }

    public void OverrideDisableWallPushback(int disable)
    {
        if (disable == 0)
        {
            overrideDisableWallPushback = false;
        }
        else
        {
            overrideDisableWallPushback = true;
        }
    }

    public void OverrideDisablePushFighters(int disable)
    {
        if (disable == 0)
        {
            overrideDisablePushFighters = false;
        }
        else
        {
            overrideDisableWallPushback = true;
        }
    }

    public void ChangeWallPushbackEffect(float effect)
    {
        wallPushbackEffect = effect;
    }

    public void ChangePushbackFightersEffect(float effect)
    {
        pushbackFightersEffect = effect;
    }

    public void ManuallySetGrounded(int enable)
    {
        if (enable == 0)
        {
            groundChecker.grounded = false;
        }
        else
        {
            groundChecker.grounded = true;
        }
    }

    public void ResetCollisionLayer()
    {
        collisionChecker.collisionFilter.layerMask = originalCollisionLayerMask;
    }

    public void WallCollisionLayerOnly()
    {
        print("Wall only layer");

        LayerMask wallLayer = LayerMask.GetMask("Wall");

        collisionChecker.collisionFilter.layerMask = wallLayer;
    }

    public void NewColliderTransform(Transform newColliderTransformToEnable)
    {
        //Changes colliderTransform, therefore also changing the ground and collision checker and changes some variables to remove bugs

        OverlapCollisionChecker previousColliderChecker = colliderTransform.GetComponent<OverlapCollisionChecker>();
        GroundChecker previousGroundChecker = colliderTransform.GetComponent<GroundChecker>();

        //Considered a new colliderTransform probably
        if (previousColliderChecker != newColliderTransformToEnable.GetComponent<OverlapCollisionChecker>())
        {
            //print($"<size=14>Remove previous touched collider reference: {previousColliderChecker} {newColliderTransformToEnable}</size>");
            previousColliderChecker.colliderTouching = null;
        }

        if (previousGroundChecker != newColliderTransformToEnable.GetComponent<GroundChecker>())
        {
            previousGroundChecker.grounded = false;
        }

        colliderTransform.gameObject.SetActive(false);

        colliderTransform = newColliderTransformToEnable;

        groundChecker = colliderTransform.GetComponent<GroundChecker>();
        collisionChecker = colliderTransform.GetComponent<OverlapCollisionChecker>();

        colliderTransform.gameObject.SetActive(true);
    }

    public void QuickSetAirTransform()
    {
        Transform groundColliderTransform = entity.transform.GetChild(4).GetChild(0);
        Transform airColliderTransform = entity.transform.GetChild(4).GetChild(1);

        groundColliderTransform.gameObject.SetActive(false);
        airColliderTransform.gameObject.SetActive(true);

        colliderTransform = airColliderTransform;
    }

    public void QuickSetGroundTransform()
    {
        Transform groundColliderTransform = entity.transform.GetChild(4).GetChild(0);
        Transform airColliderTransform = entity.transform.GetChild(4).GetChild(1);

        groundColliderTransform.gameObject.SetActive(true);
        airColliderTransform.gameObject.SetActive(false);

        colliderTransform = groundColliderTransform;
    }

    //TODO: Fix the issues, fighters are too small, so it is best to scale them up to fix push stuff

    private void FixedUpdate()
    {
        if (entityMovementLoaded)
        {
            if (!disableGroundCheck)
            {
                groundChecker.GroundCheck();
            }

            if (!disableCollisionCheck)
            {
                collisionChecker.lastPoint = transform.position;

                collisionChecker.CollisionCheck();
            }

            //Experimented with queuing all positioning/velocity to make results more consistent, I removed it since it caused really bad bugs, no matter the different methods I used


            #region Motion curve velocity movement

            if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
            {
                if (movement.movementAssignedPosition)
                {
                    if (movement.canDoMotionCurveTranslateX)
                    {
                        movement.MoveCurveTranslateX_RelativeToEntityDirection(movement.doMotionCurveTranslate.x);
                    }

                    if (movement.canDoMotionCurveTranslateY)
                    {
                        movement.MoveTranslateY2(movement.doMotionCurveTranslate.y);
                    }
                }


                if (movement.canDoMotionCurveVelocityX)
                {
                    movement.NewVelocityX(movement.doMotionCurveVelocity.x);
                }

                if (movement.canDoMotionCurveVelocityY)
                {
                    movement.NewVelocityY(movement.doMotionCurveVelocity.y);
                }
            }

            #endregion

            #region Final movement fixes

            //Collider pushing for when fighters walk into each other
            if (!disablePushFighters)
            {
                transform.position += Vector3.right * (pushVelocityX * pushbackFightersEffect);
                //movement.MoveTranslateX(pushVelocityX * pushbackFightersEffect);
            }

            //Wall pushing for when fighters interact with the wall
            if (!disableWallPushback)
            {
                //Push against wall during hitstun

                if (!pushAginstWallDuringWallClip)
                {
                    if (pushAgainstWallDuringHitstun)
                    {
                        pushAgainstWallDuringHitstun = false;

                        transform.position += Vector3.right * (amountToPushFromWall * wallPushbackEffect);
                        //movement.MoveTranslate(new Vector3(amountToPushFromWall * wallPushbackEffect, 0, 0));
                    }
                    else
                    {
                        amountToPushFromWall = 0;
                    }
                }

                if (pushAginstWallDuringWallClip)
                {

                }

                //Push against wall during clip

            }

            //Collider penetration pushing if their boundaries overlap
            if (!disablePenetrationCheck)
            {
                transform.position += Vector3.right * penetrationTranslateX;
                //movement.MoveTranslate(new Vector2(penetrationTranslateX, penetrationTranslateY));
            }

            //Push fighters when colliding to their bounds (useful during moves)
            movement.ColliderSetBounds();

            #endregion


            //Previiously was at the top
            if (!disableMovement)
            {
                movement.MoveVelocity();
            }

            //After all checks
            motionCurvePosition = transform.position;

            if (movement.shouldAssignPosition && !movement.movementAssignedPosition)
            {
                movement.movementAssignedPosition = true;

                movement.AssignOriginalTransformPosition();
            }
        }
    }

    public void QuickMoveToSnapPoint()
    {
        movement.NewPosition(groundChecker.snapPoint);
    }

    private void OnDrawGizmos()
    {
    }
}
