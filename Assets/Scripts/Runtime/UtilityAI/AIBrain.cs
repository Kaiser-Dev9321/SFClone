using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    [HideInInspector]
    public FighterAIAction previousBestAction;
    public FighterAIAction bestAction;
    public string bestActionString;

    public FighterAIController fighterAIController;

    private void Awake()
    {
        fighterAIController = GetComponent<FighterAIController>();
    }

    private void Start()
    {
        fighterAIController.ChangeBestAction(fighterAIController.actionsAvailable[1]);
    }

    //Loop through all considerations of the action
    //Give the highest scoring action
    public void DecideBestAction(FighterAIAction[] actionsAvailable)
    {
        float score = 0f;
        int nextBestActionIndex = 0;

        for (int i = 0; i < actionsAvailable.Length; i++)
        {
            if (ScoreAction(actionsAvailable[i]) > score)
            {
                nextBestActionIndex = i;
                score = actionsAvailable[i].score;
            }
        }

        //if (fighterAIController.fighterEntity.fighterInput.canAttack && fighterAIController.fighterEntity.fighterAttackManager.canPerformNormals)
        {
            fighterAIController.ChangeBestAction(actionsAvailable[nextBestActionIndex]);
        }
    }

    //Loop through all considerations of the actionis
    //Score all considerations
    //Average the consideration scores ==> overall action score
    public float ScoreAction(FighterAIAction action)
    {
        float score = 1;

        for (int i = 0; i < action.considerations.Length; i++)
        {
            float considerationScore = action.considerations[i].ScoreConsideration(fighterAIController);
            score *= considerationScore;

            //Scores current action
            if (score == 0)
            {
                action.score = 0;

                return action.score; //Score was 0, end here
            }
        }

        //Average scheme of overall score
        float originalScore = score;
        float modFactor = 1 - (1 / action.considerations.Length);
        float makeupValue = (1 - originalScore) * modFactor;

        action.score = originalScore + (makeupValue * originalScore);

        return action.score;
    }
}
