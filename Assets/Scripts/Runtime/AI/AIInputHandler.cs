using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIButtonPressContext
{
    None,
    Started,
    Performed,
    Cancelled
}

public class AIInputHandler : MonoBehaviour
{
    #region Button Contexts

    #region Light Punch
    private bool lightPunch_Started;
    private bool lightPunch_Performed;
    private bool lightPunch_Cancelled;
    private bool lightPunch_currentlyCancelled;
    #endregion

    #region Light Kick
    private bool lightKick_Started;
    private bool lightKick_Performed;
    private bool lightKick_Cancelled;
    private bool lightKick_currentlyCancelled;
    #endregion

    #region Heavy Punch
    private bool heavyPunch_Started;
    private bool heavyPunch_Performed;
    private bool heavyPunch_Cancelled;
    private bool heavyPunch_currentlyCancelled;
    #endregion

    #region Heavy Kick
    private bool heavyKick_Started;
    private bool heavyKick_Performed;
    private bool heavyKick_Cancelled;
    private bool heavyKick_currentlyCancelled;
    #endregion

    #region Fierce Punch
    private bool fiercePunch_Started;
    private bool fiercePunch_Performed;
    private bool fiercePunch_Cancelled;
    private bool fiercePunch_currentlyCancelled;
    #endregion

    #region Fierce Kick
    private bool fierceKick_Started;
    private bool fierceKick_Performed;
    private bool fierceKick_Cancelled;
    private bool fierceKick_currentlyCancelled;
    #endregion

    #endregion

    public FighterAIController fighterAIController;

    private void Start()
    {
        fighterAIController = GetComponent<FighterAIController>();
    }

    private void UpdateButtonsContext()
    {
        if (fighterAIController.fighterEntity.fighterInput.button_lightPunch)
        {
            lightPunch_Cancelled = false;

            if (!lightPunch_Started && !lightPunch_Performed && !lightPunch_Cancelled)
            {
                lightPunch_Started = true;
                lightPunch_Performed = false;

                print("Light punch started");
            }
            else if (lightPunch_Started && !lightPunch_Performed && !lightPunch_Performed)
            {
                lightPunch_Started = false;
                lightPunch_Performed = true;

                print("Light punch performed");
            }
        }
        else
        {
            if (!lightPunch_Cancelled && !lightPunch_currentlyCancelled)
            {
                lightPunch_Cancelled = true;

                lightPunch_Started = false;
                lightPunch_Performed = false;
            }
            else if (lightPunch_Cancelled && lightPunch_currentlyCancelled)
            {
                lightPunch_Cancelled = false;
                lightPunch_currentlyCancelled = false;
            }
        }

        if (fighterAIController.fighterEntity.fighterInput.button_lightKick)
        {
            lightKick_Cancelled = false;

            if (!lightKick_Started && !lightKick_Performed && !lightKick_Cancelled)
            {
                lightKick_Started = true;
                lightKick_Performed = false;
            }
            else if (lightKick_Started && !lightKick_Performed && !lightKick_Cancelled)
            {
                lightKick_Started = false;
                lightKick_Performed = true;
            }
        }
        else
        {
            if (!lightKick_Cancelled)
            {
                lightKick_Cancelled = true;

                lightKick_Started = false;
                lightKick_Performed = false;
            }
            else if (lightKick_Cancelled && lightKick_currentlyCancelled)
            {
                lightKick_Cancelled = false;
                lightKick_currentlyCancelled = false;
            }
        }

        if (fighterAIController.fighterEntity.fighterInput.button_heavyPunch)
        {
            heavyPunch_Cancelled = false;

            if (!heavyPunch_Started && !heavyPunch_Performed && !heavyPunch_Cancelled)
            {
                heavyPunch_Started = true;
                heavyPunch_Performed = false;
            }
            else if (heavyPunch_Started && !heavyPunch_Performed && !heavyPunch_Performed)
            {
                heavyPunch_Started = false;
                heavyPunch_Performed = true;
            }
        }
        else
        {
            if (!heavyPunch_Cancelled)
            {
                heavyPunch_Cancelled = true;

                heavyPunch_Started = false;
                heavyPunch_Performed = false;
            }
            else if (heavyPunch_Cancelled && heavyPunch_currentlyCancelled)
            {
                heavyPunch_Cancelled = false;
                heavyPunch_currentlyCancelled = false;
            }
        }


        if (fighterAIController.fighterEntity.fighterInput.button_heavyKick)
        {
            heavyKick_Cancelled = false;

            if (!heavyKick_Started && !heavyKick_Performed && !heavyKick_Cancelled)
            {
                heavyKick_Started = true;
                heavyKick_Performed = false;
            }
            else if (heavyKick_Started && !heavyKick_Performed && !heavyKick_Performed)
            {
                heavyKick_Started = false;
                heavyKick_Performed = true;
            }
        }
        else
        {
            if (!heavyKick_Cancelled)
            {
                heavyKick_Cancelled = true;

                heavyKick_Started = false;
                heavyKick_Performed = false;
            }
            else if (heavyKick_Cancelled && heavyKick_currentlyCancelled)
            {
                heavyKick_Cancelled = false;
                heavyKick_currentlyCancelled = false;
            }
        }


        if (fighterAIController.fighterEntity.fighterInput.button_fiercePunch)
        {
            fiercePunch_Cancelled = false;

            if (!fiercePunch_Started && !fiercePunch_Performed && !fiercePunch_Cancelled)
            {
                fiercePunch_Started = true;
                fiercePunch_Performed = false;
            }
            else if (fiercePunch_Started && !fiercePunch_Performed && !fiercePunch_Performed)
            {
                fiercePunch_Started = false;
                fiercePunch_Performed = true;
            }
        }
        else
        {
            if (!fiercePunch_Cancelled)
            {
                fiercePunch_Cancelled = true;

                fiercePunch_Started = false;
                fiercePunch_Performed = false;
            }
            else if (fiercePunch_Cancelled && fiercePunch_currentlyCancelled)
            {
                fiercePunch_Cancelled = false;
                fiercePunch_currentlyCancelled = false;
            }
        }


        if (fighterAIController.fighterEntity.fighterInput.button_fierceKick)
        {
            fierceKick_Cancelled = false;

            if (!fierceKick_Started && !fierceKick_Performed && !fierceKick_Cancelled)
            {
                fierceKick_Started = true;
                fierceKick_Performed = false;
            }
            else if (fierceKick_Started && !fierceKick_Performed && !fierceKick_Performed)
            {
                fierceKick_Started = false;
                fierceKick_Performed = true;
            }
        }
        else
        {
            if (!fierceKick_Cancelled)
            {
                fierceKick_Cancelled = true;

                fierceKick_Started = false;
                fierceKick_Performed = false;
            }
            else if (fierceKick_Cancelled && fierceKick_currentlyCancelled)
            {
                fierceKick_Cancelled = false;
                fierceKick_currentlyCancelled = false;
            }
        }
    }

