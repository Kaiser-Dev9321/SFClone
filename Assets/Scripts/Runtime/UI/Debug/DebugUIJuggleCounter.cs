using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIJuggleCounter : MonoBehaviour
{
    public FighterGameplayManager gameplayManager;
    public GameManager gameManager;

    public TextMeshProUGUI currentJuggleCountText;
    public TextMeshProUGUI maximumJugglePotentialText;
    public TextMeshProUGUI currentJugglePotentialText;

    private void Start()
    {
        gameplayManager = FindObjectOfType<FighterGameplayManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    //TODO: I really wanna turn this into a ruler
    public void SetJuggleCounterDisplay(EntityScript fighter)
    {
        currentJuggleCountText.text = $"Current Juggle: {fighter.fighterJuggleStateManager.currentJuggle_Amount}";

        AttackFighterState currentAttackState = fighter.stateMachine.currentState as AttackFighterState;

        if (currentAttackState)
        {
            if (fighter.fighterComboManager.currentPerformedAttackEffect)
            {
                currentJugglePotentialText.text = $"Current Juggle Potential: {fighter.fighterComboManager.currentPerformedAttackEffect.juggle_Potential}";
            }
        }

        if (fighter.fighterComboManager.currentPerformedAttackEffect)
        {
            if (fighter.fighterComboManager.currentAttackHit)
            {
                maximumJugglePotentialText.text = $"Maximum Juggle Potential: {fighter.fighterComboManager.currentPerformedAttackEffect.juggle_Potential}";
            }

            //Check if you can get more juggles

            //Juggle can be continued
            if (!fighter.fighterJuggleStateManager.FighterAboveMaxJuggle(fighter))
            {
                if (fighter.fighterComboManager.currentAttackHit)
                {
                    currentJuggleCountText.color = new Color(0, 1, 0, 1);
                    maximumJugglePotentialText.color = new Color(0, 1, 0, 1);
                }
            }
            //Juggle can't be continued
            else if (fighter.fighterJuggleStateManager.FighterAboveMaxJuggle(fighter))
            {
                if (fighter.fighterComboManager.currentAttackHit)
                {
                    currentJuggleCountText.color = new Color(1, 0, 0, 1);
                    maximumJugglePotentialText.color = new Color(1, 0, 0, 1);
                }
            }
        }

        if (fighter.fighterJuggleStateManager.currentJuggle_Amount <= 0)
        {
            if (fighter.fighterComboManager.currentAttackHit)
            {
                currentJuggleCountText.color = new Color(1, 1, 1, 1);
                maximumJugglePotentialText.color = new Color(1, 1, 1, 1);
            }
        }
    }

    private void Update()
    {
        SetJuggleCounterDisplay(gameManager.fighter1);
        //SetJuggleCounterDisplay(fighterGameplayManager.fighter2);
    }
}
