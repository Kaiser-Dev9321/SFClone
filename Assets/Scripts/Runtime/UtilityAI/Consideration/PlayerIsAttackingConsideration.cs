using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIsAttackingConsideration", menuName = "UtilityAI/Considerations/Player Is Attacking")]
public class PlayerIsAttackingConsideration : Consideration
{
    private float curveScore;

    public AnimationCurve responseCurve;

    [Range(0.0001f, 100)]
    public float dividedInfluence = 1;

    public override float ScoreConsideration(FighterAIController fighterAIController)
    {
        if (fighterAIController.playerIsAttacking)
        {
            curveScore += Time.deltaTime / dividedInfluence;
        }
        else
        {
            curveScore -= Time.deltaTime / dividedInfluence;
        }

        score = responseCurve.Evaluate(Mathf.Clamp01(curveScore));

        return score;
    }
}
