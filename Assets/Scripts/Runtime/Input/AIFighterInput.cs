using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighterInput : FighterInput, IEntityInputMovementFunctions, IAIInputButtonFunctions
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
    public void OnFighterInputMove(Vector2 input)
    {
        movement = input;
        cMove = movement;
        //print(movement);
    }

    //Player button functions
    public void OnFighterButtonLightPunch(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_lightPunch = true;
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

    public void OnFighterButtonHeavyPunch(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_heavyPunch = true;
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

    public void OnFighterButtonFiercePunch(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_fiercePunch = true;
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

    public void OnFighterButtonLightKick(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_lightKick = true;
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

    public void OnFighterButtonHeavyKick(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_heavyKick = true;
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

    public void OnFighterButtonFierceKick(AIButtonPressContext context)
    {
        if (context == AIButtonPressContext.Started)
        {
            if (entity.fighterAttackManager.canPerformNormals)
            {
                button_fierceKick = true;
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