using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FighterBasics_Ground
{
    public Transform groundColliderTransform;

    public void SetGroundColliderTransform(EntityScript entity)
    {
        if (!groundColliderTransform)
        {
            groundColliderTransform = entity.transform.GetChild(4).GetChild(0);
        }

        entity.entityMovement.NewColliderTransform(groundColliderTransform);
    }

    public void SetGroundColliderActive(EntityScript entity, bool setActive)
    {
        if (!groundColliderTransform)
        {
            groundColliderTransform = entity.transform.GetChild(4).GetChild(0);
        }

        groundColliderTransform.gameObject.SetActive(setActive);
    }
}

//Holds everything basic that a fighter needs in the air, could change with other characters
[System.Serializable]
public class FighterBasics_Air
{
    public Transform airColliderTransform;

    public void CheckAir_LandedOnGround(EntityScript entity)
    {
        if (!airColliderTransform)
        {
            airColliderTransform = entity.transform.GetChild(4).GetChild(1);
        }

        if (airColliderTransform)
        {
            entity.entityMovement.NewColliderTransform(airColliderTransform);
        }
    }
}

//Has to load before everything else
[DefaultExecutionOrder(-100)]
public class FighterState : IState
{
    protected EntityScript entity;

    protected StateMachine_Entity stateMachine;

    public virtual void Start()
    {
        entity = GetComponentInParent<EntityScript>();
        stateMachine = GetComponentInParent<StateMachine_Entity>();
    }

    public void AssignNewPosition()
    {
        entity.entityMovement.movement.shouldAssignPosition = true;
    }

    private void DebugTargetCombo(AttackData cancellableAttack, AttackFighterState attackFighterState, AttackData currentAttackData)
    {
        print($"<color=#3a0dff> Cancellable attack: {cancellableAttack} </color>");
        print($"<color=#22f2ff> Attack fighter state: {attackFighterState} </color>");
        print($"<color=#212eff> Current attack data: {currentAttackData} </color>");
    }

    private void DebugCanChainCancel(bool currentAttackisChainCancellable, bool nextAttackisChainCancellable)
    {
        print($"Chain cancel variables: {entity.fighterInput.canAttack} {entity.fighterAttackManager.canPerformNormals} {currentAttackisChainCancellable} {nextAttackisChainCancellable}");
    }

    //TODO: Confirm you're actually pressing the button and that you aren't holding a direction
    public bool CheckTargetCombo(AttackData currentAttackData, TargetComboData tcData)
    {
        if (tcData.tcBuild.Length > 0)
        {
            int tcDataLength = tcData.tcBuild.Length - 1;

            if (entity.fighterComboManager.lastTC_Combo_Check <= tcDataLength)
            {
                entity.fighterComboManager.lastTC_Combo_Check = tcData.tcBuild[entity.fighterComboManager.lastTC_Combo_Check].TC_comboCheck;

                //Since arrays work from 0-max number, I have to abide by that

                //Current normal
                TCBuild currentNormalCheck = tcData.tcBuild[entity.fighterComboManager.lastTC_Combo_Check - 1];

                print($"Length of TC check - Current index: {entity.fighterComboManager.lastTC_Combo_Check} {currentNormalCheck.currentTC_attackData.name} TC Length: {tcDataLength}");

                //Next normal
                TCBuild nextNormalCheck = tcData.tcBuild[entity.fighterComboManager.lastTC_Combo_Check];

                if (currentNormalCheck != null)
                {
                    if (tcData.tc_ID != null && currentAttackData != null)
                    {
                        //Do not combo if in a chain cancel, only so if you checked this
                        if (!nextNormalCheck.currentTC_attackData.unrestrictedChainCancelTC)
                        {
                            if (entity.fighterAttackManager.currentlyChainCancelling)
                            {
                                Debug.LogWarning("Unrestricted chain cancel disallowed");

                                return false;
                            }
                        }

                        //Do not combo if part of a combo, only so if you checked this
                        if (!nextNormalCheck.currentTC_attackData.unrestrictedComboTC)
                        {
                            if (entity.fighterComboManager.currentlyInACombo)
                            {
                                Debug.LogWarning("Unrestricted combo cancel disallowed");

                                return false;
                            }
                        }

                        if (entity.fighterComboManager.lastTC_Combo_Check == nextNormalCheck.TC_comboCheck - 1)
                        {
                            //print($"Last combo check is: {entity.fighterComboManager.lastTC_Combo_Check}");
                            print($"Next normal check is: {nextNormalCheck.TC_comboCheck - 1}");

                            return true;
                        }
                    }
                }
            }
        }

        Debug.LogError($"<color=#ef3204> Something failed somewhere </color>");

        return false;
    }

