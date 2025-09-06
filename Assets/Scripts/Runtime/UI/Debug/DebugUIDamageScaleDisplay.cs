using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugUIDamageScaleDisplay : MonoBehaviour
{
    public FighterGameplayManager gameplayManager;
    public GameManager gameManager;

    public TextMeshProUGUI damageScaleDisplayText;

    private int damagePercentToRead;

    public bool isFighter1;

    private void Start()
    {
        gameplayManager = FindObjectOfType<FighterGameplayManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private bool ShouldClampDamageDisplay(EntityScript entity)
    {
        /*
        print($"Entity: {entity.transform}");
        print($"Entity fighter attack manager: {entity.fighterAttackManager}");
        print($"Entity fighter damage scale manager: {entity.damageScaleManager}");
        */

        if (entity.fighterAttackManager.currentDamageScaling >= entity.damageScaleManager.damageScalePercents.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetFighterDamageDisplay(EntityScript fighter)
    {
        if (!ShouldClampDamageDisplay(fighter))
        {
            damagePercentToRead = fighter.damageScaleManager.damageScalePercents[fighter.fighterAttackManager.currentDamageScaling];
        }
        else
        {
            damagePercentToRead = fighter.damageScaleManager.damageScalePercents[fighter.damageScaleManager.damageScalePercents.Length - 1];
        }

        if (damagePercentToRead >= 50)
        {
            //print($"Around 51 - 100: {damagePercentToRead}");
            damageScaleDisplayText.color = Color.Lerp(Color.green, new Color(1, 0.5f, 0), 1 - damagePercentToRead / 100);
        }
        else if (damagePercentToRead >= 0)
        {
            //print("Around 0 - 50");
            damageScaleDisplayText.color = Color.Lerp(new Color(1, 0.5f, 0), Color.red, 1 - (damagePercentToRead / 100 / 2));
        }

        damageScaleDisplayText.text = $"{damagePercentToRead}%";
    }

    private void Update()
    {
        //Don't clamp if damage percent is below length, else, clamp
        if (isFighter1)
        {
            SetFighterDamageDisplay(gameManager.fighter1);
        }
        else
        {
            SetFighterDamageDisplay(gameManager.fighter2);
        }
    }
}
