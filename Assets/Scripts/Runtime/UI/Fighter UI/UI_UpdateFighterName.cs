using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_UpdateFighterName : MonoBehaviour
{
    public SelectCharacters selectCharacters;
    private TextMeshProUGUI text;

    public bool isFighter1;
    public bool isFighter2;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void LateUpdate()
    {
        if (isFighter1)
        {
            if (selectCharacters.activeFighter1)
            {
                text.text = selectCharacters.activeFighter1.GetComponent<EntityScript>().fighterName;
            }
        }
        else if (isFighter2)
        {
            if (selectCharacters.activeFighter2)
            {
                text.text = selectCharacters.activeFighter2.GetComponent<EntityScript>().fighterName;
            }
        }
    }
}
