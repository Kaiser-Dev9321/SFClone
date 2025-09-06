using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//TODO: Motion input fixes for block stun
[RequireComponent(typeof(FighterMotionInputCommands))]
public class FighterMotionInputManager : MonoBehaviour
{
    [HideInInspector]
    public EntityScript entity;

    [HideInInspector]
    public StateMachine_Entity stateMachine;

    public List<FighterCardinalDirectionInput> currentMotionInputStream;
    public List<FighterButtonInput> currentButtonInputStream;

    [Space(25)]
    public FighterMotionInputCommands fighterMotionInputCommands;
    
    public FighterCardinalDirectionInput[] amountOfMatchedDirections;
    public FighterMotionInputData[] amountOfMatchedInputs;

    public List<State_MotionCommand> cancellableAttacks;

    private FighterBaseMotionInputButtonData voicelineButtonData;

    private float currentInputTime;

    private bool addedInputThisFrame = false; //Duct tape fix for motion input stream
    private bool addedButtonThisFrame = false; //Duct tape fix for motion button stream

    public IState storedCommand;

    
    public InputCardinalDirection GetCardinalDirection(float xDirection, float yDirection)
    {
        if (xDirection == 0 && yDirection == 0)
        {
            //None
            return InputCardinalDirection.None;
        }

        if (xDirection == -1 && yDirection == 1)
        {
            //UpBack
            return InputCardinalDirection.UpBack;
        }

        if (xDirection == -1 && yDirection == 0)
        {
            //Back
            return InputCardinalDirection.Back;
        }

        if (xDirection == -1 && yDirection == -1)
        {
            //DownBack
            return InputCardinalDirection.DownBack;
        }

        if (xDirection == 0 && xDirection == -1)
        {
            //Down
            return InputCardinalDirection.Down;
        }

        if (xDirection == 1 && yDirection == -1)
        {
            //DownForward
            return InputCardinalDirection.DownForward;
        }

        if (xDirection == 1 && yDirection == 0)
        {
            //Forward
            return InputCardinalDirection.Forward;
        }

        if (xDirection == 1 && yDirection == 1)
        {
            //UpForward
            return InputCardinalDirection.UpForward;
        }

        if (xDirection == 0 && yDirection == 1)
        {
            //Up
            return InputCardinalDirection.Up;
        }

        return InputCardinalDirection.None;
    }

    private void DebugCommandInputs(FighterBaseMotionInputButtonData command, bool partOfCombo)
    {
        if (!partOfCombo)
        {
            print($"<color=#ff9f00>Completed special move inputs. {command.testMessageToPrint}</color>");
        }
        else
        {
            print($"<color=#059f09>Completed special move cancel inputs. {command.testMessageToPrint} </color>");
        }
        print($"<color=#ff9f0a>Command animation to play: {command.motionInputValues.attackData.attackStateData.stateAnimation}</color>");
    }

    private void DebugCancellableContents(FighterBaseMotionInputButtonData attackDataCommand, State_MotionCommand commandState)
    {
        print($"Current attack data: {entity.fighterComboManager.currentPerformedAttack}");

        print("<color=#9302ff>Attack data command: </color>" + attackDataCommand);
        print("<color=#f0f056>Command state: </color>" + commandState);
        print("<color=#0256f2>Command state attack data: </color>" + commandState.attackData);

        if (!commandState.attackData)
        {
            Debug.LogError("No attack data on this command state: " + commandState);
        }
    }

    private void Start()
    {
        entity = GetComponent<EntityScript>();

        fighterMotionInputCommands = GetComponent<FighterMotionInputCommands>();
        stateMachine = GetComponent<StateMachine_Entity>();
    }

