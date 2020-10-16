using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSplitPowerUp : PowerUpAbstract
{
    public override void ApplyPowerUp()
    {
        foreach (var item in BallManager.SP.GetAllBalls)
        {
            item.SplitBall();
        }

        
        gameObject.SetActive(false);
    }

}