    private void UpdateAIFighterInput(Vector2 movementInput)
    {
        fighterAIController.aiFighterInput.OnFighterInputMove(movementInput);
        fighterAIController.fighterEntity.fighterMotionInputManager.InputMotionEvent();
    }

    //Pass button to a function that puts it as none, started, performed or cancelled
    private void UpdateAIFighterButtons()
    {
        //Handles these functions separately since these buttons are separate in the fighter input
        #region Button Context Functions

        if (lightPunch_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightPunch(AIButtonPressContext.Started);
        }
        else if (lightPunch_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightPunch(AIButtonPressContext.Performed);
        }
        else if (lightPunch_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightPunch(AIButtonPressContext.Cancelled);
        }

        if (lightKick_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightKick(AIButtonPressContext.Started);
        }
        else if (lightKick_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightKick(AIButtonPressContext.Performed);
        }
        else if (lightKick_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonLightKick(AIButtonPressContext.Cancelled);
        }

        if (heavyPunch_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyPunch(AIButtonPressContext.Started);
        }
        else if (heavyPunch_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyPunch(AIButtonPressContext.Performed);
        }
        else if (heavyPunch_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyPunch(AIButtonPressContext.Cancelled);
        }

        if (heavyKick_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyKick(AIButtonPressContext.Started);
        }
        else if (heavyKick_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyKick(AIButtonPressContext.Performed);
        }
        else if (heavyKick_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonHeavyKick(AIButtonPressContext.Cancelled);
        }

        if (fiercePunch_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFiercePunch(AIButtonPressContext.Started);
        }
        else if (fiercePunch_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFiercePunch(AIButtonPressContext.Performed);
        }
        else if (fiercePunch_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFiercePunch(AIButtonPressContext.Cancelled);
        }

        if (fierceKick_Started)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFiercePunch(AIButtonPressContext.Started);
        }
        else if (fierceKick_Performed)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFierceKick(AIButtonPressContext.Performed);
        }
        else if (fierceKick_Cancelled)
        {
            fighterAIController.aiFighterInput.OnFighterButtonFierceKick(AIButtonPressContext.Cancelled);
        }

        fighterAIController.fighterEntity.fighterMotionInputManager.InputButtonEvent();

        #endregion
    }

    private void Update()
    {
        if (fighterAIController.aiLoaded)
        {
            /*
            print($"<size=19><color=#292900>Fighter AI Controller: {fighterAIController != null}</color></size>");
            print($"<size=19><color=#292900>Fighter AI Entity: {fighterAIController.fighterEntity != null}</color></size>");
            print($"<size=19><color=#292900>Fighter Input: {fighterAIController.fighterEntity.fighterInput != null}</color></size>");
            print($"<size=19><color=#292900>Fighter AI fighter Input: {fighterAIController.aiFighterInput != null}</color></size>");
            */

            UpdateButtonsContext();

            UpdateAIFighterInput(fighterAIController.fighterEntity.fighterInput.movement);
            UpdateAIFighterButtons();
        }
    }
}
