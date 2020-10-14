using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenIn : TweenAbstract
{
    [Header("AnimationValues")]
    [SerializeField] private AnimationCurve curve;

    private float timeTweenKey = 2;
    private float tweenValue = 2;


    private Vector3 orginScale;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        orginScale = rectTransform.localScale;
        SetLocalScale(0);
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
        yield return 0;
    }

    private void SetLocalScale(float _scale)
    {
        rectTransform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