    private void Update()
    {
        if (currentMotionInputStream.Count > 32)
        {
            //print("Removing old input instances");
            currentMotionInputStream.RemoveAt(0); //The oldest instance
        }

        if (currentButtonInputStream.Count > 32)
        {
            //print("Removing old button instances");
            currentButtonInputStream.RemoveAt(0); //The oldest instance
        }

        if (storedCommand && !entity.fighterGameplayManager.freezeStopManager.freezeStopData)
        {
            PerformStoredCommand();
        }

        addedInputThisFrame = false;
        addedButtonThisFrame = false;
    }

    private void PerformVoiceline(FighterBaseMotionInputButtonData buttonData)
    {
        if (buttonData.motionInputValues.voicelineData)
        {
            //Voiceline GameObject
            if (transform.GetChild(2).Find(buttonData.motionInputValues.voicelineData.voicelineDataName) != null)
            {
                AudioSource voicelineToPlay;

                bool voicelineExists = transform.GetChild(2).Find(buttonData.motionInputValues.voicelineData.voicelineDataName).TryGetComponent<AudioSource>(out voicelineToPlay);

                if (voicelineExists)
                {
                    //print("Voiceline playing");
                    voicelineToPlay.Play();
                }
            }
        }

        voicelineButtonData = null;
    }

    //TODO: Include voicelines too
    public void PerformStoredCommand()
    {
        if (voicelineButtonData != null)
        {
            PerformVoiceline(voicelineButtonData);
        }

        stateMachine.ChangeState(storedCommand);

        storedCommand = null;
    }

    public void ResetInputsAndDirections()
    {
        amountOfMatchedDirections = new FighterCardinalDirectionInput[0];
        amountOfMatchedInputs = new FighterMotionInputData[0];
    }

    public void InputMotionEvent()
    {
        //print("Motion detected");

        if (!addedInputThisFrame)
        {
            DetectMotionInput();
        }
    }

    public void InputButtonEvent()
    {
        //print("Button detected");

        if (!addedButtonThisFrame)
        {
            DetectButtonInput();
        }
    }

    private void DebugTargetCombo(FighterAttackButtons button, FighterButtonInput currentButtonInput, float timeframeWithinPress, float timeToPress)
    {
        print($"<b>Buttons match - Expected: {button} Got: {currentButtonInput.button}</b>");
        print($"<color=#93e39f>Time: {timeframeWithinPress}</color> <color=#39ffff>{timeToPress}</color>");
    }

