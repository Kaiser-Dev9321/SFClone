using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackEffectType
{
    Hit,
    Blow
}

[CreateAssetMenu(fileName = "AttackEffectData_", menuName = "Attack Data/New Attack Effect Data")]
public class AttackEffectData : ScriptableObject, IJuggleProperties
{
    public AttackEffectType attackEffectType;

    public int effect_hitDamage;
    public int effect_chipDamage;
    public int effect_Stun;
    public HitstunData effect_Hitstun;

    public int damageScalingPoints = 1;

    [Header("Air properties")]
    public bool canAirJuggle;
    public int juggle_Start;
    public int juggle_Increase;
    public int juggle_Potential;
    public bool automaticallyRecoverFromAttack = true;

    [Header("Meter properties")]
    public int effect_meterGainOnWhiff;
    public int effect_meterGainOnBlock;
    public int effect_meterGainOnHit;

    public int juggle_startProperty { get { return juggle_Start; } set { } }
    public int juggle_increaseProperty { get { return juggle_Increase; } set { } }
    public int juggle_potentialProperty { get { return juggle_Potential; } set { } }
}
