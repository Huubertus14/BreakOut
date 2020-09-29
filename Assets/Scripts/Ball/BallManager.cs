using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : SingetonMonobehaviour<BallManager>
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject ballParent;


    [SerializeField] private List<BallBehaviour> allBalls;

    private void Start()
    {
        allBalls = new List<BallBehaviour>();
        CreateFirstBall();
    }

    public void FireFirstBall()
    {
        allBalls[0].ShootBall(150f);
        allBalls[0].transform.SetParent(ballParent.transform);
    }

    public void CreateFirstBall()
    {
        //Create base ball
        GameObject _firstball = Instantiate(ballPrefab, MatchManager.SP.GetPB.transform.position, Quaternion.identity, MatchManager.SP.GetPB.transform);
        _firstball.transform.localPosition = new Vector3(_firstball.transform.localPosition.x, _firstball.transform.localPosition.y + 0.3f, _firstball.transform.localPosition.z);

        _firstball.GetComponent<ScaleTween>().StartTween(0.1f);
        allBalls.Add(_firstball.GetComponent<BallBehaviour>());
    }

    public void CheckBalls()
    {
        if (allBalls.Count > 0)
        {
            for (int i = 0; i < allBalls.Count; i++)
            {
                if (allBalls[i] != null)
                {
                    if (allBalls[i].gameObject.activeSelf)
                    {
                        return;
                    }
                    return;
                }

            }
        }
        StartCoroutine(MatchManager.SP.PlayerDied());
    }

    public void BallDeath(BallBehaviour _ball)
    {
        allBalls.Remove(_ball);
        CheckBalls();
    }
}