    public bool CheckButtonValidAndPressedWithinTimeframe(FighterAttackButtons button, float timeToPress, FighterButtonInput previousButtonInput, FighterButtonInput currentButtonInput)
    {
        if (button.ToString() == currentButtonInput.button.ToString())
        {
            //TODO: Figure out time between current input and previous input, if it matches, target combo, otherwise, not a target combo

            float timeframeWithinPress = currentButtonInput.button_timePressed - previousButtonInput.button_timePressed;

            //DebugTargetCombo(button, currentButtonInput, timeframeWithinPress, timeToPress);

            if (timeframeWithinPress <= timeToPress)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public State_MotionCommand GetStateFromCommandID(FighterBaseMotionInputButtonData buttonData)
    {
        State_MotionCommand debugCommand = null;

        foreach (State_MotionCommand command in stateMachine.state_MotionCommands)
        {
            debugCommand = command;

            if (buttonData.motionInputValues.motionCommandID == command.motionCommandID)
            {
                return command;
            }
        }

        Debug.LogWarning($"No such command matching this ID: {debugCommand}");

        return null;
    }

    //Adds all of this to a list of special cancellable list
    public void GetListOfCancellableAttacks(List<State_MotionCommand> comboCancelAttacks, CancelListData listData)
    {
        //Loop through the motion command list
        foreach (State_MotionCommand command in stateMachine.state_MotionCommands)
        {
            //Look through special cancellable attacks
            foreach (AttackData cancellableAttack in listData.cancellableAttacks)
            {
                //If command has attack data and the state matches, if not, the super state script doesn't have attack data (which I know yes, is redundant)
                if (command.attackData && command.attackData.attackStateData == cancellableAttack.attackStateData)
                {
                    //print("<size=14>Those attack data have the same name</size>");

                    //Confirms all these checks line up
                    if (!comboCancelAttacks.Contains(command))
                    {
                        //print("New command to contain");

                        //Add command to the list
                        comboCancelAttacks.Add(command);
                    }
                    else
                    {
                        //It was already there, skip over it
                        Debug.LogWarning("This command is already contained");
                    }
                }
            }
        }
    }

    //This serves as a temporary fix for it not comboing into moves if that button has a command normal
    //TODO: If this bypass does not work out, use a different data type for command normals

    public State_MotionCommand GetStateFromCancellableAttackCommandID(FighterBaseMotionInputButtonData buttonData, CancelListData listData)
    {
        cancellableAttacks.Clear();

        GetListOfCancellableAttacks(cancellableAttacks, listData);

        foreach (State_MotionCommand command in cancellableAttacks)
        {
            if (buttonData.motionInputValues.motionCommandID == command.motionCommandID)
            {
                //print($"<size=18><b>The motion command ID matches up with the combo cancel attacks</b></size>");

                return command;
            }
        }

        return null;
    }

    public bool CheckCanPerformStates(FighterBaseMotionInputButtonData buttonData, State_MotionCommand commandState)
    {
        if (buttonData.motionInputValues.mustPerformOnGround)
        {
            if (entity.entityMovement.groundChecker.grounded)
            {
                return true;
            }
        }
        else
        {
            return true;
        }

        if (buttonData.motionInputValues.mustPerformInAir)
        {
            if (!entity.entityMovement.groundChecker.grounded)
            {
                return true;
            }
        }

        return false;
    }

    private void CompareCommandInputToInputCommandData(FighterMotionInputData[] capturedInputs, FighterBaseMotionInputButtonData command)
    {
        int completeMatchedInputs = 0;

        for (int i = 0; i < command.motionInputValues.previousMotionInputData.Length; i++)
        {
            if (capturedInputs[i] == command.motionInputValues.previousMotionInputData[i])
            {
                completeMatchedInputs++;
            }
        }

        float addTimePerformed = 0;

        float currentButtonTime = Time.time;

        if (completeMatchedInputs == command.motionInputValues.previousMotionInputData.Length)
        {
            //Minus current button time each time between inputs
            for (int i = capturedInputs.Length - 1; i >= 0; i--)
            {
                addTimePerformed += amountOfMatchedDirections[i].input_timePressed;
            }

            addTimePerformed = currentButtonTime - (addTimePerformed / capturedInputs.Length);

            //Debug.LogError("READ FREEZE.");

            if (addTimePerformed <= command.motionInputValues.timeLeniency)
            {
                if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
                {
                    if (entity.fighterAttackManager.canPerformSpecials)
                    {
                        if (stateMachine.currentAttackStateData && stateMachine.currentAttackStateData.specialCancellable)
                        {
                            Debug.LogWarning($"There already is currentAttackStateData: {stateMachine.currentAttackStateData} which is special cancellable");

                            return;
                        }

                        entity.fighterAttackManager.inputtedMotionCommand = true;

                        State_MotionCommand commandState = GetStateFromCommandID(command);

                        //Stores command state in case if it's in hitstop
                        storedCommand = commandState;

                        voicelineButtonData = command; //Voiceline fix

                        //DebugCommandInputs(command, commandDebug, false);
                    }

                    amountOfMatchedInputs = new FighterMotionInputData[0];
                    amountOfMatchedDirections = new FighterCardinalDirectionInput[0];
                }
            }
        }
    }

    //TODO: 100% could be improved, this code is starting to become bad

    public bool CheckInputsString(string buttonString)
    {
        if (buttonString == "LightPunch")
        {
            entity.fighterInput.specialCheckButton_lightPunch = false;

            return true;
        }

        if (buttonString == "HeavyPunch")
        {
            entity.fighterInput.specialCheckButton_heavyPunch = false;

            return true;
        }

        if (buttonString == "FiercePunch")
        {
            entity.fighterInput.specialCheckButton_fiercePunch = false;

            return true;
        }

        if (buttonString == "LightKick")
        {
            entity.fighterInput.specialCheckButton_lightKick = false;

            return true;
        }

        if (buttonString == "HeavyKick")
        {
            entity.fighterInput.specialCheckButton_heavyKick = false;

            return true;
        }

        if (buttonString == "FierceKick")
        {
            entity.fighterInput.specialCheckButton_fierceKick = false;

            return true;
        }

        return false;
    }

    //Disable special/super check button on grounded or air
    public void DisableInputCancels()
    {
        if (entity && entity.fighterInput)
        {
            entity.fighterInput.specialCheckButton_lightPunch = false;
            entity.fighterInput.specialCheckButton_heavyPunch = false;
            entity.fighterInput.specialCheckButton_fiercePunch = false;
            entity.fighterInput.specialCheckButton_lightKick = false;
            entity.fighterInput.specialCheckButton_heavyKick = false;
            entity.fighterInput.specialCheckButton_fierceKick = false;
        }
    }

    public bool CaptureAttackDataMotionInputs_SpecialCancels(FighterMotionInputData[] capturedInputs, FighterBaseMotionInputButtonData attackDataCommand)
    {
        int completeMatchedInputs = 0;

        for (int i = 0; i < attackDataCommand.motionInputValues.previousMotionInputData.Length; i++)
        {
            if (capturedInputs[i] == attackDataCommand.motionInputValues.previousMotionInputData[i])
            {
                completeMatchedInputs++;
            }
        }

        float addTimePerformed = 0;

        float currentButtonTime = Time.time;

        if (completeMatchedInputs == attackDataCommand.motionInputValues.previousMotionInputData.Length)
        {
            //Minus current button time each time between inputs
            for (int i = capturedInputs.Length - 1; i >= 0; i--)
            {
                addTimePerformed += amountOfMatchedDirections[i].input_timePressed;
            }

            addTimePerformed = currentButtonTime - (addTimePerformed / capturedInputs.Length);

            if (addTimePerformed <= attackDataCommand.motionInputValues.timeLeniency)
            {
                if (entity.fighterAttackManager.canPerformSpecials)
                {
                    entity.fighterAttackManager.inputtedMotionCommand = true;

                    State_MotionCommand commandState = GetStateFromCancellableAttackCommandID(attackDataCommand, entity.fighterComboManager.currentPerformedAttack.specialsCancelListData);

                    stateMachine.currentAttackStateData = commandState.attackData.attackStateData;

                    if (attackDataCommand.motionInputValues.voicelineData)
                    {
                        voicelineButtonData = attackDataCommand; //Voiceline fix
                    }

                    //DebugCommandInputs(attackDataCommand, true);
                }

                amountOfMatchedInputs = new FighterMotionInputData[0];
                amountOfMatchedDirections = new FighterCardinalDirectionInput[0];

                return true;
            }
        }

        return false;
    }

    public bool CaptureAttackDataMotionInputs_SuperCancels(FighterMotionInputData[] capturedInputs, FighterBaseMotionInputButtonData attackDataCommand)
    {
        int completeMatchedInputs = 0;

        for (int i = 0; i < attackDataCommand.motionInputValues.previousMotionInputData.Length; i++)
        {
            if (capturedInputs[i] == attackDataCommand.motionInputValues.previousMotionInputData[i])
            {
                completeMatchedInputs++;
            }
        }

        float addTimePerformed = 0;

        float currentButtonTime = Time.time;

        if (completeMatchedInputs == attackDataCommand.motionInputValues.previousMotionInputData.Length)
        {
            //Minus current button time each time between inputs
            for (int i = capturedInputs.Length - 1; i >= 0; i--)
            {
                addTimePerformed += amountOfMatchedDirections[i].input_timePressed;
            }

            addTimePerformed = currentButtonTime - (addTimePerformed / capturedInputs.Length);

            if (addTimePerformed <= attackDataCommand.motionInputValues.timeLeniency)
            {
                if (entity.fighterAttackManager.canPerformSuper)
                {
                    entity.fighterAttackManager.inputtedMotionCommand = true;

                    State_MotionCommand commandState = GetStateFromCancellableAttackCommandID(attackDataCommand, entity.fighterComboManager.currentPerformedAttack.superCancelListData);

                    DebugCancellableContents(attackDataCommand, commandState);

                    stateMachine.currentAttackStateData = commandState.attackData.attackStateData;

                    if (attackDataCommand.motionInputValues.voicelineData)
                    {
                        voicelineButtonData = attackDataCommand; //Voiceline fix
                    }

                    //DebugCommandInputs(attackDataCommand, true);
                }

                amountOfMatchedInputs = new FighterMotionInputData[0];
                amountOfMatchedDirections = new FighterCardinalDirectionInput[0];

                return true;
            }
        }

        return false;
    }

    public bool CheckPreviousInputsInAttackData_SpecialCancels(FighterBaseMotionInputButtonData attackDataCommand, FighterAttackButtons buttonChecking)
    {
        if (attackDataCommand.RequireCondition())
        {
            //Assign new length to the new attack data command
            amountOfMatchedInputs = new FighterMotionInputData[attackDataCommand.motionInputValues.previousMotionInputData.Length];
            amountOfMatchedDirections = new FighterCardinalDirectionInput[attackDataCommand.motionInputValues.previousMotionInputData.Length];

            cancellableAttacks.Clear();

            GetListOfCancellableAttacks(cancellableAttacks, entity.fighterComboManager.currentPerformedAttack.specialsCancelListData);

            currentInputTime = Time.time;

            //Set matched input index to 0
            int matchedInputIndex = 0;

            //Go through input stream
            foreach (var input in currentMotionInputStream)
            {
                if (matchedInputIndex < amountOfMatchedInputs.Length && !entity.fighterAttackManager.inputtedMotionCommand)
                {
                    if (input.direction == attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex].directionToPress)
                    {
                        State_MotionCommand commandState = GetStateFromCancellableAttackCommandID(attackDataCommand, entity.fighterComboManager.currentPerformedAttack.specialsCancelListData);

                        //DebugCancellableContents(attackDataCommand, commandState);

                        //Check if the user can perform the state, the button matches, and the attack data is part of the combo cancellable attacks (haven't done that yet)
                        if (CheckCanPerformStates(attackDataCommand, commandState) && commandState.attackData.fighterAttackButton == buttonChecking && cancellableAttacks.Contains(GetStateFromCommandID(attackDataCommand)))
                        {
                            if (attackDataCommand.motionInputValues.commandType == CommandType.MotionInput)
                            {
                                float timeBetweenInputs = currentInputTime - input.input_timePressed;

                                //Go through the command to see if the direction matches, if yes and is within time leniency, matchedInputIndex increases by 1
                                if (timeBetweenInputs <= attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex].timeLeniency)
                                {
                                    /*
                                    print("<i><size=24>Print check test 1: </size></i>" + attackDataCommand);
                                    print("<i><size=20>Print check test 2: </size></i>" + attackDataCommand.previousMotionInputData[matchedInputIndex]);
                                    print("<b><i><size=18>Print check test 3: </size></i></b>" + matchedInputIndex);
                                    print("<b><i><size=16><color=#29ff00>Print check test 4: </color></size></i></b>" + amountOfMatchedInputs.Length);
                                    */

                                    amountOfMatchedInputs[matchedInputIndex] = attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex];
                                    amountOfMatchedDirections[matchedInputIndex] = input;

                                    //Matched input index + 1 so it matches this properly
                                    if (matchedInputIndex + 1 < attackDataCommand.motionInputValues.previousMotionInputData.Length)
                                    {
                                        matchedInputIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //If the matched input count matches the length, assume it is the command and capture the attack motion inputs
            if (amountOfMatchedInputs.Length == attackDataCommand.motionInputValues.previousMotionInputData.Length)
            {
                if (CaptureAttackDataMotionInputs_SpecialCancels(amountOfMatchedInputs, attackDataCommand))
                {
                    //print("<color=#859696>Capturing</color>");

                    //Return if it can capture the attack data motion inputs and it's in time range
                    return true;
                }
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    public bool CheckPreviousInputsInAttackData_SuperCancels(FighterBaseMotionInputButtonData attackDataCommand, FighterAttackButtons buttonChecking)
    {
        if (attackDataCommand.RequireCondition())
        {
            //Assign new length to the new attack data command
            amountOfMatchedInputs = new FighterMotionInputData[attackDataCommand.motionInputValues.previousMotionInputData.Length];
            amountOfMatchedDirections = new FighterCardinalDirectionInput[attackDataCommand.motionInputValues.previousMotionInputData.Length];

            cancellableAttacks.Clear();

            GetListOfCancellableAttacks(cancellableAttacks, entity.fighterComboManager.currentPerformedAttack.superCancelListData);

            currentInputTime = Time.time;

            //Set matched input index to 0
            int matchedInputIndex = 0;

            //Go through input stream
            foreach (var input in currentMotionInputStream)
            {
                if (matchedInputIndex < amountOfMatchedInputs.Length)
                {
                    if (input.direction == attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex].directionToPress)
                    {
                        State_MotionCommand commandState = GetStateFromCancellableAttackCommandID(attackDataCommand, entity.fighterComboManager.currentPerformedAttack.superCancelListData);

                        DebugCancellableContents(attackDataCommand, commandState);

                        //Check if the user can perform the state, the button matches, and the attack data is part of the combo cancellable attacks (haven't done that yet)
                        if (CheckCanPerformStates(attackDataCommand, commandState) && commandState.attackData.fighterAttackButton == buttonChecking && cancellableAttacks.Contains(GetStateFromCommandID(attackDataCommand)))
                        {
                            if (attackDataCommand.motionInputValues.commandType == CommandType.MotionInput)
                            {
                                float timeBetweenInputs = currentInputTime - input.input_timePressed;

                                //Go through the command to see if the direction matches, if yes and is within time leniency, matchedInputIndex increases by 1
                                if (timeBetweenInputs <= attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex].timeLeniency)
                                {
                                    /*
                                    print("<i><size=24>Print check test 1: </size></i>" + attackDataCommand);
                                    print("<i><size=20>Print check test 2: </size></i>" + attackDataCommand.previousMotionInputData[matchedInputIndex]);
                                    print("<b><i><size=18>Print check test 3: </size></i></b>" + matchedInputIndex);
                                    print("<b><i><size=16><color=#29ff00>Print check test 4: </color></size></i></b>" + amountOfMatchedInputs.Length);
                                    */

                                    amountOfMatchedInputs[matchedInputIndex] = attackDataCommand.motionInputValues.previousMotionInputData[matchedInputIndex];
                                    amountOfMatchedDirections[matchedInputIndex] = input;

                                    //Matched input index + 1 so it matches this properly
                                    if (matchedInputIndex + 1 < attackDataCommand.motionInputValues.previousMotionInputData.Length)
                                    {
                                        matchedInputIndex++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //If the matched input count matches the length, assume it is the command and capture the attack motion inputs
            if (amountOfMatchedInputs.Length == attackDataCommand.motionInputValues.previousMotionInputData.Length)
            {
                if (CaptureAttackDataMotionInputs_SuperCancels(amountOfMatchedInputs, attackDataCommand))
                {
                    //print("<color=#859696>Capturing</color>");

                    //Return if it can capture the attack data motion inputs and it's in time range
                    return true;
                }
            }
        }
        else
        {
            return false;
        }

        return false;
    }

    private void CheckEachPreviousInput(FighterBaseMotionInputButtonData command)
    {
        int matchedInputIndex = 0;

        amountOfMatchedInputs = new FighterMotionInputData[command.motionInputValues.previousMotionInputData.Length];
        amountOfMatchedDirections = new FighterCardinalDirectionInput[command.motionInputValues.previousMotionInputData.Length];

        foreach (var input in currentMotionInputStream)
        {
            //Check matched input index below the max length and if an inputted command motion is not already inputted
            if (matchedInputIndex < amountOfMatchedInputs.Length && !entity.fighterAttackManager.inputtedMotionCommand && !entity.fighterBlockManager.inBlockStun)
            {
                //DebugCommandInputs(command, false);

                //Check the direction to press
                if (input.direction == command.motionInputValues.previousMotionInputData[matchedInputIndex].directionToPress)
                {
                    //Check if motion input index matches
                    if (command.motionInputValues.previousMotionInputData[matchedInputIndex].motionInputIndex == matchedInputIndex + 1)
                    {
                        State_MotionCommand commandState = GetStateFromCommandID(command);

                        //Check if it matches if in air or ground
                        if (CheckCanPerformStates(command, commandState))
                        {
                            //Check time between inputs
                            float timeBetweenInputs = currentInputTime - input.input_timePressed;
                            //print($"<color=#9239f4> <i> Time between inputs: {timeBetweenInputs} </i> </color>");

                            //Check if time is lenient enough
                            if (timeBetweenInputs <= command.motionInputValues.previousMotionInputData[matchedInputIndex].timeLeniency)
                            {
                                amountOfMatchedInputs[matchedInputIndex] = command.motionInputValues.previousMotionInputData[matchedInputIndex];
                                amountOfMatchedDirections[matchedInputIndex] = input;

                                //print($"Matched input index: {matchedInputIndex}");

                                //print($"<color=#91b2f2> <size=14> Input is: {amountOfMatchedInputs[matchedInputIndex]} </size> </color>");

                                matchedInputIndex++;
                            }
                        }
                    }
                }
            }
        }

        if (amountOfMatchedInputs.Length == command.motionInputValues.previousMotionInputData.Length)
        {
            CompareCommandInputToInputCommandData(amountOfMatchedInputs, command);
        }
    }

    private void CheckCommandNormal(FighterBaseMotionInputButtonData command)
    {
        if (entity.fighterInput.movement.x == entity.actualDirectionX)
        {
            if (!entity.fighterGameplayManager.freezeStopManager.freezeStopData)
            {
                if (entity.fighterAttackManager.canPerformNormals && entity.fighterAttackManager.canPerformSpecials)
                {
                    if (stateMachine.currentAttackStateData && stateMachine.currentAttackStateData.specialCancellable || entity.fighterAttackManager.inputtedMotionCommand)
                    {
                        Debug.LogWarning($"There already is currentAttackStateData: {stateMachine.currentAttackStateData} which is special cancellable");

                        return;
                    }

                    entity.fighterAttackManager.inputtedMotionCommand = true;

                    State_MotionCommand commandState = GetStateFromCommandID(command);


                    if (CheckCanPerformStates(command, commandState))
                    {
                        //Stores command state in case if it's in hitstop
                        storedCommand = commandState;

                        voicelineButtonData = command; //Voiceline fix

                        //DebugCommandInputs(command, commandDebug, false);
                    }
                }

                amountOfMatchedInputs = new FighterMotionInputData[0];
                amountOfMatchedDirections = new FighterCardinalDirectionInput[0];
            }
        }
    }

    private void CheckMotionInputCommands(FighterButtonInput buttonInput)
    {
        currentInputTime = Time.time;

        //Check each motion input from command if it matches
        for (int i = 0; i < fighterMotionInputCommands.listOfMotionInputCommands.Length; i++)
        {
            fighterMotionInputCommands.listOfMotionInputCommands[i].entity = entity;

            if (fighterMotionInputCommands.listOfMotionInputCommands[i].RequireCondition())
            {
                if (fighterMotionInputCommands.listOfMotionInputCommands[i].motionInputValues.buttonToPress == buttonInput.button)
                {
                    if (fighterMotionInputCommands.listOfMotionInputCommands[i].motionInputValues.commandType == CommandType.MotionInput)
                    {
                        CheckEachPreviousInput(fighterMotionInputCommands.listOfMotionInputCommands[i]);
                    }
                    else if (fighterMotionInputCommands.listOfMotionInputCommands[i].motionInputValues.commandType == CommandType.CommandNormal)
                    {
                        CheckCommandNormal(fighterMotionInputCommands.listOfMotionInputCommands[i]);
                    }
                }
            }
        }

        /*
        //Check each motion input from command if it matches
        foreach (FighterBaseMotionInputButtonData command in fighterMotionInputCommands.listOfMotionInputCommands)
        {
            command.entity = entity;

            if (command.RequireCondition())
            {
                if (command.motionInputValues.buttonToPress == buttonInput.button)
                {
                    if (command.motionInputValues.commandType == CommandType.MotionInput)
                    {
                        CheckEachPreviousInput(command);
                    }
                    else if (command.motionInputValues.commandType == CommandType.CommandNormal)
                    {
                        CheckCommandNormal(command);
                    }
                }
            }
        }
        */
    }

    public void NewMotionInput()
    {
        FighterCardinalDirectionInput newMotionInput = new FighterCardinalDirectionInput();
        newMotionInput.rawXDirection = entity.fighterInput.movement.x * entity.GetActualXDirection();
        newMotionInput.rawYDirection = entity.fighterInput.movement.y;
        newMotionInput.input_timePressed = Time.time;

        newMotionInput.ConvertRawToCardinalDirection();

        currentMotionInputStream.Add(newMotionInput);

        addedInputThisFrame = true;
    }

    public void NewButtonInput()
    {
        FighterButtonInput newButtonInput = new FighterButtonInput();

        if (entity.fighterInput.button_lightPunch || entity.fighterInput.specialCheckButton_lightPunch)
        {
            newButtonInput.button = FighterAttackButtons.LightPunch;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        if (entity.fighterInput.button_heavyPunch || entity.fighterInput.specialCheckButton_heavyPunch)
        {
            newButtonInput.button = FighterAttackButtons.HeavyPunch;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        if (entity.fighterInput.button_fiercePunch || entity.fighterInput.specialCheckButton_fiercePunch)
        {
            newButtonInput.button = FighterAttackButtons.FiercePunch;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        if (entity.fighterInput.button_lightKick || entity.fighterInput.specialCheckButton_lightKick)
        {
            newButtonInput.button = FighterAttackButtons.LightKick;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        if (entity.fighterInput.button_heavyKick || entity.fighterInput.specialCheckButton_heavyKick)
        {
            newButtonInput.button = FighterAttackButtons.HeavyKick;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        if (entity.fighterInput.button_fierceKick || entity.fighterInput.specialCheckButton_fierceKick)
        {
            newButtonInput.button = FighterAttackButtons.FierceKick;
            newButtonInput.button_timePressed = Time.time;

            currentButtonInputStream.Add(newButtonInput);
            CheckMotionInputCommands(newButtonInput);
        }

        addedButtonThisFrame = true;
    }

    public void DetectMotionInput()
    {
        if (entity)
        {
            if (entity.fighterInput.movement.x != 0 || entity.fighterInput.movement.y != 0)
            {
                NewMotionInput();
            }
        }
    }

    public void DetectButtonInput()
    {
        if (entity)
        {
            NewButtonInput();
        }
    }
}