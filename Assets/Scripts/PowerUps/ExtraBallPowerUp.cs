using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBallPowerUp : PowerUpAbstract
{
    BallBehaviour newBall;
    public override void ApplyPowerUp()
    {
        ren.enabled = false;
        newBall = BallManager.SP.CreateFirstBall();
        StartCoroutine(SpawnDelay());
    }

    private IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
        BallManager.SP.FireFirstBall(newBall);
        gameObject.SetActive(false);
    }
}
