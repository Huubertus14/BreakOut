using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Different from game manager
/// This scripts does all the things during the gameplay and not around it
/// </summary>
public class MatchManager : SingetonMonobehaviour<MatchManager>
{
    [Header("Refs:")]
    [SerializeField] private PlayerBehaviour pb;


    [Header("Match Values")]
    [SerializeField] private float timePlayed;
    [SerializeField] private int matchScore;
    [SerializeField] private int amountOfLives;

    private bool gameStarted = false;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        ResetMatchValues();
    }

    private void Update()
    {
        timePlayed += Time.deltaTime;
    }

    public void ResetMatchValues() {
        matchScore = 0;
        timePlayed = 0;
        gameStarted = false;
        amountOfLives = 2;
        GameUIController.SP.SetGameOverPanel(false);
        GameUIController.SP.SetPlayButton(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator PlayerDied()
    {
        //Player Died
        amountOfLives--;
        gameStarted = false;
        if (amountOfLives >= 0)
        {
            BallManager.SP.CreateFirstBall();

            yield return new WaitForSeconds(0.8f);
            //Enable shoot button
            BallManager.SP.FireFirstBall();
        }
        else
        {
            //Game Over Screen
            GameUIController.SP.SetGameOverPanel(true);
        }
        yield return 0;
    }

    /// <summary>
    /// Called from the inspector
    /// </summary>
    public void PlayGame()
    {
        //TODO delay here, first create the grid of some sort
        BallManager.SP.FireFirstBall();
        gameStarted = true;
    }

    public void AddScore(int _score)
    {
        matchScore += _score;
        GameUIController.SP.SetScoreText(matchScore);
    }

    public void GoToMainMenu()
    {
        GameManager.SP.GoToMainMenu();

        //Add score to player
        //Check if new highScore
        GameManager.SP.GetPlayerData.playerTotalScore += matchScore;

        if (matchScore > GameManager.SP.GetPlayerData.playerHighScore)
        {
            //New HighScore
            GameManager.SP.GetPlayerData.playerHighScore = matchScore;
            //Do something awesome !
        }
        GameManager.SP.SaveGame();
    }

    public int GetScore => matchScore;

    public PlayerBehaviour GetPB => pb;

    public bool GameStarted => gameStarted;
}
