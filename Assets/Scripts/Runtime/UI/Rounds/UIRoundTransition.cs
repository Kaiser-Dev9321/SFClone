using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoundTransition : MonoBehaviour
{
    [HideInInspector]
    public Animator uiRoundTransitionAnimator;

    private void Awake()
    {
        uiRoundTransitionAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        uiRoundTransitionAnimator.Play("Transition");
    }

    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
