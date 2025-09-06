using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DistanceToPlayerConsideration", menuName = "UtilityAI/Considerations/Distance To Player")]
public class DistanceToPlayerConsideration : Consideration
{
    public AnimationCurve responseCurve;

    [Range(0.0001f, 100)]
    public float dividedInfluence = 1;

    public override float ScoreConsideration(FighterAIController fighterAIController)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(fighterAIController.xDistanceToPlayer / dividedInfluence));

        return score;
    }
}
