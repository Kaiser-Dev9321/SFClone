using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CloseRange", menuName = "UtilityAI/Actions/At Close Range")]
public class FighterAIAction_AtCloseRange : FighterAIAction
{
    private void RandomNormalAttack(FighterAIController ai)
    {
        int randomAttackChance = Random.Range(0, 100);
        int randomAttack = Random.Range(0, 100);

        if (ai.randomAttackTimerBool)
        {
            if (randomAttackChance < 5)
            {
                if (randomAttack < 50)
                {
                    ai.ChangeBestAction(ai.normalAttacksAvailable[3]);
                }
                else
                {
                    ai.ChangeBestAction(ai.normalAttacksAvailable[5]);
                }
            }

            ai.randomAttackTimerBool = false;
        }
    }

    private void LinkAttacks(FighterAIController ai)
    {
        //Debug.Log($"Link attacks begin");

        ai.ChangeBestAction(ai.attackLinksAvailable[0]);
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
            //Debug.Log("Active and close range");

            //RandomNormalAttack(ai);
            LinkAttacks(ai);
            MoveDirection(ai);
        }

        //Debug.Log($"<color=#219bdb>Distance score: {considerations[0].score}</color>");

        if (score < 0.25f)
        {
            ai.OnFinishedAction();
        }
    }

    public override void Exit(FighterAIController ai)
    {
    }
}
