using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Different from game manager
/// This scripts does all the things during the gameplay and not around it
/// </summary>
public class MatchManager : SingetonMonobehaviour<MatchManager>
{
    [Header("Refs:")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject ballParent;
    [SerializeField] private PlayerBehaviour pb;

    private List<BallBehaviour> allBalls;

    private void Start()
    {
        allBalls = new List<BallBehaviour>();
        //Create base ball
        GameObject _firstball = Instantiate(ballPrefab, pb.transform.position, Quaternion.identity, pb.transform);
        _firstball.transform.localPosition = new Vector3(_firstball.transform.localPosition.x, _firstball.transform.localPosition.y + 0.3f, _firstball.transform.localPosition.z);

        allBalls.Add(_firstball.GetComponent<BallBehaviour>());
    }

    public void PlayGame()
    {
        allBalls[0].ShootBall(150f);
        allBalls[0].transform.SetParent(ballParent.transform);
    }


    public GameObject GetBallPrefab => ballPrefab;
}
