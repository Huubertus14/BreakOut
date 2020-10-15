#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTween : TweenAbstract
{
    [Header("AnimationValues")]
    [SerializeField] private AnimationCurve curve;

    private float timeTweenKey = 2;
    private float tweenValue = 2;

    private float scale;

    private Vector3 orginScale;

    private void Awake()
    {
        orginScale = transform.localScale;
        scale = 1;
    }

    public override void StartTween(float _duration)
    {
        base.StartTween(_duration);
        timeTweenKey = 0;
        StartCoroutine(DoTween());
    }

    protected override IEnumerator DoTween()
    {
        while (timeTweenKey < 1)
        {
            timeTweenKey += Time.deltaTime / duration;
            tweenValue = curve.Evaluate(timeTweenKey);
            SetLocalScale(tweenValue);
            yield return 0;
        }

        transform.localScale = orginScale;
        yield return 0;
    }

    private void SetLocalScale(float _scale)
    {
        scale = _scale;
        transform.localScale = new Vector3(transform.localScale.x * scale, transform.localScale.y * scale, transform.localScale.z * scale);
    }

}
