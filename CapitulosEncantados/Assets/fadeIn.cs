using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeIn : MonoBehaviour
{
    public Animator fadeAnimator;

    void Start()
    {
        if (fadeAnimator != null)
        {
            fadeAnimator.SetTrigger("Start");
        }
    }
}
