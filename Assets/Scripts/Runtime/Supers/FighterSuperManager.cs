using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSuperManager : MonoBehaviour
{
    public UI_SuperMeter ui_SuperMeter;

    public int[] superMeter = new int[2];
    public bool[] superMeterStock = new bool[2];

    public bool fullSuperBars = false;
    public bool inSuperFreeze;

    [SerializeField]
    private int currentMeterStock;

    public void LoadSuperMeterEssentials(UI_SuperMeter newUISuperMeter)
    {
        ui_SuperMeter = newUISuperMeter;
    }

    private void CheckStock()
    {
        if (superMeterStock[0] == false)
        {
            //print($"<b>Stock 1</b>");

            currentMeterStock = 0;
        }
        else if (superMeterStock[1] == false)
        {
            //print($"<b>Stock 2</b>");

            currentMeterStock = 1;
        }
    }

    public void GainMeter(int meterToGain)
    {
        if (!fullSuperBars)
        {
            CheckStock();

            //print($"Meter gain: {meterToGain}");
            superMeter[currentMeterStock] += meterToGain;
            superMeter[currentMeterStock] = Mathf.Clamp(superMeter[currentMeterStock], 0, 48);

            //print($"Super length test: {superMeter[currentMeterStock]} {superMeter.Length}");

            if (superMeter[currentMeterStock] >= 48)
            {
                superMeterStock[currentMeterStock] = true;
            }

            if (superMeterStock[superMeter.Length - 1] == true)
            {
                //print("Completely full super meters");
                fullSuperBars = true;
            }

            ChangeSuperMeter(superMeter[currentMeterStock]);
        }
    }

    public void DepleteSuperStock(int stockToDeplete)
    {
        currentMeterStock -= stockToDeplete;
        currentMeterStock = Mathf.Clamp(currentMeterStock, 0, superMeterStock.Length - 1);
    }

    public void UseSuperMeter()
    {
        if (fullSuperBars)
        {
            fullSuperBars = false;

            DepleteSuperStock(0);
        }
        else
        {
            DepleteSuperStock(1);
        }

        superMeter[currentMeterStock] = 0;
        superMeterStock[currentMeterStock] = false;

        CheckStock();
    }

    public void ChangeSuperMeter(int newValue)
    {
        ui_SuperMeter.superMeterSlider.value = newValue;

        if (fullSuperBars)
        {
            ui_SuperMeter.superMeterAvailableText.text = superMeter.Length.ToString();
        }
        else
        {
            ui_SuperMeter.superMeterAvailableText.text = currentMeterStock.ToString();
        }
    }

    public void DisableSuperState()
    {

    }
}
