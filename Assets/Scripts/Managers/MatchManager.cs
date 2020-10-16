#pragma warning disable 0649

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
    [SerializeField] private HealthBarBehaviour healthBar;

    [Header("Match Values")]
    [SerializeField] private MatchData matchData;


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
        matchData.timePlayed += Time.deltaTime;
    }

    public void ResetMatchValues()
    {
        matchData = new MatchData();

        gameStarted = false;
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
        matchData.amountOfLives--;

        if (matchData.amountOfLives >= 0)
        {
            BallBehaviour firstBall = BallManager.SP.CreateFirstBall();

            yield return new WaitForSeconds(0.8f);
            //Enable shoot button
            BallManager.SP.FireFirstBall(firstBall);
            //Update hp bar

        }
        else
        {
            //Game Over Screen
            GameUIController.SP.SetGameOverPanel(true);
            gameStarted = false;
        }
        healthBar.UpdateLives();
        yield return 0;
    }

    /// <summary>
    /// Called from the inspector
    /// </summary>
    public void PlayGame()
    {
        //TODO delay here, first create the grid of some sort
        BallBehaviour first = BallManager.SP.CreateFirstBall();

        BallManager.SP.FireFirstBall(first);
        gameStarted = true;
    }

    public void AddLive(int live = 1)
    {
        matchData.amountOfLives++;
        healthBar.UpdateLives();
    }

    public void AddScore(int _score)
    {
        matchData.matchScore += _score;
        GameUIController.SP.SetScoreText(matchData.matchScore);
    }

    public void AddBrickHit(BrickColor brick)
    {
        switch (brick)
        {
            case BrickColor.Blue:
                matchData.blueBricksHit++;
                break;
            case BrickColor.Green:
                matchData.greenBricksHit++;
                break;
            case BrickColor.Orange:
                matchData.orangeBricksHit++;
                break;
            case BrickColor.Pink:
                matchData.pinkBricksHit++;
                break;
            case BrickColor.Purple:
                matchData.purpleBricksHit++;
                break;
            case BrickColor.Red:
                matchData.redBricksHit++;
                break;
            case BrickColor.None:
                break;
            default:
                break;
        }
    }

    public void GoToMainMenu()
    {
        GameManager.SP.GoToMainMenu();

        //Add score to player
        //Check if new highScore
        GameManager.SP.GetPlayerData.playerTotalScore += matchData.matchScore;

        if (matchData.matchScore > GameManager.SP.GetPlayerData.playerHighScore)
        {
            //New HighScore
            GameManager.SP.GetPlayerData.playerHighScore = matchData.matchScore;
            //Do something awesome !
        }
        GameManager.SP.SaveGame();
    }

    public int GetScore => matchData.matchScore;

    public PlayerBehaviour GetPB => pb;

    public bool GameStarted => gameStarted;

    public int GetMatchLives => matchData.amountOfLives;

    public HealthBarBehaviour GetHealthbar => healthBar;
}
