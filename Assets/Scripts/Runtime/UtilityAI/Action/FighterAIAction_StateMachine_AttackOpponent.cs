using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Opponent", menuName = "UtilityAI/Actions/Attack Opponent")]
public class FighterAIAction_StateMachine_AttackOpponent : FighterAIAction
{
    public enum AIAttackStateNormals
    {
        StandingLightPunch,
        StandingLightKick,
        StandingHeavyPunch,
        StandingHeavyKick,
        StandingFiercePunch,
        StandingFierceKick,

        AirborneLightPunch,
        AirborneLightKick,
        AirborneHeavyPunch,
        AirborneHeavyKick,
        AirborneFiercePunch,
        AirborneFierceKick,

        CrouchingLightPunch,
        CrouchingLightKick,
        CrouchingHeavyPunch,
        CrouchingHeavyKick,
        CrouchingFiercePunch,
        CroucihngFierceKick
    }

    [HideInInspector]
    public bool attackHit;

    public string hitboxName;
    public int hitboxChild;

    public AIAttackStateNormals aiAttackStateNormal;

    private AttackFighterState attackState;

    private void PickedAttackState_Normals(StateMachine_Entity stateMachine)
    {
        if (aiAttackStateNormal == AIAttackStateNormals.StandingLightPunch)
        {
            attackState = stateMachine.state_groundLightPunch;
        }
        else if (aiAttackStateNormal == AIAttackStateNormals.StandingLightKick)
        {
            attackState = stateMachine.state_groundLightKick;
        }
        else if (aiAttackStateNormal == AIAttackStateNormals.StandingHeavyPunch)
        {
            attackState = stateMachine.state_groundHeavyPunch;
        }
        else if (aiAttackStateNormal == AIAttackStateNormals.StandingHeavyKick)
        {
            attackState = stateMachine.state_groundHeavyKick;
        }
        else if (aiAttackStateNormal == AIAttackStateNormals.StandingFiercePunch)
        {
            attackState = stateMachine.state_groundFiercePunch;
        }
        else if (aiAttackStateNormal == AIAttackStateNormals.StandingFierceKick)
        {
            attackState = stateMachine.state_groundFierceKick;
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
            PickedAttackState_Normals(ai.stateMachine_Entity);

            Debug.Log($"<size=15><color=#9aff0f>Sm: {ai.stateMachine_Entity} {attackState}</color></size>");

            ai.stateMachine_Entity.ChangeState(attackState);
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
