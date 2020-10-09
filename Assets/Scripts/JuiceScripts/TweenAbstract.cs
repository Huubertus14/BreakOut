using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TweenAbstract : MonoBehaviour
{
    protected float duration;


    public virtual void StartTween(float _duration)
    {
        StopAllCoroutines();
        duration = _duration;
    }

    protected abstract IEnumerator DoTween();
}
