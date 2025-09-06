using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class AIAttackPattern
{
    //TODO: Shrink these button enums to just 1
    public enum AIAttackPatternButtons
    {
        None,
        LightPunch,
        LightKick,
        HeavyPunch,
        HeavyKick,
        FiercePunch,
        FierceKick
    }

    public float waitTime;
    public Vector2 inputDirectionRelativeToEntityDirection;
    public AIAttackPatternButtons buttonToPress;
}

//Base version for other AI to use
[CreateAssetMenu(fileName = "AttackPattern to Opponent", menuName = "UtilityAI/Actions/AttackPattern to Opponent")]
public class FighterAIAction_StateMachine_AttackPatternOpponent : FighterAIAction
{
    [SerializeField]
    private Timer attackPatternTimer;

    public AIAttackPattern[] aIAttackPattern;

    private int currentAttackPatternIndex = 0;

    private bool updateAttackInputNow = false;

    [SerializeField]
    private bool inputsFinished = false;

    private Vector2 lastAIInput;

    public override void Awake()
    {
        base.Awake();
    }

    private void GetAIAttackPattern_ButtonInput(FighterAIController ai, int index)
    {
        switch(aIAttackPattern[index].buttonToPress)
        {
            case AIAttackPattern.AIAttackPatternButtons.None:
                ai.fighterEntity.fighterInput.button_lightPunch = false;
                ai.fighterEntity.fighterInput.button_heavyPunch = false;
                ai.fighterEntity.fighterInput.button_fiercePunch = false;
                ai.fighterEntity.fighterInput.button_lightKick = false;
                ai.fighterEntity.fighterInput.button_heavyKick = false;
                ai.fighterEntity.fighterInput.button_fierceKick = false;
                break;

            case AIAttackPattern.AIAttackPatternButtons.LightPunch:
                ai.fighterEntity.fighterInput.button_lightPunch = true;
                break;
            case AIAttackPattern.AIAttackPatternButtons.LightKick:
                ai.fighterEntity.fighterInput.button_lightKick = true;
                break;
            case AIAttackPattern.AIAttackPatternButtons.HeavyPunch:
                ai.fighterEntity.fighterInput.button_heavyPunch = true;
                break;
            case AIAttackPattern.AIAttackPatternButtons.HeavyKick:
                ai.fighterEntity.fighterInput.button_heavyKick = true;
                break;
            case AIAttackPattern.AIAttackPatternButtons.FiercePunch:
                ai.fighterEntity.fighterInput.button_fiercePunch = true;
                break;
            case AIAttackPattern.AIAttackPatternButtons.FierceKick:
                ai.fighterEntity.fighterInput.button_fierceKick = true;
                break;
        }
    }

    private void DoAIAttackPattern(FighterAIController ai)
    {
        Vector2 currentAttackPatternIndex_InputDirection = aIAttackPattern[currentAttackPatternIndex].inputDirectionRelativeToEntityDirection;

        if (currentAttackPatternIndex_InputDirection != lastAIInput)
        {
            //TODO: Fix AI spamming inputs, check amount of inputs from motion input manager

            //Debug.Log($"New input from AI: {ai.fighterEntity.fighterInput.movement}");

            ai.fighterEntity.fighterInput.movement = new Vector2(currentAttackPatternIndex_InputDirection.x * ai.fighterEntity.GetXDirection(), currentAttackPatternIndex_InputDirection.y);

            lastAIInput = ai.fighterEntity.fighterInput.movement;
        }

        GetAIAttackPattern_ButtonInput(ai, currentAttackPatternIndex);

        //Update when timer reaches below zero and should be updated
        if (updateAttackInputNow)
        {
            updateAttackInputNow = false;

            currentAttackPatternIndex++;
            currentAttackPatternIndex = Mathf.Clamp(currentAttackPatternIndex, 0, aIAttackPattern.Length - 1);

            ResetAttackTimer();
        }
    }

    //Update attack pattern timer
    private void UpdateAttackPatternTimer()
    {
        if (attackPatternTimer.currentTime > 0 && !updateAttackInputNow)
        {
            attackPatternTimer.DecreaseByTick();
        }

        if (attackPatternTimer.currentTime <= 0)
        {
            updateAttackInputNow = true;

            if (currentAttackPatternIndex >= aIAttackPattern.Length - 1)
            {
                inputsFinished = true;
            }
        }
    }

    private void ResetAttackTimer()
    {
        lastAIInput = Vector2.zero;
        attackPatternTimer = new Timer(aIAttackPattern[currentAttackPatternIndex].waitTime, aIAttackPattern[currentAttackPatternIndex].waitTime);
    }

    public override void Enter(FighterAIController ai)
    {
        currentAttackPatternIndex = 0;
        inputsFinished = false;

        ResetAttackTimer();
    }

    public override void Execute(FighterAIController ai)
    {
        UpdateAttackPatternTimer();

        //Perform direction and input
        if (updateAttackInputNow)
        {
            //Debug.Log($"<size=15>Attack time reset</size>");

            attackPatternTimer.SetToTimer();
        }

        DoAIAttackPattern(ai);

        if (ai.fighterEntity.entityHitstun.hitStunned || inputsFinished)
        {
            //Debug.Log($"<color=#30facd><b>Completed attack input</b></color>");

            ai.ChangeBestAction(ai.actionsAvailable[0]);
        }

        //Debug.Log($"Current input: {attackPatternTimer.currentTime}");
    }

    public override void Exit(FighterAIController ai)
    {
    }
}
