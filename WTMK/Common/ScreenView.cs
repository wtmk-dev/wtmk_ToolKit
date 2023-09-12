using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenView : MonoBehaviour
{
    [HideInInspector]
    public bool Transition;

    [HideInInspector]
    public int Track;

    public virtual void TransitionIn()
    {

    }

    public virtual void TransitionOut()
    {

    }
}
