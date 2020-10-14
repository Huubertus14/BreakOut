using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpAbstract : MonoBehaviour, IPowerUp
{
    public abstract void ApplyPowerUp();

    public float Duration()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        //
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it hits the paddle or the deadthzone
        DeathZoneBehaviour dz = collision.gameObject.GetComponent<DeathZoneBehaviour>();
        if (dz != null)
        {
            //Remove here, aka place in the new queueue
        }

        PlayerBehaviour pb = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (pb != null)
        {
            //Apply powerup
            pb.PowerUp(this);
        }
    }
}
