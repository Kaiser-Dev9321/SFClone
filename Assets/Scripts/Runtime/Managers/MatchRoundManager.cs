using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TODO: Put on a separate script
public class RoundManager : MonoBehaviour
{
    protected SceneLoader sceneLoader;
    protected FighterGameManager fighterGameManager;

    public TextMeshProUGUI roundWinText;
    public GameObject roundTransition;

    public UI_RoundInfo ui_RoundInfo;
    public Animator roundInfoAnimator;

    public GameObject fighter1StartingPosition;
    public GameObject fighter2StartingPosition;

    public int currentRound = 1;
    public int maxRounds = 5;

    public bool fighter1Victory = true;
    public bool fighter2Victory = true;

    public string[] roundWinner;

    protected string matchWinner;

    [Space(75)]
    public AudioSource announcerAudioSource;

    public AudioClip beginRoundClip;
    public AudioClip[] countdownClips = new AudioClip[10];
    public AudioClip fightClip;
    public AudioClip koClip;
    public AudioClip timeOverClip;


    #region Fighter Handling Virtual Functions
    public virtual void MakeFighterInactive(EntityScript entity)
    {

    }

    public virtual void MakeFighterActive(EntityScript entity)
    {

    }

    #endregion

    #region Match Handling Virtual Functions
    public virtual void StartRoundUI()
    {

    }

    public virtual void KOUI()
    {

    }

    public virtual void ShowRoundWinUI(string winningFighterName)
    {

    }

    public virtual void HideRoundWinUI()
    {

    }

    public virtual string FindFighterWinner()
    {
        return null;
    }

    public virtual void SetMatchFighterWinner(string fighterThatWon, Transform fighter1RoundWinsTransform, Transform fighter2RoundWinsTransform)
    {
    }

    public virtual void SetRoundWinner(Transform fighter1RoundWinsTransform, Transform fighter2RoundWinsTransform)
    {
    }

    public virtual void SetMatchWinner()
    {
    }

    public virtual void TransitionToNextRound()
    {
        //print("Next round!");

        roundTransition.SetActive(true);
    }

    public virtual void CompletedMatch()
    {
    }
    #endregion
}

public class MatchRoundManager : RoundManager
{
    private void Awake()
    {
        fighterGameManager = FindObjectOfType<FighterGameManager>();
        sceneLoader = FindObjectOfType<SceneLoader>();

        fighter1StartingPosition = GameObject.Find("Fighter1StartingPosition");
        fighter2StartingPosition = GameObject.Find("Fighter2StartingPosition");

        roundWinner = new string[maxRounds];

        for (int i = 0; i < roundWinner.Length; i++)
        {
            roundWinner[i] = string.Empty;
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
    }
    #endregion

    #region Round Handling
    public override void StartRoundUI()
    {
        roundInfoAnimator.Play("RoundInfo_StartRound");
    }

    public override void KOUI()
    {
        roundInfoAnimator.Play("RoundInfo_Knockout");
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

        print($"Round is: {currentRound}");
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
        }
        else
        {
            print("There was a draw");
        }

        currentRound = 1;

        Scene stageScene = SceneManager.GetActiveScene();
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
}
