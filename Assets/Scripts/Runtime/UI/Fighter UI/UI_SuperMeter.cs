using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SuperMeter : MonoBehaviour
{
    public Slider superMeterSlider;
    public TextMeshProUGUI superMeterAvailableText;

    public void IncreaseSlider(int meterToGain)
    {
        superMeterSlider.value += meterToGain;
        superMeterSlider.value = Mathf.Clamp(meterToGain, 0, 48);
    }
}
