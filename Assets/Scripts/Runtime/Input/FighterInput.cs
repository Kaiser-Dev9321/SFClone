using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IEntityButtonInput
{
    public Vector2 movement { get; set; }

    public bool button_lightPunch { get; set; }
    public bool button_heavyPunch { get; set; }
    public bool button_fiercePunch { get; set; }

    public bool button_lightKick { get; set; }
    public bool button_heavyKick { get; set; }
    public bool button_fierceKick { get; set; }
}

public interface IEntityInputMovementFunctions
{
    public void OnFighterInputMove(Vector2 input);
}

public interface IPlayerInputMovementFunctions
{
    public void OnFighterInputMove(InputAction.CallbackContext context);
}

public interface IEntityInputButtonFunctions
{
    public void OnFighterButtonLightPunch(bool lightPunch);
    public void OnFighterButtonHeavyPunch(bool heavyPunch);
    public void OnFighterButtonFiercePunch(bool fiercePunch);
    public void OnFighterButtonLightKick(bool lightKick);
    public void OnFighterButtonHeavyKick(bool heavyKick);
    public void OnFighterButtonFierceKick(bool fierceKick);
}

public interface IPlayerInputButtonFunctions
{
    public void OnFighterButtonLightPunch(InputAction.CallbackContext context);
    public void OnFighterButtonHeavyPunch(InputAction.CallbackContext context);
    public void OnFighterButtonFiercePunch(InputAction.CallbackContext context);
    public void OnFighterButtonLightKick(InputAction.CallbackContext context);
    public void OnFighterButtonHeavyKick(InputAction.CallbackContext context);
    public void OnFighterButtonFierceKick(InputAction.CallbackContext context);
}

public interface IAIInputButtonFunctions
{
    public void OnFighterButtonLightPunch(AIButtonPressContext context);
    public void OnFighterButtonHeavyPunch(AIButtonPressContext context);
    public void OnFighterButtonFiercePunch(AIButtonPressContext context);
    public void OnFighterButtonLightKick(AIButtonPressContext context);
    public void OnFighterButtonHeavyKick(AIButtonPressContext context);
    public void OnFighterButtonFierceKick(AIButtonPressContext context);
}

public class FighterInput : MonoBehaviour, IEntityButtonInput
{
    public bool canAttack = true;

    public Vector2 movement { get; set; }
    public bool button_lightPunch { get; set; }
    public bool button_heavyPunch { get; set; }
    public bool button_fiercePunch { get; set; }
    public bool button_lightKick { get; set; }
    public bool button_heavyKick { get; set; }
    public bool button_fierceKick { get; set; }

    public bool specialCheckButton_lightPunch;
    public bool specialCheckButton_heavyPunch;
    public bool specialCheckButton_fiercePunch;

    public bool specialCheckButton_lightKick;
    public bool specialCheckButton_heavyKick;
    public bool specialCheckButton_fierceKick;

    public virtual void ChangeAttackState(int canAttackChange)
    {
        if (canAttackChange == 0)
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }
    }

    public bool CheckIfBlocking(float inputX, int currentDirectionX)
    {
        if (inputX != 0)
        {
            if (Mathf.Floor(inputX) != currentDirectionX)
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
}