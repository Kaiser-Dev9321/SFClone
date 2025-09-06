using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterGameplayManager : MonoBehaviour
{
    public Canvas gameCanvas;

    [HideInInspector]
    public GameObject fightersSuperEmpty;

    [HideInInspector]
    public FighterGameManager gameManager;

    [HideInInspector]
    public FighterCameraManager cameraManager;

    [HideInInspector]
    public RoundManager roundManager;

    [HideInInspector]
    public FighterShowComboUIManager fighterShowComboUIManager;

    public FreezeStopManager freezeStopManager;

    public Canvas roundInfoCanvas;

    public string winningFighterName;

    [HideInInspector]
    public Transform fighter1RoundWins;
    [HideInInspector]
    public Transform fighter2RoundWins;

    #region Match Handling Virtual Functions
    public virtual void PerformRoundOrMatchWin()
    {
    }
    #endregion
}