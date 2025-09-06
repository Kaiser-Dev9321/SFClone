using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTransition : MonoBehaviour
{
    public SceneTransitionManager sceneTransitionManager;

    private void Start()
    {
        sceneTransitionManager = GameObject.Find("TransitionCanvas").GetComponent<SceneTransitionManager>();

        sceneTransitionManager.PlaySceneTransitions("CurtainsOut", 2);
    }
}
