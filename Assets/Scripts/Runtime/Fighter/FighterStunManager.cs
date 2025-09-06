using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStunManager : MonoBehaviour
{
    private EntityHitstop entityHitstop;

    public HitstopData standing_Light_HitstopData;
    public HitstopData standing_Heavy_HitstopData;
    public HitstopData standing_Fierce_HitstopData;

    public HitstopData crouching_Light_HitstopData;
    public HitstopData crouching_Heavy_HitstopData;
    public HitstopData crouching_Fierce_HitstopData;

    private void Start()
    {
        entityHitstop = GetComponent<EntityHitstop>();
    }

    public void ActivateHitstop(HitstopData hitstopData, bool isTheAttacker)
    {
        //print("Activate hitstop");
        entityHitstop.hitstopData = hitstopData;

        if (!isTheAttacker)
        {
            entityHitstop.DoDefenderHitstop();
        }
        else
        {
            entityHitstop.DoAttackerHitstop();
        }
    }
}