    public virtual void CheckGrabs()
    {
    }

    public void ButtonNormalsInTargetCombo(AttackData attackData, AttackFighterState TCState, TargetComboData tcData)
    {
        //Only do this if this attack hit, checks constantly while this state is active, also don't think this should live here
        //Check from currentPerformedAttackEffect instead

        //print($"<color=#5f8022>TC State: {TCState}</color>");

        if (entity.fighterComboManager.currentPerformedAttackEffect)
        {
            //TODO: Obsolete way of doing so, especially at the start of a game. use a proper list instead

            //TODO: Check if button is actually being pressed

            int length = entity.fighterMotionInputManager.currentButtonInputStream.Count - 1;

            //TODO: Check current button being pressed within timeframe function

            if (entity.fighterMotionInputManager.currentButtonInputStream.Count > 1)
            {
                //Time since LAST button pressed
                if (entity.fighterMotionInputManager.CheckButtonValidAndPressedWithinTimeframe(TCState.attackData.fighterAttackButton, 0.6f, entity.fighterMotionInputManager.currentButtonInputStream[length - 1], entity.fighterMotionInputManager.currentButtonInputStream[length]))
                {
                    bool comboCheck = CheckTargetCombo(attackData, tcData);

                    //print($"Target combo check successful? {comboCheck}");

                    if (comboCheck && entity.stateMachine.currentState != TCState)
                    {
                        //print("<size=15><i><color=#2044f2>Button target combo iterate</color></i></size>");

                        stateMachine.ChangeState(TCState);
                    }
                    else
                    {
                    }
                }
            }
        }
    }

