using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifePowerUp : PowerUpAbstract
{
    
    public override void ApplyPowerUp()
    {
        MatchManager.SP.AddLive();
    }

}
