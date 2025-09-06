using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_CharacterCell : MonoBehaviour
{
    //Data
    public GameObject fighterGameObject;
    public ArcadeModeData fighterArcadeModeData;


    //Functions
    public UnityEvent onMoveUp;
    public UnityEvent onMoveRight;
    public UnityEvent onMoveDown;
    public UnityEvent onMoveLeft;

    [Space]
    public UnityEvent onPress;
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private void Awake()
    {
        OnExitFunction();
    }

    public virtual void MoveUpFunction()
    {
        onMoveUp?.Invoke();
    }

    public virtual void MoveRightFunction()
    {
        onMoveRight?.Invoke();
    }

    public virtual void MoveDownFunction()
    {
        onMoveDown?.Invoke();
    }

    public virtual void MoveLeftFunction()
    {
        onMoveLeft?.Invoke();
    }

    public virtual void OnPressFunction()
    {
        onPress?.Invoke();
    }

    public virtual void OnEnterFunction()
    {
        onEnter?.Invoke();
    }

    public virtual void OnExitFunction()
    {
        onExit?.Invoke();
    }
}
