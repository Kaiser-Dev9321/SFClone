using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileManager : MonoBehaviour
{
    public Animator animator;
    public string animationToPlay;

    private GameObject hitbox;
    private ProjectileHitboxOnTrigger projectileHitbox;

    private int currentHitsLeft;

    [Space]
    public EntityScript attacker_Entity;

    public ProjectileData projectileData;
    private float projectileMaxLifetimeTime;

    private bool shouldDestroy = false;
    private bool shouldReactivateTimer = true;

    private bool projectileHit = false;

    private bool hasPausedBeforeProjectileSpawn = false;
    private bool shouldSpawnProjectileThisFrame = false;
    private bool hasSpawnedProjectile = false;

    public Rigidbody2D rb;

    public int directionX;

    [HideInInspector]
    public bool canHitself = false;

    private Timer multipleHits_reactivateTimer;

    public delegate void LoadProjectileReferences();
    public event LoadProjectileReferences LoadProjectileReferencesEvent;

    public UnityEvent eventOnProjectileDestroyed;

    public void SpawnObjectAtPosition(GameObject objectToSpawn)
    {
        GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }

    public void SpawnProjectileAtPosition(GameObject projectile)
    {
        GameObject projectilePrefab = Instantiate(projectile, transform.position, Quaternion.identity);

        ProjectileManager prefabProjectileManager = projectilePrefab.GetComponent<ProjectileManager>();
        prefabProjectileManager.attacker_Entity = attacker_Entity;
        prefabProjectileManager.directionX = (int)prefabProjectileManager.attacker_Entity.GetXDirection();

        prefabProjectileManager.ShootProjectile();
    }

    private void Awake()
    {
        multipleHits_reactivateTimer = new Timer(projectileData.multipleHits_reactivateTimer, projectileData.multipleHits_reactivateTimer);
        multipleHits_reactivateTimer.SetToTimer();

        currentHitsLeft = projectileData.maximumHits;

        hitbox = gameObject.GetComponentInChildren<BoxCollider2D>().gameObject;
        projectileHitbox = hitbox.GetComponent<ProjectileHitboxOnTrigger>();
    }

    public virtual void ShootProjectile()
    {
        if (animator)
        {
            animator.Play(animationToPlay);
        }

        if (LoadProjectileReferencesEvent != null)
        {
            print($"<size=16>Shoot proj invoke</size>");

            LoadProjectileReferencesEvent.Invoke();
        }
        else
        {
            Debug.LogWarning($"<size=16>Shoot proj fail</size>");
        }
    }

    public void SubtractHitsLeft(int subtractBy)
    {
        currentHitsLeft -= subtractBy;
    }

    protected virtual void UpdateProjectile()
    {
        if (!attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped)
        {
            rb.velocity = Vector2.right * directionX * projectileData.projectileSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (!projectileData.limitByHits)
        {
            ProjectileFunctions.ProjectileUpdate(gameObject, hitbox, attacker_Entity, projectileData, ref projectileMaxLifetimeTime, 99, ref animator, ref shouldReactivateTimer, ref shouldDestroy);
        }
        else
        {
            ProjectileFunctions.ProjectileUpdate(gameObject, hitbox, attacker_Entity, projectileData, ref projectileMaxLifetimeTime, currentHitsLeft, ref animator, ref shouldReactivateTimer, ref shouldDestroy);
        }

        if (shouldDestroy)
        {
            if (!attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped)
            {
                //Spawn if prerequisites were met
                if (eventOnProjectileDestroyed.GetPersistentEventCount() > 0)
                {
                    print("Invoke proj destroy 2");

                    if (hasPausedBeforeProjectileSpawn && shouldSpawnProjectileThisFrame)
                    {
                        print("Invoke proj destroy 3");

                        hasSpawnedProjectile = true;

                        eventOnProjectileDestroyed.Invoke();

                        Destroy(gameObject);
                    }
                }
                else
                {
                    print("Invoke proj destroy 1");
                    Destroy(gameObject);
                }
            }
        }
    }

    private void DisableProjectile()
    {
        hitbox.SetActive(false);
    }

    private void UpdateMultiHitProjectile()
    {
        if (projectileHitbox.thisHitboxHit)
        {
            projectileHit = true;
            DisableProjectile();
        }

        if (!hitbox.activeSelf && !shouldReactivateTimer)
        {
            if (!attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped)
            {
                if (multipleHits_reactivateTimer.currentTime > 0)
                {
                    multipleHits_reactivateTimer.DecreaseByTick();
                }
                else
                {
                    projectileHit = false;
                    shouldReactivateTimer = true;

                    hitbox.gameObject.SetActive(true);

                    multipleHits_reactivateTimer.SetToTimer();
                }
            }
        }
    }

    private void Update()
    {
        UpdateProjectile();

        if (projectileData.maximumHits > 1)
        {
            UpdateMultiHitProjectile();
        }

        if (currentHitsLeft >= projectileData.maximumHits - 1)
        {
            if (attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped && !hasPausedBeforeProjectileSpawn && !shouldSpawnProjectileThisFrame)
            {
                hasPausedBeforeProjectileSpawn = true;
            }

            if (!attacker_Entity.fighterGameplayManager.freezeStopManager.freezeStopped && hasPausedBeforeProjectileSpawn && !shouldSpawnProjectileThisFrame)
            {
                shouldSpawnProjectileThisFrame = true;
            }
        }
    }
}
