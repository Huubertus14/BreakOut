using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ScaleTween : MonoBehaviour
{
    [Header("AnimationValues")]
    [SerializeField] private AnimationCurve curve;

    private float timeTweenKey = 2;
    private float tweenValue = 2;

    private float duration;

    private Vector3 orginScale;

    private void Awake()
    {
        orginScale = transform.localScale;
    }

    public void StartTween(float _duration)
    {
        StopAllCoroutines();
        duration = _duration;
        timeTweenKey = 0;
        StartCoroutine(DoTween());
    }

    private IEnumerator DoTween()
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
        transform.localScale = new Vector3(transform.localScale.x * _scale, transform.localScale.y * _scale, transform.localScale.z * _scale);
    }

}
