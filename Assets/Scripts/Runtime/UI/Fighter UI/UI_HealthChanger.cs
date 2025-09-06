using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthChanger : MonoBehaviour
{
    [HideInInspector]
    public Slider healthSlider;

    private void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }

    public void SetNewHealth(float newHealth)
    {
        healthSlider.value = newHealth;
    }
}
