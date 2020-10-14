using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFeedbackController : SingetonMonobehaviour<TextFeedbackController>
{
    [Header("Prefabs:")]
    [SerializeField] private TextFeedbackBehaviour textFeedbackPrefab;

    [Header("Refs:")]
    [SerializeField] private GameObject scoreObject;

    private Queue<TextFeedbackBehaviour> textPool;
    private int textPoolLength = 25;

    private void Start()
    {
        textPool = new Queue<TextFeedbackBehaviour>();

        for (int i = 0; i < textPoolLength; i++)
        {
            TextFeedbackBehaviour temp = Instantiate(textFeedbackPrefab, transform.position, Quaternion.identity, transform);
            textPool.Enqueue(temp);
            temp.gameObject.SetActive(false);
        }
    }

    public TextFeedbackBehaviour GetText(BricksAbstract brickValue)
    {
        TextFeedbackBehaviour temp = textPool.Dequeue();
        temp.gameObject.SetActive(true);

        temp.SetFeedback(brickValue.GetScore.ToString(), GameConstants.GetColorFromBrick(brickValue.GetColor), brickValue.transform.position, scoreObject.transform.position);

        textPool.Enqueue(temp);

        return temp;
    }
}
