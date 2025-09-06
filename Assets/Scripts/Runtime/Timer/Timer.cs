using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Timer
{
    public float currentTime;
    public float currentTimer;

    public Timer(float setNewCurrentTime, float setNewCurrentTimer)
    {
        this.currentTime = setNewCurrentTime;
        this.currentTimer = setNewCurrentTimer;
    }

    public void SetToZero()
    {
        currentTime = 0;
    }
    
    public void SetToTimer()
    {
        currentTime = currentTimer;
    }

    public void DecreaseByTick()
    {
        currentTime -= Time.deltaTime;
    }

    public void IncreaseByTick()
    {
        currentTime += Time.deltaTime;
    }


    public void DecreaseByTickUnscaled()
    {
        currentTime -= Time.unscaledDeltaTime;
    }

    public void IncreaseByTickUnscaled()
    {
        currentTime += Time.unscaledDeltaTime;
    }
}
