using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Separate from chain cancels, these handle special and super cancellable moves, although I might change my mind later
public class CancellableAttackFighterState : AttackFighterState
{
    public override void CheckAttacks()
    {
        if (entity.fighterInput.canAttack && !entity.fighterMotionInputManager.storedCommand)
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
    }
}
