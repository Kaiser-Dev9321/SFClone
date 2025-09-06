using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpeedConsideration", menuName = "UtilityAI/Considerations/Player Speed")]
public class PlayerSpeedConsideration : Consideration
{
    public AnimationCurve responseCurve;

    public override float ScoreConsideration(FighterAIController fighterAIController)
    {
        score = responseCurve.Evaluate(Mathf.Clamp01(Mathf.Abs(fighterAIController.opponentEntity.entityMovement.movement.velocity.x / 5)));

        return score;
    }
}
