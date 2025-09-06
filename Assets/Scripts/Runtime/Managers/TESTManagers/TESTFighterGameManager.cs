using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TESTFighterGameManager : GameManager
{
    public void AssignFighterEmptyTransform()
    {
        fightersEmptyTransform = GameObject.Find("Fighters").transform;
    }

    private void Start()
    {
        AssignFighterEmptyTransform();
        AssignFighters();
    }

    public void LoadAIComponents(EntityScript entity)
    {
        FighterAIController fighterAIController = entity.transform.GetComponent<FighterAIController>();
        AIFighterInput aiFighterInput = entity.transform.GetComponent<AIFighterInput>();

        if (fighterAIController)
        {
            fighterAIController.enabled = true;
        }

        if (aiFighterInput)
        {
            aiFighterInput.enabled = true;
        }
    }

    public void AssignFighters()
    {
        fighter1.name = fighter1.fighterName;
        fighter2.name = fighter2.fighterName;

        fighter1.tag = "Fighter1";
        fighter2.tag = "Fighter2";

        fighter1.FlipXDirection(false);
        fighter2.FlipXDirection(true);

        fighter1.LoadEntityReferences();
        fighter2.LoadEntityReferences();

        LoadAIComponents(fighter1);
        LoadAIComponents(fighter2);
    }

    public override void PutFighterBackInFightersEmptyTransform(Transform fighterTransform)
    {
        fightersEmptyTransform.parent = fightersEmptyTransform;
    }
}
