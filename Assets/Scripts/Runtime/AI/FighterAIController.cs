using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAIController : MonoBehaviour
{
    public enum FighterAIType
    {
        Shoto,
        Zoner,
        Rushdown,
        Mixup,
        Grappler
    }

    public FighterAIType fighterAIType;

    [HideInInspector]
    public AIBrain aiBrain;

    //TODO: Add this to other AI fighters
    public StateMachine_Entity stateMachine_Entity;

    [HideInInspector]
    public AIFighterInput aiFighterInput;

    public FighterAIAction[] actionsAvailable;
    public FighterAIAction[] normalAttacksAvailable;
    public FighterAIAction[] attackLinksAvailable;
    public FighterAIAction[] specialMovesAvailable;

    [HideInInspector]
    public EntityScript fighterEntity;

    [HideInInspector]
    public EntityScript opponentEntity;

    #region AI Variables
    public float xDistanceToPlayer;
    public bool playerIsAttacking;
    public bool playerIsBlocking;
    public bool playerIsJumping;
    public float playerSpeed;

    [HideInInspector]
    public bool moveTimerBool;
    public bool randomAttackTimerBool;
    public bool randomPickSpecialMoveTimerBool;
    #endregion

    #region Timers
    private Timer moveTimer;
    private Timer randomAttackTimer;
    private Timer randomPickSpecialMoveTimer;
    #endregion

    [HideInInspector]
    public bool aiLoaded = false;

    private void Awake()
    {
        transform.GetComponent<EntityEventLoader>().entityEssentialsLoadedEvent += EntityEventLoader_entityEssentialsLoadedEvent;

        moveTimer = new Timer(1, 1);
        randomAttackTimer = new Timer(1, 1);
        randomPickSpecialMoveTimer = new Timer(1, 1);

        aiBrain = GetComponent<AIBrain>();
    }

    private void EntityEventLoader_entityEssentialsLoadedEvent()
    {
        aiLoaded = true;
    }

    private void Start()
    {
        fighterEntity = GetComponent<EntityScript>();

        opponentEntity = GameObject.FindObjectOfType<GameManager>().fighter1;

        aiFighterInput = fighterEntity.fighterInput as AIFighterInput;

        SetAIVariables();
    }

    private void UpdateAITimers()
    {
        if (moveTimer.currentTime >= 0)
        {
            moveTimer.DecreaseByTick();
        }

        if (moveTimer.currentTime < 0)
        {
            //print("Move timer set");

            moveTimerBool = true;
            moveTimer.SetToTimer();
        }

        if (randomAttackTimer.currentTime >= 0)
        {
            randomAttackTimer.DecreaseByTick();
        }

        if (randomAttackTimer.currentTime < 0)
        {
            //print("Random attack timer set");

            randomAttackTimerBool = true;
            randomAttackTimer.SetToTimer();
        }

        if (randomPickSpecialMoveTimer.currentTime < 0)
        {
            //print("Random special move timer set");

            randomPickSpecialMoveTimerBool = true;
            randomPickSpecialMoveTimer.SetToTimer();
        }
    }

    private void SetAIVariables()
    {
        xDistanceToPlayer = Vector2.Distance(transform.position, opponentEntity.transform.position);
        playerIsAttacking = !opponentEntity.fighterAttackManager.canPerformNormals;
    }

    public void ChangeBestAction(FighterAIAction fighterAIAction)
    {
        if (aiBrain.bestAction != null)
        {
            aiBrain.bestAction.Exit(this);
        }

        aiBrain.previousBestAction = aiBrain.bestAction;

        aiBrain.bestAction = fighterAIAction;
        aiBrain.bestActionString = fighterAIAction.ToString();

        aiBrain.bestAction.Enter(this);
    }

    private void Update()
    {
        if (aiLoaded)
        {
            if (aiBrain.bestAction)
            {
                //print("Best Action: " + aiBrain.bestAction.Name + "           " + aiBrain.bestAction.score);

                aiBrain.bestAction.Execute(this);

                aiBrain.ScoreAction(aiBrain.bestAction);
            }

            SetAIVariables();
            UpdateAITimers();
        }

        if (!opponentEntity)
        {
            opponentEntity = GameObject.FindObjectOfType<GameManager>().fighter1;
        }
    }

    public void OnFinishedAction()
    {
        aiBrain.DecideBestAction(actionsAvailable);
    }
}


