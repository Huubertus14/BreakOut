using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverBehaviour : MonoBehaviour
{

    [SerializeField] private TextCounter scoreText;
    [SerializeField] private TextCounter neededText;

    TweenIn tweenIn;
    TweenOut tweenOut;
    private void Awake()
    {
        tweenIn = GetComponentInChildren<TweenIn>();
        tweenOut = GetComponentInChildren<TweenOut>();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        tweenIn.StartTween(0.9f);
    }

    public void CreateEndGamePanel()
    {
        StartCoroutine(CreatePanel());
    }

    private IEnumerator CreatePanel()
    {
        yield return new WaitForSeconds(0.9f);
        //Check for touch then skip this
        scoreText.SetCounter(MatchManager.SP.GetScore);
        yield return new WaitForSeconds(1.1f);
        neededText.SetCounter(MatchManager.SP.GetScore * 1.1f);

        yield return 0;
    }


    public void SetInactive()
    {
        if (gameObject.activeSelf)
        {
            if (tweenOut == null)
            {
                tweenOut = GetComponentInChildren<TweenOut>();
            }
            tweenOut.StartTween(0.5f);
        }
    }
}
