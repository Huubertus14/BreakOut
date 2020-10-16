using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp : PowerUpAbstract
{
    [SerializeField] private BallPower ballPower;

    public override void ApplyPowerUp()
    {
        gameObject.SetActive(false);
    }

    public override void ApplyPowerUp(BallBehaviour bb = null)
    {
        base.ApplyPowerUp(bb);
        bb.MakeSpecial(ballPower);
        ApplyPowerUp();
    }
}
