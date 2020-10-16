#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : SingetonMonobehaviour<BallManager>
{
    [SerializeField] private BallBehaviour ballPrefab;
    [SerializeField] private GameObject ballParent;

    [SerializeField] private List<BallBehaviour> ballsInGame;
    private Queue<BallBehaviour> ballPool;
    private int poolSize = 90;

    private void Start()
    {
        ballPool = new Queue<BallBehaviour>();
        ballsInGame = new List<BallBehaviour>();

        CreatePool();
        // CreateFirstBall();
    }

    private void CreatePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            BallBehaviour temp = Instantiate(ballPrefab, MatchManager.SP.GetPB.transform.position, Quaternion.identity, ballParent.transform);
            ballPool.Enqueue(temp);
            temp.gameObject.SetActive(false);
        }
    }

    public void FireFirstBall(BallBehaviour ball)
    {
        ball.ShootBall(150f);
        ball.transform.SetParent(ballParent.transform);
        ballsInGame.Add(ball);
    }

    public BallBehaviour CreateFirstBall()
    {
        //Create base ball
        BallBehaviour _firstball = CreateBall();
        _firstball.GetRigidBody.Sleep();
        _firstball.GetRigidBody.isKinematic = true;
        _firstball.transform.SetParent(MatchManager.SP.GetPB.transform);
        _firstball.transform.localPosition = Vector3.zero;
        //BallBehaviour _firstball = Instantiate(ballPrefab, MatchManager.SP.GetPB.transform.position, Quaternion.identity, MatchManager.SP.GetPB.transform);
        _firstball.transform.localPosition = new Vector3(_firstball.transform.localPosition.x, _firstball.transform.localPosition.y + 0.3f, _firstball.transform.localPosition.z);
        _firstball.GetComponent<TweenAbstract>().StartTween(0.1f);
        return _firstball;
    }

    public BallBehaviour CreateBall()
    {
        if (ballPool.Count > 0)
        {
            BallBehaviour ball = ballPool.Dequeue();
            ballsInGame.Add(ball);
            ball.gameObject.SetActive(true);
            return ball;
        }
        else
        {
            return null;
        }
    }


    public void CheckBalls()
    {
        if (ballsInGame.Count > 0)
        {
            for (int i = 0; i < ballsInGame.Count; i++)
            {
                if (ballsInGame[i] != null)
                {
                    if (!ballsInGame[i].gameObject.activeSelf)
                    {
                        continue;
                    }
                    return;
                }

            }
        }
        StartCoroutine(MatchManager.SP.PlayerDied());
    }

    public void BallDeath(BallBehaviour ball)
    {
        ballPool.Enqueue(ball);
        ballsInGame.Remove(ball);
        ball.gameObject.SetActive(false);
        CheckBalls();
    }

    public BallBehaviour[] GetAllBalls => ballsInGame.ToArray();
}
