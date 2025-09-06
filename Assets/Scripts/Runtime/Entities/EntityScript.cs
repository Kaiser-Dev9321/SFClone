using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

//TODO: Grab and hold stuff, supers, command normals, input display (it's in training right now)

public interface IEntity
{
    public EntityMovement entityMovement { get; set; }
    public EntityAudio entityAudio { get; set; }
    public EntityHealth entityHealth { get; set; }

    public EntityHitstun entityHitstun { get; set; }
    public FighterAttackManager fighterAttackManager { get; set; }
    public FighterBlockManager fighterBlockManager { get; set; }

    public void FlipXDirection(bool flip);
    public float GetXDirection();
}

public class EntityScript : MonoBehaviour, IEntity
{
    [Header("Public references")]
    public GameObject spriteEmpty;
    public GameObject collisionsEmpty;
    public GameObject projectileSpawnersEmpty;
    public GameObject moveTransformsEmpty;
    public GameObject grabOpponentAnimationTransformsEmpty;

    #region Hidden References

    [HideInInspector]
    public GameManager gameManager;

    //[HideInInspector]
    public FighterGameplayManager fighterGameplayManager;

    [HideInInspector]
    public EntityEventLoader entityEventLoader;

    //[HideInInspector]
    //Direction of the fighters, that only change on ground
    public int entityDirectionX;

    //Direction of the fighters that changes instantly when that direction has changed
    public int actualDirectionX;

    public EntityMovement entityMovement { get; set; }
    public EntityAudio entityAudio { get; set; }
    public EntityHealth entityHealth { get; set; }
    public EntityHitstun entityHitstun { get; set; }
    public FighterAttackManager fighterAttackManager { get; set; }
    public FighterBlockManager fighterBlockManager { get; set; }

    public string fighterName;

    [HideInInspector]
    public FighterGrabManager fighterGrabManager;

    [HideInInspector]
    public FighterKnockdownManager fighterKnockdownManager;

    [HideInInspector]
    public EntityAnimator entityAnimator;

    public IEntityStateMachine stateMachine;

    [HideInInspector]
    public FighterInput fighterInput;

    [HideInInspector]
    public FighterComboManager fighterComboManager;

    [HideInInspector]
    public FighterListOfStates fighterListOfStates;

    [HideInInspector]
    public FighterMotionInputManager fighterMotionInputManager;

    [HideInInspector]
    public FighterMotionInputCommands fighterMotionInputCommands;

    [HideInInspector]
    public FighterSuperManager fighterSuperManager;

    [HideInInspector]
    public FighterJuggleStateManager fighterJuggleStateManager;

    [HideInInspector]
    public FighterProjectileManager fighterProjectileManager;

    [HideInInspector]
    public RoundManager roundManager;

    [HideInInspector]
    public DamageScaleManager damageScaleManager;
    #endregion

    public bool inPrejump = false;

    [Space]
    public bool onTheGround = false;
    public bool isKnockedDown = false;

    #region Protected References
    #endregion

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        fighterGameplayManager = FindObjectOfType<FighterGameplayManager>();
        entityEventLoader = GetComponent<EntityEventLoader>();

        entityAnimator = GetComponent<EntityAnimator>();
        entityMovement = GetComponent<EntityMovement>();
        entityHealth = GetComponent<EntityHealth>();
        entityHitstun = GetComponent<EntityHitstun>();
        entityAudio = GetComponentInChildren<EntityAudio>();

        fighterInput = GetComponent<FighterInput>();
        fighterComboManager = GetComponent<FighterComboManager>();
        fighterListOfStates = GetComponent<FighterListOfStates>();
        fighterMotionInputManager = GetComponent<FighterMotionInputManager>();
        fighterMotionInputCommands = GetComponent<FighterMotionInputCommands>();
        fighterSuperManager = GetComponent<FighterSuperManager>();
        fighterAttackManager = GetComponent<FighterAttackManager>();
        fighterBlockManager = GetComponent<FighterBlockManager>();
        fighterJuggleStateManager = GetComponent<FighterJuggleStateManager>();
        fighterGrabManager = GetComponent<FighterGrabManager>();
        fighterKnockdownManager = GetComponent<FighterKnockdownManager>();

