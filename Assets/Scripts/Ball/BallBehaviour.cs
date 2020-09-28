using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BricksAbstract ab = collision.gameObject.GetComponent<BricksAbstract>();
        if (ab != null)
        {
            ab.HitBrick();
            return;
        }

        BorderBehaviour bb = collision.gameObject.GetComponent<BorderBehaviour>();
        if (bb != null)
        {
            BounceOff(collision.contacts[0].point);
        }
    }
}