    public void CheckCancellableAttacks()
    {
        if (entity.fighterInput.specialCheckButton_lightPunch)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.LightPunch);
            CheckSpecialCancelAttacks(FighterAttackButtons.LightPunch);
        }

        if (entity.fighterInput.specialCheckButton_heavyPunch)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.HeavyPunch);
            CheckSpecialCancelAttacks(FighterAttackButtons.HeavyPunch);
        }

        if (entity.fighterInput.specialCheckButton_fiercePunch)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.FiercePunch);
            CheckSpecialCancelAttacks(FighterAttackButtons.FiercePunch);
        }

        if (entity.fighterInput.specialCheckButton_lightKick)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.LightKick);
            CheckSpecialCancelAttacks(FighterAttackButtons.LightKick);
        }

        if (entity.fighterInput.specialCheckButton_heavyKick)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.HeavyKick);
            CheckSpecialCancelAttacks(FighterAttackButtons.HeavyKick);
        }

        if (entity.fighterInput.specialCheckButton_fierceKick)
        {
            CheckSuperCancelAttacks(FighterAttackButtons.FierceKick);
            CheckSpecialCancelAttacks(FighterAttackButtons.FierceKick);
        }
    }

    public void RegisterCurrentPerformedAttackToComboManager(AttackData registerAttackData)
    {
        entity.fighterComboManager.currentPerformedAttack = registerAttackData;
    }

    public void RegisterLastPerformedAttackToComboManager(AttackData registerAttackData)
    {
        entity.fighterComboManager.currentPerformedAttackEffect = null;
        entity.fighterComboManager.lastPerformedAttack = registerAttackData;
    }

    public virtual void CheckChainCancelAttacks()
    {
        //DebugCanChainCancel(stateMachine.currentAttackStateData.chainCancellable, stateMachine.state_groundLightPunch.attackData.attackStateData.chainCancellable);

        if (!entity.fighterInput.canAttack && entity.fighterAttackManager.inChainCancelWindow)
        {
            if (stateMachine.currentAttackStateData.chainCancellable && !entity.fighterMotionInputManager.storedCommand && entity.fighterAttackManager.canPerformNormals)
            {
                //Only if that button is cancellable and combo is more than 1
                if (entity.fighterComboManager.combo_activeCount > 1)
                {
                    entity.fighterAttackManager.currentlyChainCancelling = true;
                }

                if (entity.fighterInput.button_lightPunch && stateMachine.state_groundLightPunch.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundLightPunch);
                }

                if (entity.fighterInput.button_heavyPunch && stateMachine.state_groundHeavyPunch.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundHeavyPunch);
                }

                if (entity.fighterInput.button_fiercePunch && stateMachine.state_groundFiercePunch.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundFiercePunch);
                }

                if (entity.fighterInput.button_lightKick && stateMachine.state_groundLightKick.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundLightKick);
                }

                if (entity.fighterInput.button_heavyKick && stateMachine.state_groundHeavyKick.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundHeavyKick);
                }

                if (entity.fighterInput.button_fierceKick && stateMachine.state_groundFierceKick.attackData.attackStateData.chainCancellable)
                {
                    this.stateMachine.ChangeState(stateMachine.state_groundFierceKick);
                }
            }
        }
    }

    public void CheckSpecialCancelAttacks(FighterAttackButtons buttonChecking)
    {
        if (entity.fighterComboManager.currentPerformedAttack)
        {
            if (!entity.fighterInput.canAttack && entity.fighterComboManager.currentPerformedAttack.specialsCancelListData)
            {
                if (stateMachine.currentAttackStateData.specialCancellable && entity.fighterAttackManager.canPerformSpecials)
                {
                    //Loop through cancellable attacks and check if they have motion inputs, if they do, see if the current motion and button input streams matches

                    AttackData[] cancellableAttacks = entity.fighterComboManager.currentPerformedAttack.specialsCancelListData.cancellableAttacks;

                    //Loop through cancellable attacks
                    foreach (AttackData attack in cancellableAttacks)
                    {
                        //Check motion inputs data length above 0
                        if (attack.motionInputsAttachedToAttackData.Length > 0)
                        {
                            //Check each motion inputs data length
                            for (int i = 0; i < attack.motionInputsAttachedToAttackData.Length; i++)
                            {
                                //Check each button to see if it matches buttonChecking

                                float buttonTimeLeniency = (Time.time - attack.motionInputsAttachedToAttackData[i].motionInputValues.timeLeniency) - Time.time;

                                if (buttonTimeLeniency <= attack.motionInputsAttachedToAttackData[i].motionInputValues.timeLeniency)
                                {
                                    if (entity.fighterMotionInputManager.CheckInputsString(buttonChecking.ToString()))
                                    {
                                        bool inputMotionCheck = entity.fighterMotionInputManager.CheckPreviousInputsInAttackData_SpecialCancels(attack.motionInputsAttachedToAttackData[i], buttonChecking); //loop through instead of checking the first index

                                        //print("<b> Completely matches inputs in a combo?: </b>" + inputMotionCheck);

                                        if (inputMotionCheck)
                                        {
                                            State_MotionCommand commandState = entity.fighterMotionInputManager.GetStateFromCommandID(attack.motionInputsAttachedToAttackData[i]);

                                            stateMachine.currentAttackStateData = attack.motionInputsAttachedToAttackData[i].motionInputValues.attackData.attackStateData;
                                            entity.fighterMotionInputManager.storedCommand = commandState;

                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void CheckSuperCancelAttacks(FighterAttackButtons buttonChecking)
    {
        /*
        print($"Can attack: {entity.fighterInput.canAttack}");
        print($"Current performed attack: {entity.fighterComboManager.currentPerformedAttack}");
        print($"Super cancels: {entity.fighterComboManager.currentPerformedAttack.superCancelListData}");
        */

        if (entity.fighterComboManager.currentPerformedAttack)
        {
            if (!entity.fighterInput.canAttack && entity.fighterComboManager.currentPerformedAttack.superCancelListData)
            {
                if (entity.fighterComboManager.attacked_Entity)
                {
                    if (stateMachine.currentAttackStateData.superCancellable && entity.fighterAttackManager.canPerformSuper)
                    {
                        //Loop through cancellable attacks and check if they have motion inputs, if they do, see if the current motion and button input streams matches

                        AttackData[] cancellableAttacks = entity.fighterComboManager.currentPerformedAttack.superCancelListData.cancellableAttacks;

                        //Loop through cancellable attacks
                        foreach (AttackData attack in cancellableAttacks)
                        {
                            //Check motion inputs data length above 0
                            if (attack.motionInputsAttachedToAttackData.Length > 0)
                            {
                                //Check each motion inputs data length
                                for (int i = 0; i < attack.motionInputsAttachedToAttackData.Length; i++)
                                {
                                    //Check each button to see if it matches buttonChecking

                                    float buttonTimeLeniency = (Time.time - attack.motionInputsAttachedToAttackData[i].motionInputValues.timeLeniency) - Time.time;

                                    if (buttonTimeLeniency <= attack.motionInputsAttachedToAttackData[i].motionInputValues.timeLeniency)
                                    {
                                        if (entity.fighterMotionInputManager.CheckInputsString(buttonChecking.ToString()))
                                        {
                                            bool inputMotionCheck = entity.fighterMotionInputManager.CheckPreviousInputsInAttackData_SuperCancels(attack.motionInputsAttachedToAttackData[i], buttonChecking); //loop through instead of checking the first index

                                            print("<b> Completely matches inputs in a combo?: </b>" + inputMotionCheck);

                                            if (inputMotionCheck)
                                            {
                                                State_MotionCommand commandState = entity.fighterMotionInputManager.GetStateFromCommandID(attack.motionInputsAttachedToAttackData[i]);

                                                stateMachine.currentAttackStateData = attack.motionInputsAttachedToAttackData[i].motionInputValues.attackData.attackStateData;
                                                entity.fighterMotionInputManager.storedCommand = commandState;

                                                print($"<size=24>Super cancel check</size>");

                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void GroundAttacks()
    {
        if (entity.fighterInput.button_lightPunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundLightPunch);
        }

        if (entity.fighterInput.button_heavyPunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundHeavyPunch);
        }

        if (entity.fighterInput.button_fiercePunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundFiercePunch);
        }

        if (entity.fighterInput.button_lightKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundLightKick);
        }

        if (entity.fighterInput.button_heavyKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundHeavyKick);
        }

        if (entity.fighterInput.button_fierceKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_groundFierceKick);
        }
    }

    private void AirAttacks()
    {
        if (entity.fighterInput.button_lightPunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_airLightPunch);
        }

        if (entity.fighterInput.button_heavyPunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_airHeavyPunch);
        }

        if (entity.fighterInput.button_fiercePunch)
        {
            this.stateMachine.ChangeState(stateMachine.state_airFiercePunch);
        }

        if (entity.fighterInput.button_lightKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_airLightKick);
        }

        if (entity.fighterInput.button_heavyKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_airHeavyKick);
        }

        if (entity.fighterInput.button_fierceKick)
        {
            this.stateMachine.ChangeState(stateMachine.state_airFierceKick);
        }
    }

    public virtual void CheckAttacks()
    {
        if (entity.fighterInput.canAttack && !entity.fighterMotionInputManager.storedCommand)
        {
            if (entity.entityMovement.groundChecker.grounded && entity.fighterInput.movement.y == 0)
            {
                GroundAttacks();
            }
            if (entity.entityMovement.groundChecker.grounded && entity.fighterInput.movement.y == -1)
            {
                //print("Crouch attack");
            }
            else if (!entity.entityMovement.groundChecker.grounded)
            {
                AirAttacks();
            }
        }
    }
}
