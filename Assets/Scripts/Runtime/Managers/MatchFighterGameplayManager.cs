using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

public class MatchFighterGameplayManager : FighterGameplayManager
{
    private void Awake()
    {
        gameManager = Object.FindObjectOfType<FighterGameManager>();
        fightersSuperEmpty = gameCanvas.transform.Find("FightersSuperEmpty").gameObject;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        LoadStageAtLaunch();

        CallMatchReferences();

        ShowRoundUIOnStart();

        SetFighterPositions();
    }

    #region Load Fighters

    private void AssignManagersOnEntityLoad()
    {
        gameManager.fighter1.entityEventLoader.entityLoadedEvent += LoadFighter1;
        gameManager.fighter2.entityEventLoader.entityLoadedEvent += LoadFighter2;
    }

    private void LoadFighter1()
    {
        gameManager.fighter1.LoadEntityReferences();
        gameManager.fighter1.fighterSuperManager.ui_SuperMeter = fightersSuperEmpty.transform.GetChild(0).GetComponent<UI_SuperMeter>();

        RestoreFighter(gameManager.fighter1, true);
    }

    private void LoadFighter2()
    {
        gameManager.fighter2.LoadEntityReferences();
        gameManager.fighter1.fighterSuperManager.ui_SuperMeter = fightersSuperEmpty.transform.GetChild(1).GetComponent<UI_SuperMeter>();

        RestoreFighter(gameManager.fighter2, false);
    }

    public void RestoreFighter(EntityScript fighter, bool isFighter1)
    {
        roundManager.MakeFighterInactive(fighter);

        if (isFighter1)
        {
            roundManager.fighter1Victory = true;
        }
        else
        {
            roundManager.fighter2Victory = true;
        }

        fighter.onTheGround = false;
        fighter.isKnockedDown = false;

        fighter.CompletelyEnableAttacking();

        fighter.entityHealth.SetHealthToMaxHealth();

        fighter.fighterBlockManager.ResetBlocking();

        fighter.entityHitstun.state_airHitstun.knockedDown = false;

        StateMachine_Entity fighter_stateMachine = fighter.stateMachine as StateMachine_Entity;

        fighter_stateMachine.ChangeState(fighter_stateMachine.state_idle);

        fighter.fighterComboManager.ResetCombo();
    }

    private void SetFighterPositions()
    {
        gameManager.fighter1.transform.position = roundManager.fighter1StartingPosition.transform.position;
        gameManager.fighter2.transform.position = roundManager.fighter2StartingPosition.transform.position;
    }

    #endregion

    #region Load Rounds

    private void ShowRoundUIOnStart()
    {
        print("Round manager start round UI");

        roundManager.StartRoundUI();
    }

    private void ResetRound()
    {
        SetFighterPositions();

        roundManager.HideRoundWinUI();

        RestoreFighter(gameManager.fighter1, true);
        RestoreFighter(gameManager.fighter2, false);
    }

