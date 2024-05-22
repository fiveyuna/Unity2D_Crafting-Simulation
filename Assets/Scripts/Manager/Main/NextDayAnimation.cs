using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDayAnimation : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 3f;
    private static NextDayAnimation instance;

    private void Awake()
    {
        instance = this;
    }

    public static NextDayAnimation GetInstance()
    {
        return instance;
    }

    public bool PlayNextDayAnimation()
    {
        StartCoroutine(StartAnimation());
        return true;
    }

    private IEnumerator StartAnimation()
    {
        // Play animation
        transition.Play("Crossfade_Start 0");

        // Wait
        yield return new WaitForSeconds(transitionTime);
    }
}
