using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FighterGameManager : GameManager
{
    public int activeSceneIndex;

    public void LoadStage()
    {
        SceneManager.LoadScene(activeSceneIndex, LoadSceneMode.Additive);
    }

    public void AssignFighterEmptyTransform()
    {
        fightersEmptyTransform = GameObject.Find("Fighters").transform;
    }

    public void AssignFighters()
    {
        fighter1 = Instantiate(fighter1, fightersEmptyTransform);
        fighter1.name = fighter1.fighterName;

        fighter2 = Instantiate(fighter2, fightersEmptyTransform);
        fighter2.name = fighter2.fighterName;

        fighter1.tag = "Fighter1";
        fighter2.tag = "Fighter2";

        fighter1.FlipXDirection(false);
        fighter1.actualDirectionX = 1;

        fighter2.FlipXDirection(true);
        fighter2.actualDirectionX = -1;
    }
}
