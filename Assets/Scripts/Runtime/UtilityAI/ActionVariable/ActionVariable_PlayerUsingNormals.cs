using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionVariable_AI_", menuName = "UtilityAI/Action Variables/Player Using Normals")]
public class ActionVariable_PlayerUsingNormals : ActionVariable
{
    private float curveScore;

    public AnimationCurve responseCurve;

    [Range(0.0001f, 100)]
    public float multipliedInfluence;

    public override float ScoreActionVariable(FighterAIController fighterAIController)
    {
        if (fighterAIController.playerIsAttacking)
        {
            curveScore += multipliedInfluence * Time.deltaTime;
        }
        else
        {
            curveScore -= multipliedInfluence * Time.deltaTime;
        }

        curveScore = Mathf.Clamp01(curveScore);

        score = responseCurve.Evaluate(Mathf.Clamp01(curveScore));

        return score;
    }
}