    private void DisplayRoundAmount()
    {
        Transform roundWinsTransform = roundInfoCanvas.transform.Find("RoundWinsEmpty");

        fighter1RoundWins = roundWinsTransform.GetChild(0);
        fighter2RoundWins = roundWinsTransform.GetChild(1);

        /*
         * How rounds work if I forget:
         * Each fighter has a round, there is a possibility for each fighter to win, apart from the last round, where there must be a decisive winner, or it's a draw, this counts as 3 possibilities, so 3 rounds for that case,
         * Fighter 1 wins, Fighter 2 wins, or draw.
         * If there is a last round and each fighter has a win, there are only 2 possibilites, fighter 1 wins, or fighter 2 wins, if the whole match ends in a draw, the winner will be decided by how each fighter performed
        */

        if (roundManager.maxRounds == 1)
        {
            for (int i = 0; i < 1; i++)
            {
                fighter1RoundWins.GetChild(i).gameObject.SetActive(true);
                fighter2RoundWins.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (roundManager.maxRounds == 5)
        {
            for (int i = 0; i < 2; i++)
            {
                fighter1RoundWins.GetChild(i).gameObject.SetActive(true);
                fighter2RoundWins.GetChild(i).gameObject.SetActive(true);
            }
        }

        if (roundManager.maxRounds == 7)
        {
            for (int i = 0; i < 3; i++)
            {
                fighter1RoundWins.GetChild(i).gameObject.SetActive(true);
                fighter2RoundWins.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Match Handling
    private void CallMatchReferences()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.isLoaded)
        {
            cameraManager = Object.FindObjectOfType<FighterCameraManager>();
            roundManager = Object.FindObjectOfType<RoundManager>();
            fighterShowComboUIManager = Object.FindObjectOfType<FighterShowComboUIManager>();

            LoadMatchEssentials();
        }
    }

    private bool CheckIfMatchIsOver()
    {
        switch (roundManager.maxRounds)
        {
            case 7:
                if (roundManager.roundWinner[0] != string.Empty && roundManager.roundWinner[1] != string.Empty && roundManager.roundWinner[2] != string.Empty)
                {
                    return true;
                }
                break;
            case 5:
                if (roundManager.roundWinner[0] != string.Empty && roundManager.roundWinner[1] != string.Empty)
                {
                    return true;
                }
                break;
            case 1:
                if (roundManager.roundWinner[0] != string.Empty)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public override void PerformRoundOrMatchWin()
    {
        //print($"<color=#2a44f2>Match over: {CheckIfMatchIsOver()} </color>");

        if (!CheckIfMatchIsOver())
        {
            //print($"<b><size=14><color=#4920ff>Current round less than max rounds: {roundManager.currentRound}</color> Max rounds: {roundManager.maxRounds}</size></b>");
            if (gameManager.fighter1.isKnockedDown && roundManager.fighter1Victory)
            {
                roundManager.fighter1Victory = false;

                winningFighterName = gameManager.fighter2.GetComponent<EntityScript>().fighterName;
                StartCoroutine("KnockoutRoundOver");
            }

            if (gameManager.fighter2.isKnockedDown && roundManager.fighter2Victory)
            {
                roundManager.fighter2Victory = false;

                winningFighterName = gameManager.fighter1.GetComponent<EntityScript>().fighterName;
                StartCoroutine("KnockoutRoundOver");
            }
        }
        else
        {
            print("<b><size=14><color=#559e3f>Current round is the last round, time to end</color></size></b>");

            roundManager.SetMatchWinner();
        }
    }

    #endregion


    //Load stage
    private void LoadStageAtLaunch()
    {
        Scene activeStage = SceneManager.GetSceneByBuildIndex(gameManager.activeSceneIndex);
        SceneManager.SetActiveScene(activeStage);

        gameManager.AssignFighterEmptyTransform();
        gameManager.AssignFighters();
    }

    private void LoadMatchEssentials()
    {
        roundManager.roundInfoAnimator = roundInfoCanvas.GetComponentInChildren<Animator>();
        roundManager.roundWinText = gameCanvas.transform.Find("RoundWinDisplay").GetComponent<TextMeshProUGUI>();
        roundManager.roundTransition = gameCanvas.transform.Find("RoundTransition").gameObject;

        fighterShowComboUIManager.fighter1ComboText = gameCanvas.transform.Find("ComboManager1").GetComponentInChildren<TextMeshProUGUI>();
        fighterShowComboUIManager.fighter2ComboText = gameCanvas.transform.Find("ComboManager2").GetComponentInChildren<TextMeshProUGUI>();

        gameManager.activeSceneIndex = SceneManager.GetActiveScene().buildIndex;

        freezeStopManager.fighter1 = gameManager.fighter1;
        freezeStopManager.fighter2 = gameManager.fighter2;


        cameraManager.ChangeCamera(cameraManager.fightersTargetSuperCam, cameraManager.fightersTargetSuperCam);

        cameraManager.cameraNibHandler.gameManager = gameManager;

        DisplayRoundAmount();

        AssignManagersOnEntityLoad();
    }

    private IEnumerator KnockoutRoundOver()
    {
        yield return new WaitForSeconds(2);

        roundManager.KOUI();

        yield return new WaitForSeconds(1.2f);

        roundManager.ShowRoundWinUI(winningFighterName);

        yield return new WaitForSeconds(1);

        ResetRound();

        roundManager.StartRoundUI();
    }
}
