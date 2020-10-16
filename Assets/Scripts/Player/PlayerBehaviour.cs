#pragma warning disable 0649

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float paddleSize = 1.5f;
    private TweenAbstract hitWiggle;

    private Queue<PowerUpAbstract> powerUpQueue;

    private void Start()
    {
        //Create ball prefab
        hitWiggle = GetComponent<TweenAbstract>();
        powerUpQueue = new Queue<PowerUpAbstract>();
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            //Go to pos of first touch
        }

        SetPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
    }

    private void SetPosition(float _Xpos)
    {
        _Xpos = Mathf.Clamp(_Xpos, -2.5f, 2.5f);
        transform.position = new Vector3(_Xpos, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallBehaviour bb = collision.gameObject.GetComponent<BallBehaviour>();
        if (bb != null)
        {
            hitWiggle.StartTween(0.2f);

            if (bb.GetBallPower == BallPower.None) //Only do this to balls that are not special
            {
                if (powerUpQueue.Count > 0)
                {
                    powerUpQueue.Dequeue().ApplyPowerUp(bb);
                }
            }

        }
    }

    public void PowerUp(PowerUpAbstract pu)
    {
        if (pu.queueAble)
        {
            powerUpQueue.Enqueue(pu);
            pu.DisablePowerUp();
        }
        else
        {
            pu.ApplyPowerUp();
        }
    }
}
