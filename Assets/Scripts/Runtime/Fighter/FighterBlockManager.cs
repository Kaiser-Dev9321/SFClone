using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockStates
{
    NotBlocking,
    HoldingBlock,
    StandAirBlocking,
    StandBodyBlocking,
    CrouchBlocking
}

public class FighterBlockManager : MonoBehaviour
{
    protected State_Blockstun state_Blockstun;

    [HideInInspector]
    public EntityScript entity;

    public bool proximityBlocking = false;
    public bool holdingBackBlock = false;
    public bool hitDuringStun = false;

    public bool inBlockStun = false;

    [HideInInspector]
    public BlockStates blockState = BlockStates.NotBlocking;

    private void Start()
    {
        entity = GetComponent<EntityScript>();

        state_Blockstun = GetComponentInChildren<State_Blockstun>();
    }

    public void ResetBlocking()
    {
        proximityBlocking = false;
        holdingBackBlock = false;
        hitDuringStun = false;
        inBlockStun = false;
    }

    public void ChangeBlockStunState(int enable)
    {
        if (enable == 0)
        {
            inBlockStun = false;
        }

        if (enable == 1)
        {
            inBlockStun = true;
        }
    }

    public bool CheckBlock()
    {
        if (entity.GetXDirection() == 1 && entity.fighterInput.movement.x == -1)
        {
            return true;
        }

        if (entity.GetXDirection() == -1 && entity.fighterInput.movement.x == 1)
        {
            return true;
        }

        return false;
    }

    public void AssignBlockstun(BlockstunData blockstunData)
    {
        if (!blockstunData)
        {
            Debug.LogError("No blockstun data, returned null");
            return;
        }

        state_Blockstun.blockAnimationName = blockstunData.animationName;
        
        state_Blockstun.blockStunCurveX = blockstunData.blockstunAnimationCurveX;
        state_Blockstun.blockStunCurveY = blockstunData.blockstunAnimationCurveY;
    }

    public void PerformBlock()
    {
        blockState = BlockStates.HoldingBlock;

        entity.stateMachine.ChangeState(state_Blockstun);
    }

    public void ChangeProximityBlockState(BlockStates newBlockState)
    {
        blockState = newBlockState;
    }
}
