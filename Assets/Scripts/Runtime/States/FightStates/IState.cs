using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IState : MonoBehaviour
{
    public virtual void State_Enter()
    {
    }

    public virtual void State_Update()
    {
    }

    public virtual void State_PhysicsUpdate()
    {
    }

    public virtual void State_Exit()
    {
    }
}