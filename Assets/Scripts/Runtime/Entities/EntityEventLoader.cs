using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEventLoader : MonoBehaviour
{
    private GameManager fighterGameManager;

    public delegate void EntityLoaded();
    public event EntityLoaded entityLoadedEvent;

    public delegate void EntityEssentialsLoaded();
    public event EntityEssentialsLoaded entityEssentialsLoadedEvent;

    private void Awake()
    {
        fighterGameManager = UnityEngine.Object.FindObjectOfType<GameManager>();
    }

    public void OnEntityLoaded()
    {
        entityLoadedEvent.Invoke();
    }

    public void OnEntityEssentialsLoaded()
    {
        entityEssentialsLoadedEvent.Invoke();
    }
}
