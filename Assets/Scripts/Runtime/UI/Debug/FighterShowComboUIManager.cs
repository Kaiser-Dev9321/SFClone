using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FighterShowComboUIManager : MonoBehaviour
{
    public GameManager gameManager;

    public TextMeshProUGUI fighter1ComboText;
    public TextMeshProUGUI fighter2ComboText;

    private void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>();
    }

    //TOOD: Fix to use events instead of update
    private void Update()
    {
        fighter1ComboText.text = $"{gameManager.fighter1.GetComponent<FighterComboManager>().combo_activeCount} hit\nCombo";
        fighter2ComboText.text = $"{gameManager.fighter2.GetComponent<FighterComboManager>().combo_activeCount} hit\nCombo";
    }
}
