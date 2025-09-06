using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Get Away From Player", menuName = "UtilityAI/Actions/Get Away From Player")]
public class FighterAIAction_AtFarRange : FighterAIAction
{
    public ActionVariable_PlayerUsingNormals actionVariable_PlayerUsingPokes;

    private void RandomAttack(FighterAIController ai)
    {
        if (actionVariable_PlayerUsingPokes.score == 1)
        {
            int randomAttack = Random.Range(0, 100);

            if (randomAttack < 5 && ai.randomAttackTimerBool)
            {
                ai.randomAttackTimerBool = false;
                ai.ChangeBestAction(ai.normalAttacksAvailable[3]);
            }
        }
    }

    private void MoveDirection(FighterAIController ai)
    {
        int getCloseOrStay = Random.Range(0, 100);

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

            ai.moveTimerBool = false;
        }
    }

    public override void Enter(FighterAIController ai)
    {
    }

    public override void Execute(FighterAIController ai)
    {

        if (!ai.fighterEntity.onTheGround && ai.fighterEntity.fighterInput.canAttack && !ai.fighterEntity.fighterAttackManager.inputtedMotionCommand)
        {
            //Debug.Log("Active and far range");

            RandomAttack(ai);
            MoveDirection(ai);
        }

        //Debug.Log($"<color=#92028f>Distance score: {considerations[0].score}</color>");

        if (score <= 0.75f)
        {
            ai.OnFinishedAction();
        }
    }

    public override void Exit(FighterAIController ai)
    {
    }
}
