#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOut : TweenAbstract
{
    [Header("AnimationValues")]
    [SerializeField] private AnimationCurve curve;

    public float timeTweenKey = 2;
    public float tweenValue = 2;

    public Vector3 orginScale;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        orginScale = rectTransform.localScale;
    }

    public override void StartTween(float _duration)
    {
        base.StartTween(_duration);
        tweenValue = 0;
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

        rectTransform.localScale = orginScale;
        gameObject.SetActive(false);
        yield return 0;
    }

    private void SetLocalScale(float _scale)
    {
        rectTransform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
