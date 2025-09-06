using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFighterGameplayManager : FighterGameplayManager
{
    private GameManager testGameManager;

    public Timer healthUpdateTimer1, healthUpdateTimer2;

    private void Awake()
    {
        testGameManager = Object.FindObjectOfType<GameManager>();
        fightersSuperEmpty = gameCanvas.transform.Find("FightersSuperEmpty").gameObject;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        healthUpdateTimer1 = new Timer(0, 2);
        healthUpdateTimer2 = new Timer(0, 2);

        print("Testing assign func 1");

        AssignManagersOnEntityLoad();
    }

    #region Load Fighters
    public void AssignManagersOnEntityLoad()
    {
        print("Testing assign func 2");

        testGameManager.fighter1.gameObject.SetActive(true);
        testGameManager.fighter2.gameObject.SetActive(true);

        testGameManager.fighter1.entityEventLoader.entityLoadedEvent += LoadFighter1;
        testGameManager.fighter2.entityEventLoader.entityLoadedEvent += LoadFighter2;
    }

    private void LoadFighter1()
    {
        testGameManager.fighter1.LoadEntityReferences();
        testGameManager.fighter1.fighterSuperManager.ui_SuperMeter = fightersSuperEmpty.transform.GetChild(0).GetComponent<UI_SuperMeter>();

        RestoreFighter(testGameManager.fighter1, true);
    }

    private void LoadFighter2()
    {
        testGameManager.fighter2.LoadEntityReferences();
        testGameManager.fighter2.fighterSuperManager.ui_SuperMeter = fightersSuperEmpty.transform.GetChild(1).GetComponent<UI_SuperMeter>();

        RestoreFighter(testGameManager.fighter2, false);
    }

    private void RestoreFighter(EntityScript fighter, bool isFighter1)
    {
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

    #endregion


    private bool FighterInHitstun(EntityScript entity)
    {
        if (entity.entityHitstun.hitStunned)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (healthUpdateTimer1.currentTime <= 0)
        {
            if (testGameManager.fighter1.entityHealth.currentHealth > 0)
            {
                testGameManager.fighter1.entityHealth.SetHealthToMaxHealth();
            }

            healthUpdateTimer1.SetToTimer();
        }
        else
        {
            if (!FighterInHitstun(testGameManager.fighter1))
            {
                healthUpdateTimer1.DecreaseByTick();
            }
            else
            {
                healthUpdateTimer1.SetToTimer();
            }
        }


        if (healthUpdateTimer2.currentTime <= 0)
        {
            if (testGameManager.fighter2.entityHealth.currentHealth > 0)
            {
                testGameManager.fighter2.entityHealth.SetHealthToMaxHealth();
            }

            healthUpdateTimer2.SetToTimer();
        }
        else
        {
            if (!FighterInHitstun(testGameManager.fighter2))
            {
                healthUpdateTimer2.DecreaseByTick();
            }
            else
            {
                healthUpdateTimer2.SetToTimer();
            }
        }
    }
}
