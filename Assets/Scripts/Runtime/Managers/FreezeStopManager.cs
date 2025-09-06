using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class FreezeStopManager : MonoBehaviour
{
    [HideInInspector]
    public bool freezeStopped = false;


    public float freezeStopTime;
    //This entity's freezestop data
    public FreezeStopData freezeStopData;

    public GameObject hitstunnedParticles;
    public UnityEvent onFreezeStopped;

    public EntityScript fighter1;
    public EntityScript fighter2;

    private void Update()
    {
        if (freezeStopTime > 0)
        {
            freezeStopTime -= Time.deltaTime;

            if (!fighter1.entityMovement.disableMovement)
            {
                fighter1.entityMovement.DisableMovement(1);
            }

            if (!fighter2.entityMovement.disableMovement)
            {
                fighter2.entityMovement.DisableMovement(1);
            }
        }
        else if (freezeStopTime < 0)
        {
            freezeStopped = false;
            freezeStopTime = 0;

            RestoreFreezeStop(fighter1, fighter2);
        }
    }

    private void RestoreFreezeStop(EntityScript attackerEntity, EntityScript defenderEntity)
    {
        attackerEntity.entityMovement.DisableMovement(0);

        if (!attackerEntity.entityMovement.overrideDisableWallPushback)
        {
            attackerEntity.entityMovement.DisableWallPushback(0);
        }

        attackerEntity.entityAnimator.animator.speed = 1;

        defenderEntity.entityMovement.DisableMovement(0);

        if (!defenderEntity.entityMovement.overrideDisableWallPushback)
        {
            defenderEntity.entityMovement.DisableWallPushback(0);
        }

        defenderEntity.entityAnimator.animator.speed = 1;

        freezeStopData = null;

        //Assumes entity was in freezestop
        if (attackerEntity.entityHitstun.duringFreezeStop)
        {
            attackerEntity.entityHitstun.duringFreezeStop = false;
            attackerEntity.entityHitstun.afterFreezeStop = true;
        }

        if (defenderEntity.entityHitstun.duringFreezeStop)
        {
            defenderEntity.entityHitstun.duringFreezeStop = false;
            defenderEntity.entityHitstun.afterFreezeStop = true;
        }
    }

    public void SpawnFreezeStopParticles(GameObject spawnObject, GameObject particlesToSpawn)
    {
        //Instantiate(particlesToSpawn, spawnObject.transform.position, Quaternion.identity);
    }

    public void AssignFreezeStopTime(FreezeStopData newFreezeStopData)
    {
        freezeStopData = newFreezeStopData;
        freezeStopTime = freezeStopData.freezeStop_Milliseconds;
        freezeStopped = true;
    }

    public void QuickSuperFreezeStop(EntityScript entity)
    {
        if (entity.transform.tag == "Fighter1")
        {
            DoFreezeStop(fighter1, true);
            DoFreezeStop(fighter2, false);
        }

        if (entity.transform.tag == "Fighter2")
        {
            DoFreezeStop(fighter1, false);
            DoFreezeStop(fighter2, true);
        }

        entity.entityHitstun.duringFreezeStop = true;
    }

    public void DoFreezeStop(EntityScript entity, bool isTheAttacker)
    {
        entity.entityMovement.DisableMovement(1);
        entity.entityMovement.DisableWallPushback(1);

        if (freezeStopData.freezeAttacker)
        {
            if (!isTheAttacker)
            {
                entity.entityAnimator.animator.speed = 0;
            }
        }
        else
        {
            entity.entityAnimator.animator.speed = 0;
        }
    }
}
