using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoundInfo : MonoBehaviour
{
    private FighterGameplayManager fighterGameplayManager;
    private RoundManager roundManager;
    private Animator roundInfoAnimator;
    private void Start()
    {
        fighterGameplayManager = FindObjectOfType<FighterGameplayManager>();
        roundManager = FindObjectOfType<RoundManager>();
        roundInfoAnimator = GetComponent<Animator>();
    }

    public void ResetRoundAnimation(int isBeginningOfRound)
    {
        if (isBeginningOfRound == 0)
        {
            roundInfoAnimator.Play("RoundInfo_Empty");
        }

        if (isBeginningOfRound == 1)
        {
            roundManager.MakeFighterActive(fighterGameplayManager.gameManager.fighter1);
            roundManager.MakeFighterActive(fighterGameplayManager.gameManager.fighter2);
        }
    }
}
