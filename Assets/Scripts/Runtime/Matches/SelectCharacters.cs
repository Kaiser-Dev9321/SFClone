using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacters : MonoBehaviour
{
    public GameObject storedFighter;

    public GameObject activeFighter1;
    public GameObject activeFighter2;

    //Player select buttons assign storedFighter to activeFighter
    public virtual void AssignActiveFighter(int fighterIndex)
    {
        if (fighterIndex == 0)
        {
            activeFighter1 = storedFighter;
        }

        if (fighterIndex == 1)
        {
            activeFighter2 = storedFighter;
        }
    }

    //New stored fighter to be stored until it is selected
    public void NewStoredFighter(GameObject fighter)
    {
        storedFighter = fighter;
    }
}
