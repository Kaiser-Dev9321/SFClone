using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputData_", menuName = "Input Data/New Super Motion Input Button Data")]
public class FighterSuperMotionInputButtonData : FighterBaseMotionInputButtonData
{
    public override bool RequireCondition()
    {
        if (entity.fighterSuperManager.superMeterStock[0] == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
