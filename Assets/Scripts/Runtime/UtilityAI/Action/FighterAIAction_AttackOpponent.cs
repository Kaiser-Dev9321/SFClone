using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Opponent", menuName = "UtilityAI/Actions/Attack Opponent")]
public class FighterAIAction_AttackOpponent : FighterAIAction
{

    [HideInInspector]
    public bool attackHit;

    public string hitboxName;
    public int hitboxChild;

    public FighterAttackButtons aiAttackStateNormal;

    private AttackFighterState attackState;

    private void PickedAttackState_Normals(FighterAIController ai)
    {
        if (aiAttackStateNormal == FighterAttackButtons.LightPunch)
        {
            ai.fighterEntity.fighterInput.button_lightPunch = true;
        }
        else if (aiAttackStateNormal == FighterAttackButtons.LightKick)
        {
            ai.fighterEntity.fighterInput.button_lightKick = true;
        }
        else if (aiAttackStateNormal == FighterAttackButtons.HeavyPunch)
        {
            ai.fighterEntity.fighterInput.button_heavyPunch = true;
        }
        else if (aiAttackStateNormal == FighterAttackButtons.HeavyKick)
        {
            ai.fighterEntity.fighterInput.button_heavyKick = true;
        }
        else if (aiAttackStateNormal == FighterAttackButtons.FiercePunch)
        {
            ai.fighterEntity.fighterInput.button_fiercePunch = true;
        }
        else if (aiAttackStateNormal == FighterAttackButtons.FierceKick)
        {
            ai.fighterEntity.fighterInput.button_fierceKick = true;
        }
    }

    public override void Enter(FighterAIController ai)
    {
        attackHit = false;
    }

    public override void Execute(FighterAIController ai)
    {
        if (!ai.fighterEntity.onTheGround && !ai.fighterEntity.entityHitstun.hitStunned && ai.fighterEntity.fighterInput.canAttack)
        {
            PickedAttackState_Normals(ai);
        }

        if (score < 0.4f)
        {
            Debug.Log("Finish attack action");

            ai.OnFinishedAction();
        }
    }

    public override void Exit(FighterAIController ai)
    {
    }
}
