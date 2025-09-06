using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFighterInput : FighterInput, IPlayerInputMovementFunctions, IPlayerInputButtonFunctions
{
    public EntityScript entity;
    private StateMachine_Entity stateMachine_Entity;

    public Vector2 cMove;

    private void Start()
    {
        entity = GetComponent<EntityScript>();
        stateMachine_Entity = entity.stateMachine as StateMachine_Entity;
    }

    //Player movement functions
    public void OnFighterInputMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
        cMove = movement;
        //print(movement);
    }

    //Player button functions
    public void OnFighterButtonLightPunch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_lightPunch = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_lightPunch = true;
                }
            }
        }
    }

    public void OnFighterButtonHeavyPunch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_heavyPunch = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_heavyPunch = true;
                }
            }
        }
    }

    public void OnFighterButtonFiercePunch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_fiercePunch = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_fiercePunch = true;
                }
            }
        }
    }

    public void OnFighterButtonLightKick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_lightKick = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_lightKick = true;
                }
            }
        }
    }

    public void OnFighterButtonHeavyKick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_heavyKick = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_heavyKick = true;
                }
            }
        }
    }

    public void OnFighterButtonFierceKick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_fierceKick = context.started;
            }

            if (stateMachine_Entity.currentAttackStateData)
            {
                if (stateMachine_Entity.currentAttackStateData.specialCancellable || stateMachine_Entity.currentAttackStateData.superCancellable)
                {
                    specialCheckButton_fierceKick = true;
                }
            }
        }
    }
}