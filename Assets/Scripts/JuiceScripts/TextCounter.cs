using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCounter : MonoBehaviour
{
    [Header("refs:")]
    [SerializeField] private TextMeshProUGUI countText;

    [Header("Counter values")]
    [SerializeField] private float counterSpeed = 10;
    [SerializeField] private string baseText = "";

    private float goalValue;
    private float currentValue;

    private void Awake()
    {
        currentValue = 0;
        goalValue = 0;
        SetText(baseText, (int)currentValue);
    }

    public void SetCounter(float _newGoal, float _speed = 10)
    {
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            counterSpeed = _speed;
            goalValue = _newGoal;
            StartCoroutine(Counter());
        }
    }

    private IEnumerator Counter()
    {
        while (currentValue < goalValue)
        {
            currentValue += counterSpeed;
            SetText(baseText, (int)currentValue);
            yield return 0;
        }
        currentValue = goalValue;
        yield return 0;
    }

    private void SetText(string _baseText, int _counted)
    {
        countText.text = _baseText + _counted;
    }
}
