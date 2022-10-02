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
}
