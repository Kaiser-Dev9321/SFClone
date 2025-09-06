using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAttackManager : MonoBehaviour
{
    [HideInInspector]
    public EntityScript entity;

    [HideInInspector]
    public FighterInput fighterInput;
    public bool inputtedMotionCommand;
    public bool canPerformNormals = true;
    public bool canPerformSpecials = true;
    public bool canPerformSuper = true;
    public bool inChainCancelWindow = false;

    public bool currentlyChainCancelling;

    [HideInInspector]
    public FighterComboManager fighterComboManager;

    public int currentDamageScaling = 0;

    private void Awake()
    {
        transform.GetComponent<EntityEventLoader>().entityEssentialsLoadedEvent += FighterAttackManager_entityEssentialsLoadedEvent;
    }

    private void FighterAttackManager_entityEssentialsLoadedEvent()
    {
        LoadAttackManagerEssentials();
    }

    private void LoadAttackManagerEssentials()
    {
        entity = GetComponent<EntityScript>();
        fighterInput = GetComponent<FighterInput>();
        fighterComboManager = GetComponent<FighterComboManager>();
    }

    public void ResetDamageScaling()
    {
        //print($"<size=14>Reset damage scaling</size>");
        currentDamageScaling = 0;
    }

    public int DamageScaling(float currentEnemyHealth, float currentDamage, int scalePoint)
    {
        //Don't know if I should do this for blocking
        if (currentDamageScaling > entity.damageScaleManager.damageScalePercents.Length)
        {
            currentDamageScaling = entity.damageScaleManager.damageScalePercents.Length - 1;
        }

        //print($"Damage scaling: {currentDamageScaling}");

        float currentDamageScaled = currentDamage;

        float damagePercentCheck = entity.damageScaleManager.damageScalePercents[currentDamageScaling];

        if (entity.fighterComboManager.combo_activeCount > 1)
        {
            //print("Scale after first hit");

            currentDamageScaling += scalePoint; //Modifier, possibly needs more involvement in the equation, seems left out

            currentDamageScaling = Mathf.Clamp(currentDamageScaling, 0, entity.damageScaleManager.damageScalePercents.Length - 1);

            currentDamageScaled = currentDamage / (100f / damagePercentCheck); //Equation
        }

        currentDamageScaled = Mathf.FloorToInt(currentDamageScaled);

        return (int)currentDamageScaled;
    }

    public bool CheckAttackButtons(AttackData attackData, string buttonWanted)
    {
        if (attackData.fighterAttackButton.ToString() == buttonWanted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetPerformNormals(int value)
    {
        if (value == 0)
        {
            canPerformNormals = false;
        }
        
        //Regular enable which also enables inChainCancelWindow
        if (value == 1)
        {
            canPerformNormals = true;
            inChainCancelWindow = true;
        }

        //For enabling while in hitstop, separate function for chain cancelling a few frames later
        if (value == 2)
        {
            canPerformNormals = true;
        }
    }

    public void SetChainCancelWindow(int value)
    {
        if (value == 0)
        {
            inChainCancelWindow = false;
        }
        else
        {
            inChainCancelWindow = true;
        }
    }

    public void SetPerformSpecials(int value)
    {
        if (value == 0)
        {
            canPerformSpecials = false;
        }

        if (value == 1)
        {
            canPerformSpecials = true;
        }
    }
    
    public void SetPerformSuper(int value)
    {
        if (value == 0)
        {
            canPerformSuper = false;
        }

        if (value == 1)
        {
            canPerformSuper = true;
        }
    }

    public bool CanPerformNormalsAndCanAttack()
    {
        if (fighterInput.canAttack && !inputtedMotionCommand && canPerformNormals)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanPerformSpecialsAndCanAttack()
    {
        if (fighterInput.canAttack && !inputtedMotionCommand && canPerformSpecials)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
