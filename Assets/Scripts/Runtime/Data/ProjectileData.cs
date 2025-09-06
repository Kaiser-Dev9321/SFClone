using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Projectile Data", menuName = "Fighter Data/New Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public float projectileSpeed;

    public bool hasSpecialEffect;
    //Special effect stuff, but not implemented yet

    [Space]
    public bool limitByHits = false;
    public bool destroyProjectileAfterHitsDepleted = true; //Cannot be true if the bottom
    public float multipleHits_reactivateTimer;
    public int maximumHits;

    [Space]
    public float projectileMaxLifetimeTimer;

    //public UnityEvent specialEffectEvent;
}
