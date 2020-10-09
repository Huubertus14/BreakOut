using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private TweenAbstract hitWiggle;

    private float maxVelocity = 5; //5 is a lot

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitWiggle = GetComponent<TweenAbstract>();
        rb.Sleep();
        rb.isKinematic = true;
        
    }

    public void ShootBall(float _force)
    {
        rb.isKinematic = false;
        rb.AddForce(_force * Vector3.up, ForceMode2D.Force);
    }

    public void BounceOff(Vector3 _offPoint)
    {
        Vector3 _dir = transform.position - _offPoint;
        _dir.Normalize();
        rb.AddForce(_dir);
    }

    private void Update()
    {
        SetVelocity();
    }

    private void SetVelocity()
    {
        if (rb.velocity.sqrMagnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BricksAbstract ab = collision.gameObject.GetComponent<BricksAbstract>();
        if (ab != null)
        {
            ab.HitBrick();
            MatchManager.SP.AddScore(ab.GetScore);
            hitWiggle.StartTween(0.2f);
            return;
        }

        BorderBehaviour bb = collision.gameObject.GetComponent<BorderBehaviour>();
        if (bb != null)
        {
            BounceOff(collision.contacts[0].point);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DeathZoneBehaviour dz = collision.gameObject.GetComponent<DeathZoneBehaviour>();
        if (dz != null)
        {
            //ball enterd the dead zone
            BallManager.SP.BallDeath(this);
        }
    }

}
