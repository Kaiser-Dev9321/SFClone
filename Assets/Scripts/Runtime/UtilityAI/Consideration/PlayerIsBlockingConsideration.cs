using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerIsBlockingConsideration", menuName = "UtilityAI/Considerations/Player Is Blocking")]
public class PlayerIsBlockingConsideration : Consideration
{
    public AnimationCurve responseCurve;

    [Range(0.0001f, 100)]
    public float dividedInfluence = 1;

    public override float ScoreConsideration(FighterAIController fighterAIController)
    {
        if (fighterAIController.playerIsBlocking)
        {
            score += 0.1f / dividedInfluence;
        }
        else
        {
            score -= 0.1f / dividedInfluence;
        }

        score = responseCurve.Evaluate(Mathf.Clamp01(score));

        return score;
    }
}
