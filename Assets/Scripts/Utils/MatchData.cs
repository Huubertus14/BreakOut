using System;
using UnityEngine;

[Serializable]
public class MatchData
{
    public float timePlayed;
    public int matchScore;
    public int amountOfLives;
    [Space]
    //Bricks Hit
    public int pinkBricksHit;
    public int redBricksHit;
    public int blueBricksHit;
    public int orangeBricksHit;
    public int purpleBricksHit;
    public int greenBricksHit;
    [Space]
    public int powerUpPicked;
    public int powerUpsMissed;
    [Space]
    public int totalBallsSpawned;

    public MatchData()
    {
        ResetData();
    }

    public void ResetData()
    {
        timePlayed = 0;
        matchScore = 0;
        amountOfLives = 2;

        pinkBricksHit = 0;
        redBricksHit = 0;
        greenBricksHit = 0;
        pinkBricksHit = 0;
        purpleBricksHit = 0;
        blueBricksHit = 0;
        powerUpPicked = 0;
        powerUpsMissed = 0;

        totalBallsSpawned = 0;

    }

}
