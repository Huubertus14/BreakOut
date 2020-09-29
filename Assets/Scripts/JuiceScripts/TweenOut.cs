using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenOut : MonoBehaviour
{
    [Header("AnimationValues")]
    [SerializeField] private AnimationCurve curve;

    public float timeTweenKey = 2;
    public float tweenValue = 2;

    public float duration;

    public Vector3 orginScale;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        orginScale = rectTransform.localScale;
    }

    public void StartTween(float _duration)
    {
        StopAllCoroutines();
        duration = _duration;
        tweenValue = 0;
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

        rectTransform.localScale = orginScale;
        gameObject.SetActive(false);
        yield return 0;
    }

    private void SetLocalScale(float _scale)
    {
        rectTransform.localScale = new Vector3(_scale, _scale, _scale);
    }
}
