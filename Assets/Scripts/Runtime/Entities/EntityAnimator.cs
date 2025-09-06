using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityAnimator
{
    public Animator animator { get; set; }
    public StateMachine_Entity stateMachine { get; set; }
    public void PlayAnimation(string newAnimationState);
}

public class EntityAnimator : MonoBehaviour, IEntityAnimator
{
    public Animator animator { get; set; }

    public StateMachine_Entity stateMachine { get; set; }

    [HideInInspector]
    public EntityScript entity;

    private void Awake()
    {
        entity = GetComponent<EntityScript>();
        animator = GetComponent<Animator>();
        stateMachine = GetComponentInParent<StateMachine_Entity>();
    }

    public void PlayAnimation(string newAnimationState)
    {
        animator.Play(newAnimationState);
    }

    public void PlayAnimation(string newAnimationState, int layer, float normalisedTime)
    {
        animator.Play(newAnimationState, layer, normalisedTime);
    }

    public void ReturnToNeutral()
    {
        //TOOD: Will probably need to change this in the future
        entity.fighterComboManager.attacker_Entity = null;

        if (entity.entityMovement.groundChecker.grounded)
        {
            if (entity.fighterInput.movement.x == 0)
            {
                //print("Return to idle state");

                //entity.fighterInput.ChangeAttackState(1);
                this.stateMachine.ChangeState(stateMachine.state_idle);
            }
            else
            {
                //print("Return to walk state");

                //entity.fighterInput.ChangeAttackState(1);
                this.stateMachine.ChangeState(stateMachine.state_walk);
            }
        }
        else
        {
            this.stateMachine.ChangeState(stateMachine.state_air);
        }

        entity.fighterComboManager.lastTC_Combo_Check = 0;

        entity.fighterMotionInputManager.ResetInputsAndDirections();

        entity.fighterAttackManager.canPerformNormals = true;
        entity.fighterAttackManager.canPerformSpecials = true;
        entity.fighterAttackManager.canPerformSuper = true;

        entity.fighterAttackManager.inChainCancelWindow = false;
        entity.fighterAttackManager.currentlyChainCancelling = false;

        entity.entityMovement.OverrideDisableWallPushback(0);
        entity.entityMovement.OverrideDisablePushFighters(0);

        entity.entityMovement.ChangeWallPushbackEffect(1);
        entity.entityMovement.ChangePushbackFightersEffect(1);

        //TODO: Also gotta do something about disablewallpushback and disablepushfighters
    }
}
