using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomConsideration", menuName = "UtilityAI/Considerations/Random")]
public class RandomConsideration : Consideration
{
    public override float ScoreConsideration(FighterAIController fighterAIController)
    {
        score = Random.Range(0, 1);

        return score;
    }
}
