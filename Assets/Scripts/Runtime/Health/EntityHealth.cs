using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    private EntityScript entity;

    public FighterHealthData fighter_HealthData;
    public float currentHealth;

    private UI_HealthChanger uiHealthChanger;

    private Canvas canvas;
    public string healthImage;

    public bool debugUIHealth;

    public void LoadHealthEssentials()
    {
        entity = GetComponent<EntityScript>();

        currentHealth = fighter_HealthData.fighter_maxHealth;
        canvas = GameObject.Find("Canvas_GameInfo").GetComponent<Canvas>();

        uiHealthChanger = canvas.transform.Find(healthImage).GetComponentInChildren<UI_HealthChanger>();

        uiHealthChanger.healthSlider.maxValue = fighter_HealthData.fighter_maxHealth;
        uiHealthChanger.healthSlider.value = currentHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetHealthToMaxHealth()
    {
        currentHealth = fighter_HealthData.fighter_maxHealth;

        uiHealthChanger.SetNewHealth(currentHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            if (debugUIHealth)
            {
                currentHealth = fighter_HealthData.fighter_maxHealth;
            }
            else
            {
                //print($"Entity knock down: {entity.fighterKnockdownManager}");

                entity.fighterKnockdownManager.ActivateKnockout();
            }
        }

        uiHealthChanger.SetNewHealth(currentHealth);
    }
}
