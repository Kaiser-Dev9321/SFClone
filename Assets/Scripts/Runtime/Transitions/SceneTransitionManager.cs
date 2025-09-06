using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public string transitionPlaying;
    public bool transitionComplete = true;

    private Animator animator;

    private void Start()
    {
        if (!animator)
        {
            DontDestroyOnLoad(this);
            animator = GetComponent<Animator>();
        }
    }

    public void PlaySceneTransitions(string transitionName, float speed)
    {
        transitionPlaying = transitionName;

        animator.SetFloat("TransitionSpeed", speed);

        switch (transitionName)
        {
            case "CurtainsIn":
                animator.Play("CurtainsIn");
                break;
            case "CurtainsOut":
                animator.Play("CurtainsOut");
                break;
        }

        transitionComplete = false;
    }

    public void SetTransitionCompleteBool(int completeBool)
    {
        if (completeBool == 0)
        {
            transitionComplete = false;
        }
        else
        {
            transitionComplete = true;
        }
    }
}
