using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Increases juggle when in the air
[RequireComponent(typeof(FighterComboManager))]
public class FighterJuggleStateManager : MonoBehaviour
{
    public int currentJuggle_Amount;

    public void SetStartJuggle(int amount)
    {
        //print("Start juggle");
        currentJuggle_Amount = amount;
    }

    public void IncreaseJuggle(int amount)
    {
        //print("Increase juggle");
        currentJuggle_Amount += amount;
    }

    public void ResetJuggle()
    {
        //print("Reset juggle");
        currentJuggle_Amount = 0;
    }

    public bool CanJuggle(AttackEffectData attackEffectData, StateMachine_Entity stateMachine)
    {
        if (attackEffectData.canAirJuggle)
        {
            return true;
        }

        if (attackEffectData.attackEffectType == AttackEffectType.Blow)
        {
            return true;
        }

        if (stateMachine.currentState == stateMachine.state_air)
        {
            return true;
        }

        return false;
    }

    public void AbleToJuggle(AttackEffectData attackEffectData, StateMachine_Entity stateMachine, EntityScript attacker_Entity, EntityScript defender_Entity)
    {
        if (CanJuggle(attackEffectData, stateMachine))
        {
            if (attacker_Entity.fighterJuggleStateManager.currentJuggle_Amount <= 0)
            {
                if (attackEffectData.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                {
                    return;
                }

                attacker_Entity.fighterJuggleStateManager.SetStartJuggle(attackEffectData.juggle_Start);
            }
            else
            {
                if (attackEffectData.attackEffectType == AttackEffectType.Hit && defender_Entity.entityMovement.groundChecker.grounded)
                {
                    return;
                }

                attacker_Entity.fighterJuggleStateManager.IncreaseJuggle(attackEffectData.juggle_Increase);
            }
        }
    }

    public void SimpleSetJuggle(EntityScript entity, AttackEffectData attackEffectData)
    {
        if (entity.fighterJuggleStateManager.CanJuggle(attackEffectData, entity.entityAnimator.stateMachine))
        {
            if (entity.fighterJuggleStateManager.currentJuggle_Amount <= 0)
            {
                entity.fighterJuggleStateManager.SetStartJuggle(attackEffectData.juggle_Start);
            }
            else
            {
                entity.fighterJuggleStateManager.IncreaseJuggle(attackEffectData.juggle_Increase);
            }
        }
    }

    public bool CheckJuggleAboveMaximum(int currentJuggleAmount, int maximumJuggleAmount)
    {
        if (currentJuggleAmount > maximumJuggleAmount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FighterAboveMaxJuggle(EntityScript attacker_Entity)
    {
        return currentJuggle_Amount > attacker_Entity.fighterComboManager.currentPerformedAttackEffect.juggle_Potential;
    }
}
