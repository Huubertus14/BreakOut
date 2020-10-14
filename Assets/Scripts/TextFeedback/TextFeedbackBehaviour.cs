using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFeedbackBehaviour : MonoBehaviour
{
    private TextMeshPro feedbackText;
    private TweenAbstract scaleTween;
    private void Awake()
    {
        feedbackText = GetComponent<TextMeshPro>();
        scaleTween = GetComponent<TweenAbstract>();
        feedbackText.text = "";
        feedbackText.color = Color.white;
    }

    public void SetFeedback(string text, Color textColor, Vector3 startPosition, Vector3 endPos)
    {
        feedbackText.text = text;
        feedbackText.color = textColor;
        transform.position = startPosition + new Vector3(0, 0, -0.9f);
        endPos.z = transform.position.z;

        StartCoroutine(TweenAnimation(endPos));
        scaleTween.StartTween(0.4f);
    }

    private IEnumerator TweenAnimation(Vector3 endPoint)
    {
        float speed = 1;
        while (transform.position != endPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, Time.deltaTime * speed);
            speed *= 1.05f;
            yield return 0;
        }

        yield return 0;
    }
}
