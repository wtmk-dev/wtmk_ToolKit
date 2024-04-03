using System;
using System.Collections;
using UnityEngine;

public sealed class CorutineRunner : SingeltonMonoBehavior<CorutineRunner>
{
    public IEnumerator Coroutine;

    public void Run(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    public void Stop(IEnumerator routine)
    {
        StopCoroutine(routine);
    }

    public IEnumerator DelayBeforeCallBack(float waitTime, Action callBack)
    {
        yield return new WaitForSeconds(waitTime);
        callBack();
    }

    public IEnumerator WaitFrameBeforeCallBack(Action callBack)
    {
        yield return new WaitForEndOfFrame();
        callBack();
    }
}
