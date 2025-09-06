using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Lee_LightAuraBarrage : State_MotionCommand
{
    public AttackEffectData auraBarrageAttackEffectData;

    public override void State_Enter()
    {
        entity.fighterInput.button_lightKick = false;

        entity.entityMovement.movement.velocity.x = 0;
        entity.fighterSuperManager.GainMeter(auraBarrageAttackEffectData.effect_meterGainOnWhiff);

        base.State_Enter();
    }

    public override void State_Update()
    {
        base.State_Update();
    }

    public override void State_Exit()
    {
        base.State_Exit();
    }
}
