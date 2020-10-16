using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallPower
{
    Bomb,
    NoCollision,
    None
}

public class BallBehaviour : MonoBehaviour
{
    private SpriteRenderer ren;
    private Rigidbody2D rb;
    private TweenAbstract hitWiggle;
    private float maxVelocity = 5; //5 is a lot
    [SerializeField] private BallPower ballPower;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitWiggle = GetComponent<TweenAbstract>();
        ren = GetComponent<SpriteRenderer>();
        rb.Sleep();
        rb.isKinematic = true;
    }

    private void OnEnable()
    {
        ballPower = BallPower.None;
        ren.color = GetColor(ballPower);
    }

    public void MakeSpecial(BallPower newPower)
    {
        ren.color = GetColor(newPower);
        ballPower = newPower;
    }

    public void ShootBall(float _force)
    {
        rb.isKinematic = false;
        rb.AddForce(_force * Vector3.up, ForceMode2D.Force);
    }

    public void ShootBall(Vector2 newVelocity)
    {
        rb.isKinematic = false;
        rb.velocity = newVelocity;
    }

    public void BounceOff(Vector3 _offPoint)
    {
        Vector3 _dir = transform.position - _offPoint;
        _dir.Normalize();
        rb.AddForce(_dir);
    }

    public void SplitBall()
    {
        //Create a new ball
        BallBehaviour otherBall = BallManager.SP.CreateBall();
        if (otherBall != null)
        {
            Vector2 otherVel = rb.velocity;
            otherVel.x *= -1;

            if (otherVel.x == 0)
            {
                otherVel.x = (Random.Range(1, 10) % 2 == 1) ? -3f : 3f;
            }

            otherBall.transform.position = transform.position;
            otherBall.ShootBall(otherVel);
        }
    }

    private void Update()
    {
        ClampVelocity();
    }

    private void ClampVelocity()
    {
        if (rb.velocity.sqrMagnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

        if (rb.velocity.y < 0.5f && rb.velocity.y > -0.5f)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.5f);
            }
            else if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -0.5f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BricksAbstract ab = collision.gameObject.GetComponent<BricksAbstract>();
        if (ab != null)
        {
            ab.HitBrick(ballPower);
            FeedbackController.SP.GetText(ab);
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

    private Color GetColor(BallPower bp)
    {
        switch (bp)
        {
            case BallPower.Bomb:
                return Color.red;
            case BallPower.NoCollision:
                return Color.cyan;
            case BallPower.None:
                return Color.white;
            default:
                return Color.white;
        }
    }

    public BallPower GetBallPower => ballPower;
    public Rigidbody2D GetRigidBody => rb;
}
