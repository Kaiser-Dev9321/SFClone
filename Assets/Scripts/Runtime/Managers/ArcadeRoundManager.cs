using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeRoundManager : RoundManager
{
    //Move to RoundManager
    private WinQuotesManager winQuotesManager;

    public ArcadeRunData arcadeRunData;

    private void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        fighterGameManager = FindObjectOfType<FighterGameManager>();
        winQuotesManager = FindObjectOfType<WinQuotesManager>();
        arcadeRunData = FindObjectOfType<ArcadeRunData>();

        fighter1StartingPosition = GameObject.Find("Fighter1StartingPosition");
        fighter2StartingPosition = GameObject.Find("Fighter2StartingPosition");

        roundWinner = new string[maxRounds];

        for (int i = 0; i < roundWinner.Length; i++)
        {
            roundWinner[i] = string.Empty;
        }
    }

    private void GetAIComponents(EntityScript entity, bool enabled)
    {
        FighterAIController fighterAIController = entity.transform.GetComponent<FighterAIController>();
        AIFighterInput aiFighterInput = entity.transform.GetComponent<AIFighterInput>();

        if (fighterAIController)
        {
            fighterAIController.enabled = enabled;
        }

        if (aiFighterInput)
        {
            aiFighterInput.enabled = enabled;
        }
    }

    #region Fighter Handling
    public override void MakeFighterInactive(EntityScript entity)
    {
        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.enabled = false;
        }

        if (entity.fighterComboManager)
        {
            entity.fighterComboManager.enabled = false;
        }

        if (entity.fighterMotionInputManager)
        {
            entity.fighterMotionInputManager.enabled = false;
        }

        if (entity.fighterGrabManager)
        {
            entity.fighterGrabManager.enabled = false;
        }

        if (entity.entityMovement)
        {
            entity.entityMovement.enabled = false;
        }

        FighterAIController fighterAIController = entity.transform.GetComponent<FighterAIController>();

        GetAIComponents(entity, false);
    }

    public override void MakeFighterActive(EntityScript entity)
    {
        if (entity.fighterAttackManager)
        {
            entity.fighterAttackManager.enabled = true;
        }

        if (entity.fighterComboManager)
        {
            entity.fighterComboManager.enabled = true;
        }

        if (entity.fighterMotionInputManager)
        {
            entity.fighterMotionInputManager.enabled = true;
        }

        if (entity.fighterGrabManager)
        {
            entity.fighterGrabManager.enabled = true;
        }

        if (entity.entityMovement)
        {
            entity.entityMovement.enabled = true;
        }

        FighterAIController fighterAIController = entity.transform.GetComponent<FighterAIController>();

        GetAIComponents(entity, true);
    }
    #endregion

    #region Round Handling
    public override void StartRoundUI()
    {
        StartCoroutine(Announcer_RoundBegin());

        roundInfoAnimator.Play("RoundInfo_StartRound");
    }

    public override void KOUI()
    {
        announcerAudioSource.clip = koClip;
        announcerAudioSource.Play();

        roundInfoAnimator.Play("RoundInfo_Knockout");
    }

    //TODO: Tidy up announcer clips
    private IEnumerator Announcer_RoundBegin()
    {
        announcerAudioSource.clip = beginRoundClip;
        announcerAudioSource.Play();

        yield return new WaitForSeconds(0.5f);

        announcerAudioSource.clip = countdownClips[currentRound];
        announcerAudioSource.Play();

        yield return new WaitForSeconds(1.235f);

        announcerAudioSource.clip = fightClip;
        announcerAudioSource.Play();
    }

    public override void ShowRoundWinUI(string winningFighterName)
    {
        //print("Round win test print");

        roundWinText.gameObject.SetActive(true);
        roundWinText.text = $"{winningFighterName} wins";
    }

    public override void HideRoundWinUI()
    {
        roundWinText.gameObject.SetActive(false);
    }

    public override string FindFighterWinner()
    {
        if (fighter1Victory)
        {
            return "Fighter1";
        }
        else if (fighter2Victory)
        {
            return "Fighter2";
        }
        else if (!fighter1Victory && !fighter2Victory)
        {
            return "Draw";
        }

        return null;
    }

    private void SetRoundUIWinner(string fighterName, Transform winningFighterRoundWinUITransform, Transform losingFighterRoundWinUITransform)
    {
        winningFighterRoundWinUITransform.GetChild(currentRound - 1).GetComponent<Image>().color = new Color(0, 1, 0);
        losingFighterRoundWinUITransform.GetChild(currentRound - 1).GetComponent<Image>().color = new Color(1, 0, 0);
    }

    public override void SetMatchFighterWinner(string fighterThatWon, Transform fighter1RoundWinsTransform, Transform fighter2RoundWinsTransform)
    {
        if (fighterThatWon == "Fighter1")
        {
            SetRoundUIWinner(fighterThatWon, fighter1RoundWinsTransform, fighter2RoundWinsTransform);
        }
        else if (fighterThatWon == "Fighter2")
        {
            SetRoundUIWinner(fighterThatWon, fighter2RoundWinsTransform, fighter1RoundWinsTransform);
        }
        else if (fighterThatWon == "Draw")
        {

        }
    }

    public override void SetRoundWinner(Transform fighter1RoundWinsTransform, Transform fighter2RoundWinsTransform)
    {
        string fighterThatWon = FindFighterWinner();

        switch (currentRound)
        {
            case 4:
                roundWinner[3] = fighterThatWon;
                break;
            case 3:
                roundWinner[2] = fighterThatWon;
                break;
            case 2:
                roundWinner[1] = fighterThatWon;
                break;
            case 1:
                roundWinner[0] = fighterThatWon;
                break;
        }

        SetMatchFighterWinner(fighterThatWon, fighter1RoundWinsTransform, fighter2RoundWinsTransform);

        currentRound++;
        currentRound = Mathf.Clamp(currentRound, 1, maxRounds);
    }

    private string CountRoundsWon_DecideWinner()
    {
        int fighter1Wins = 0;
        int fighter2Wins = 0;

        for (int i = 0; i < maxRounds - 1; i++)
        {
            if (roundWinner[i] == "Fighter1")
            {
                fighter1Wins++;
            }
            else if (roundWinner[i] == "Fighter2")
            {
                fighter2Wins++;
            }
        }

        if (fighter1Wins > fighter2Wins)
        {
            return fighterGameManager.fighter1.fighterName;
        }
        else if (fighter2Wins > fighter1Wins)
        {
            return fighterGameManager.fighter2.fighterName;
        }
        else
        {
            return "Draw";
        }
    }

    private void DecideMatchWinner()
    {
        matchWinner = CountRoundsWon_DecideWinner();
    }

    public override void SetMatchWinner()
    {
        DecideMatchWinner();

        if (matchWinner != "Draw")
        {
            print($"{matchWinner} won the match");

            if (matchWinner == fighterGameManager.fighter1.fighterName)
            {
                print($"<i><size=20><color=#121299>Arcade win, time to move on</color></size></i>");

                winQuotesManager.LoadQuote(fighterGameManager.fighter1, fighterGameManager.fighter2);

                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
                SceneManager.UnloadSceneAsync("_arcadegameplaypreload");

                print($"Has next arcade ladder: {arcadeRunData.fighterArcadeModeData.hasNextArcadeLadder}");
                print($"Current opponent: {arcadeRunData.currentOpponentEntity.gameObject}\nArcade mode opponent: {arcadeRunData.fighterArcadeModeData.currentOpponent}");

                if (!arcadeRunData.fighterArcadeModeData.hasNextArcadeLadder && arcadeRunData.currentOpponentEntity.gameObject == arcadeRunData.fighterArcadeModeData.currentOpponent)
                {
                    print($"<size=18><b>ARCADE GAME OVER</b></size>");

                    ResetArcadeData(arcadeRunData);

                    sceneLoader.LoadScene("ArcadeGameOver");
                }
                else
                {
                    AsyncOperation postMatchStageLoadAsync = sceneLoader.LoadSceneAsync("Stage_PostMatchStage", LoadSceneMode.Additive);

                    postMatchStageLoadAsync.completed += PostMatchStageLoadAsync_completed;
                }

                if (arcadeRunData.fighterArcadeModeData.hasNextArcadeLadder)
                {
                    SetNextStageData(arcadeRunData);
                }
            }
            else
            {
                print($"<i><size=20><color=#100099>Arcade lost, back to main menu</color></size></i>");

                ResetArcadeData(arcadeRunData);

                sceneLoader.LoadScene("MainMenu");
            }
        }
        else
        {
            print("There was a draw");
        }

        currentRound = 1;
    }

    private void PostMatchStageLoadAsync_completed(AsyncOperation obj)
    {
        Scene postMatchStage = SceneManager.GetSceneByName("Stage_PostMatchStage");

        SceneManager.SetActiveScene(postMatchStage);
    }

    public override void TransitionToNextRound()
    {
        //print("Next round!");

        roundTransition.SetActive(true);
    }

    public override void CompletedMatch()
    {
        print("Match finished!");
    }
    #endregion

    private void SetNextStageData(ArcadeRunData arcadeRunData)
    {
        arcadeRunData.currentArcadeStageIndex++;
        arcadeRunData.fighterArcadeModeData = arcadeRunData.fighterArcadeModeData.nextArcadeLadder;
        arcadeRunData.currentOpponentEntity = arcadeRunData.fighterArcadeModeData.currentOpponent.GetComponent<EntityScript>();
    }

    private void ResetArcadeData(ArcadeRunData arcadeRunData)
    {
        arcadeRunData.currentArcadeStageIndex = 0;
        arcadeRunData.arcadeCharacter = null;
        arcadeRunData.fighterArcadeModeData = null;
        arcadeRunData.currentOpponentEntity = null;
    }
}
