using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameUIController : SingetonMonobehaviour<GameUIController>
{
    [Header("Refs:")]
    [SerializeField] private TextCounter scoreText;
    [SerializeField] private GameOverBehaviour gameOverPanel;

    [Header("PlayButton Refs")]
    [SerializeField] private TweenAbstract playButIn;
    [SerializeField] private TweenAbstract playButOut;

    protected override void Awake()
    {
        base.Awake();
        playButOut.enabled = false;
    }

    public void LeaveGame()
    {
        GameManager.SP.GoToMainMenu();
    }

    public void SetScoreText(int _score)
    {
        scoreText.SetCounter(_score);
    }

    public void SetGameOverPanel(bool _value)
    {
        if (_value)
        {
            gameOverPanel.gameObject.SetActive(true);
            gameOverPanel.CreateEndGamePanel();
        }
        else
        {
            //gameOverPanel.gameObject.SetActive(false);
            gameOverPanel.SetInactive();
        }
    }

    public void SetPlayButton(bool _value)
    {
        playButIn.gameObject.SetActive(true);
        if (_value)
        {
            playButIn.enabled = true;
            playButOut.enabled = false;
            playButIn.StartTween(0.4f);
        }
        else
        {
            playButIn.enabled = false;
            playButOut.enabled = true;

            playButOut.StartTween(0.6f);
        }
    }
}
