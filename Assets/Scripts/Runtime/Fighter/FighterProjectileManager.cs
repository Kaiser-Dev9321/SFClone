using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterProjectileManager : MonoBehaviour
{
    public Transform[] spawnTransforms;
    private GameObject projectile;

    private Transform activeSpawnTransform;

    public void SetProjectileSpawnPosition(int projectileSpawnerIndex)
    {
        activeSpawnTransform = spawnTransforms[projectileSpawnerIndex];
    }

    public void SetProjectilePrefab(GameObject projectileGO)
    {
        projectile = projectileGO;
    }

    public void SpawnProjectile()
    {
        GameObject projectilePrefab = Instantiate(projectile, activeSpawnTransform.position, Quaternion.identity);
        EntityScript attacker_Entity = transform.GetComponent<EntityScript>();

        ProjectileManager prefabProjectileManager = projectilePrefab.GetComponent<ProjectileManager>();
        prefabProjectileManager.attacker_Entity = attacker_Entity;
        prefabProjectileManager.directionX = (int) prefabProjectileManager.attacker_Entity.GetXDirection();

        prefabProjectileManager.ShootProjectile();
    }
}
