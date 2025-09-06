using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterKnockdownManager : MonoBehaviour
{
    private EntityScript entity;
    private StateMachine_Entity stateMachine_Entity;

    public AirRecoveryData airRecoveryData;
    public HitstunData knockdownAirHitstunData;

    private RoundManager roundManager;
    private FighterGameplayManager fighterGameplayManager;

    private void Start()
    {
        entity = GetComponent<EntityScript>();
        stateMachine_Entity = GetComponent<StateMachine_Entity>();

        roundManager = FindObjectOfType<RoundManager>();
        fighterGameplayManager = FindObjectOfType<FighterGameplayManager>();
    }

    private void SetAirHitstun()
    {
        entity.entityHitstun.state_airHitstun.hitstunCurveX = knockdownAirHitstunData.hitstunAnimationCurveX;
        entity.entityHitstun.state_airHitstun.hitstunCurveY = knockdownAirHitstunData.hitstunAnimationCurveY;

        entity.entityHitstun.state_airHitstun.animationName = knockdownAirHitstunData.animationName;
        entity.entityHitstun.state_airHitstun.layer = -1;
        entity.entityHitstun.state_airHitstun.normalisedTime = 0;

        entity.entityHitstun.state_airHitstun.knockedDown = true;
        entity.entityHitstun.state_airHitstun.disableGroundCheck = true;
    }

    public void ActivateKnockout()
    {
        if (!entity.isKnockedDown)
        {
            print($"<b>KNOCK OUT !!</b>");

            SetAirHitstun();

            entity.stateMachine.ChangeState(entity.entityHitstun.state_airHitstun);

            entity.isKnockedDown = true;

            roundManager.SetRoundWinner(fighterGameplayManager.fighter1RoundWins.transform, fighterGameplayManager.fighter2RoundWins.transform);

            fighterGameplayManager.PerformRoundOrMatchWin();
        }
    }
}