        entityDirectionX = 1;

        stateMachine = GetComponent<IEntityStateMachine>();
    }

    private void Start()
    {
        entityEventLoader.OnEntityLoaded();
    }

    public void LoadEntityReferences()
    {
        roundManager = UnityEngine.Object.FindObjectOfType<RoundManager>();
        damageScaleManager = UnityEngine.Object.FindObjectOfType<DamageScaleManager>();

        entityHealth.LoadHealthEssentials();
        //Load entity essentials from there
        entityEventLoader.OnEntityEssentialsLoaded();
    }

    public void FlipXDirection(bool flip)
    {
        float currentSpriteScaleX = Mathf.Abs(spriteEmpty.transform.localScale.x);

        int previousEntityDirectionX = entityDirectionX;

        if (flip)
        {
            spriteEmpty.transform.localScale = new Vector3(-currentSpriteScaleX, spriteEmpty.transform.localScale.y, spriteEmpty.transform.localScale.z);
            collisionsEmpty.transform.localScale = new Vector3(-1, collisionsEmpty.transform.localScale.y, collisionsEmpty.transform.localScale.z);
            projectileSpawnersEmpty.transform.localScale = new Vector3(-1, projectileSpawnersEmpty.transform.localScale.y, projectileSpawnersEmpty.transform.localScale.z);
            moveTransformsEmpty.transform.localScale = new Vector3(-1, moveTransformsEmpty.transform.localScale.y, moveTransformsEmpty.transform.localScale.z);
            grabOpponentAnimationTransformsEmpty.transform.localScale = new Vector3(-1, grabOpponentAnimationTransformsEmpty.transform.localScale.y, grabOpponentAnimationTransformsEmpty.transform.localScale.z);

            entityDirectionX = -1;
        }
        else
        {
            spriteEmpty.transform.localScale = new Vector3(currentSpriteScaleX, spriteEmpty.transform.localScale.y, spriteEmpty.transform.localScale.z);
            collisionsEmpty.transform.localScale = new Vector3(1, collisionsEmpty.transform.localScale.y, collisionsEmpty.transform.localScale.z);
            projectileSpawnersEmpty.transform.localScale = new Vector3(1, projectileSpawnersEmpty.transform.localScale.y, projectileSpawnersEmpty.transform.localScale.z);
            moveTransformsEmpty.transform.localScale = new Vector3(1, moveTransformsEmpty.transform.localScale.y, moveTransformsEmpty.transform.localScale.z);
            grabOpponentAnimationTransformsEmpty.transform.localScale = new Vector3(1, grabOpponentAnimationTransformsEmpty.transform.localScale.y, grabOpponentAnimationTransformsEmpty.transform.localScale.z);

            entityDirectionX = 1;
        }
    }

    public float GetActualXDirection()
    {
        return actualDirectionX;
    }

    public float GetXDirection()
    {
        return entityDirectionX;
    }

    public void CompletelyEnableAttacking()
    {
        if (fighterInput)
        {
            fighterInput.canAttack = true;
        }

        if (fighterAttackManager)
        {
            fighterAttackManager.inputtedMotionCommand = false;
            fighterAttackManager.canPerformNormals = true;
            fighterAttackManager.canPerformSpecials = true;
            fighterAttackManager.canPerformSuper = true;
            fighterAttackManager.inChainCancelWindow = false;
        }

        if (entityMovement)
        {
            entityMovement.DisableCollisionCheck(0);
            entityMovement.DisableGroundCheck(0);
            entityMovement.DisableMovement(0);
        }
    }
}
