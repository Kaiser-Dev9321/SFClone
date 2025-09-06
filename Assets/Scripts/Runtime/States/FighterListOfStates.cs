using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterListOfStates : MonoBehaviour
{

    //TODO: Change for all fighters to allow target combos
    public Transform groundAttacksStatesTransform;
    public Transform airAttacksStatesTransform;

    //TODO: Add list of states to FighterState check combos
    public List<FighterState> listOfFighterStates;

    public List<AttackFighterState> listOfAttackFighterStates;

    private void Start()
    {
        FighterState[] fighterGroundAttacks_StateArray = groundAttacksStatesTransform.GetComponents<FighterState>();
        AttackFighterState[] fighterGroundAttacks_AttackStateArray = groundAttacksStatesTransform.GetComponents<AttackFighterState>();

        FighterState[] fighterAirAttacks_StateArray = airAttacksStatesTransform.GetComponents<FighterState>();
        AttackFighterState[] fighterAirAttacks_AttackStateArray = airAttacksStatesTransform.GetComponents<AttackFighterState>();

        foreach (FighterState fighterState in fighterGroundAttacks_StateArray)
        {
            listOfFighterStates.Add(fighterState);
        }

        foreach (AttackFighterState attackFighterState in fighterGroundAttacks_AttackStateArray)
        {
            listOfAttackFighterStates.Add(attackFighterState);
        }

        foreach (FighterState fighterState in fighterAirAttacks_StateArray)
        {
            listOfFighterStates.Add(fighterState);
        }

        foreach (AttackFighterState attackFighterState in fighterAirAttacks_AttackStateArray)
        {
            listOfAttackFighterStates.Add(attackFighterState);
        }
    }
}
