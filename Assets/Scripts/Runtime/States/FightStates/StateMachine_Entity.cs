using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class StateMachine_Entity : MonoBehaviour, IEntityStateMachine
{
    private FighterInput fighterInput;

    public IState currentState { get; set; }
    public FighterStateData currentAttackStateData;

    public IState previousState { get; set; }

    public string currentStateString;

    private EntityMovement entityMovement;
    private EntityAnimator entityAnimator;

    public State_MotionCommand[] state_MotionCommands;

    public FighterState state_idle;
    public FighterState state_walk;

    public FighterState state_standingBlock;
    public FighterState state_crouchingBlock;

    public FighterState state_air;
    public FighterState state_jump;

    //Ground/standing attacks
    public AttackFighterState state_groundLightPunch;
    public AttackFighterState state_groundHeavyPunch;
    public AttackFighterState state_groundFiercePunch;
    public AttackFighterState state_groundLightKick;
    public AttackFighterState state_groundHeavyKick;
    public AttackFighterState state_groundFierceKick;

    //Air attacks
    [Space]
    public AttackFighterState state_airLightPunch;
    public AttackFighterState state_airHeavyPunch;
    public AttackFighterState state_airFiercePunch;
    public AttackFighterState state_airLightKick;
    public AttackFighterState state_airHeavyKick;
    public AttackFighterState state_airFierceKick;

    //TODO: Crouching attacks coming soon

    public GrabFighterState state_Grab; //TODO: Unsure if I should write things to this state to then override it, or make separate states for it

    private bool stateMachineLoaded = false;

    public void GetEntityComponents()
    {
        //print("Loaded state machine essentials");

        entityAnimator = GetComponent<EntityAnimator>();
        entityMovement = GetComponentInChildren<EntityMovement>();

        fighterInput = GetComponent<FighterInput>();
    }

    private void Awake()
    {
        transform.GetComponent<EntityEventLoader>().entityEssentialsLoadedEvent += StateMachine_Entity_entityEssentialsLoadedEvent;
    }

    private void StateMachine_Entity_entityEssentialsLoadedEvent()
    {
        stateMachineLoaded = true;

        GetEntityComponents();
    }

    public virtual void Start() //For beginning state transitions
    {
        //GetEntityComponents();
    }

    private void Update()
    {
        if (stateMachineLoaded)
        {
            ExecuteStateUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (stateMachineLoaded)
        {
            ExecuteStatePhysicsUpdate();
        }
    }

    public void ChangeState(IState newState)
    {
        //print($"Changing state to: {newState}");

        if (this.currentState != null)
        {
            this.currentState.State_Exit();
        }

        this.previousState = this.currentState;

        this.currentState = newState;
        this.currentStateString = newState.ToString();

        this.currentState.State_Enter();
    }

    public void ExecuteStateUpdate()
    {
        if (this.currentState != null)
        {
            this.currentState.State_Update();
        }
    }

    public void ExecuteStatePhysicsUpdate()
    {
        if (this.currentState != null)
        {
            this.currentState.State_PhysicsUpdate();
        }
    }
}
