using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class GameUIController : SingetonMonobehaviour<GameUIController>
{
    [Header("Refs:")]
    [SerializeField] private TextMeshProUGUI scoreText;




    public void LeaveGame()
    {
        GameManager.SP.GoToMainMenu();
    }

    public void SetScoreText(int _score)
    {
        scoreText.text = "Score: " + _score;
    }
}
