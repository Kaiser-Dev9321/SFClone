using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MidRange", menuName = "UtilityAI/Actions/At Mid Range")]
public class FighterAIAction_AtMidRange : FighterAIAction
{
    public ActionVariable_PlayerUsingNormals actionVariable_PlayerUsingPokes;

    private void UpdateActionVariables(FighterAIController ai)
    {
        actionVariable_PlayerUsingPokes.ScoreActionVariable(ai);
    }

    private void PickSpecialMoves(FighterAIController ai)
    {
        if (ai.randomPickSpecialMoveTimerBool)
        {
            int randomPickSpecialMoveAttack = Random.Range(0, 100);

            switch (ai.fighterAIType)
            {
                case FighterAIController.FighterAIType.Shoto:
                    break;
                case FighterAIController.FighterAIType.Zoner:
                    if (randomPickSpecialMoveAttack < 80)
                    {
                        ai.ChangeBestAction(ai.specialMovesAvailable[0]);
                    }
                    else if (randomPickSpecialMoveAttack < 60)
                    {
                        ai.ChangeBestAction(ai.specialMovesAvailable[1]);
                    }
                    else if (randomPickSpecialMoveAttack < 40)
                    {
                        ai.ChangeBestAction(ai.specialMovesAvailable[2]);
                    }
                    break;
                case FighterAIController.FighterAIType.Rushdown:
                    break;
                case FighterAIController.FighterAIType.Mixup:
                    break;
                case FighterAIController.FighterAIType.Grappler:
                    break;
            }

            ai.randomPickSpecialMoveTimerBool = false;
        }
    }

    private void RandomAttack(FighterAIController ai)
    {
        if (ai.randomAttackTimerBool)
        {
            //Debug.Log($"Random attack: {actionVariable_PlayerUsingPokes.score}");

            int randomAttack = Random.Range(0, 100);

            if (randomAttack < 5)
            {
                ai.ChangeBestAction(ai.normalAttacksAvailable[4]);
            }

            ai.randomAttackTimerBool = false;
        }
    }

    private void CounterAttack(FighterAIController ai)
    {
        //Debug.Log($"<size=22><color=#705376>Counter attack: {actionVariable_PlayerUsingPokes.score}</color></size>");

        if (actionVariable_PlayerUsingPokes.score == 1)
        {
            ai.ChangeBestAction(ai.normalAttacksAvailable[Random.Range(2,5)]);
        }
    }

    private void MoveDirection(FighterAIController ai)
    {
        int getCloseOrStay = Random.Range(0, 100);

        switch (ai.fighterAIType)
        {
            case FighterAIController.FighterAIType.Shoto:
                if (ai.moveTimerBool)
                {
                    if (getCloseOrStay < 50)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(0, 0);
                    }
                }
                break;
            case FighterAIController.FighterAIType.Zoner:

                if (ai.moveTimerBool)
                {
                    if (getCloseOrStay < 70)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else if (getCloseOrStay < 40)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(-1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(0, 0);
                    }
                }
                break;
            case FighterAIController.FighterAIType.Rushdown:

                if (ai.moveTimerBool)
                {
                    if (getCloseOrStay < 85)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(0, 0);
                    }
                }
                break;
            case FighterAIController.FighterAIType.Mixup:
                if (ai.moveTimerBool)
                {
                    if (getCloseOrStay < 70)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else if (getCloseOrStay < 35)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(0, 0);
                    }
                }
                break;
            case FighterAIController.FighterAIType.Grappler:
                if (ai.moveTimerBool)
                {
                    if (getCloseOrStay < 85)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else if (getCloseOrStay < 65)
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(-1 * ai.fighterEntity.GetXDirection(), 0);
                    }
                    else
                    {
                        ai.fighterEntity.fighterInput.movement = new Vector2(0, 0);
                    }
                }
                break;
        }


        ai.moveTimerBool = false;
    }

    public override void Enter(FighterAIController ai)
    {
    }

    public override void Execute(FighterAIController ai)
    {

        if (!ai.fighterEntity.onTheGround && ai.fighterEntity.fighterInput.canAttack && !ai.fighterEntity.fighterAttackManager.inputtedMotionCommand)
        {
            UpdateActionVariables(ai);
            PickSpecialMoves(ai);
            CounterAttack(ai);
            RandomAttack(ai);
            MoveDirection(ai);
        }

        //Debug.Log($"<color=#20f60b>Distance score: {considerations[0].score}</color>");

        if (score < 0.25f || score > 0.75f)
        {
            ai.OnFinishedAction();
        }
    }

    public override void Exit(FighterAIController ai)
    {
    }
}
