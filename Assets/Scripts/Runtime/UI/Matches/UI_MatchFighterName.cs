using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_MatchFighterName : MonoBehaviour
{
    private GameManager gameManager;
    public TextMeshProUGUI fighterNameText;

    public bool isFighter1;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        if (isFighter1)
        {
            fighterNameText.text = gameManager.fighter1.fighterName;
        }
        else
        {
            fighterNameText.text = gameManager.fighter2.fighterName;
        }
    }
}
