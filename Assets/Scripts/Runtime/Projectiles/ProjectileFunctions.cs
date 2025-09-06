using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ProjectileFunctions : MonoBehaviour
{
    public static void ProjectileUpdate(GameObject projectile, GameObject hitbox, EntityScript attacker_Entity, ProjectileData projectileData, ref float projectileMaxTime, int currentHitsLeft, ref Animator animator, ref bool shouldReactivate, ref bool shouldDestroy)
    {
        if (currentHitsLeft <= 0)
        {
            shouldDestroy = true;
        }

        if (projectileMaxTime <= projectileData.projectileMaxLifetimeTimer)
        {
            //TOOD: Repeating basic maximumHits statement, needs refactoring lol

            if (!attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped)
            {
                projectileMaxTime += Time.deltaTime;
                if (animator)
                {
                    animator.speed = 1;
                }
            }
            else
            {
                if (projectileData.maximumHits > 1)
                {
                    shouldReactivate = false;
                }

                if (animator)
                {
                    animator.speed = 0;
                }
            }
        }
        else if (projectileMaxTime > projectileData.projectileMaxLifetimeTimer)
        {
            print($"<color=#94320f>Projectile out of time</color>");
            shouldDestroy = true;
        }
    }
}
