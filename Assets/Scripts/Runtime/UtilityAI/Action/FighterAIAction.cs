using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAIAction : ScriptableObject
{
    public string Name;
    private float _score;
    public float score
    {
        get
        {
            return _score;
        }
        set
        {
            this._score = Mathf.Clamp01(value);
        }
    }

    public Consideration[] considerations;

    public virtual void Awake()
    {
        score = 0;
    }

    public abstract void Enter(FighterAIController ai);
    public abstract void Execute(FighterAIController ai);
    public abstract void Exit(FighterAIController ai);
}
