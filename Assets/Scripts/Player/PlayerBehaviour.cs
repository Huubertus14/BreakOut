using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float wideSize = 1.5f;

    private TweenAbstract hitWiggle;
    private void Start()
    {
        //Create ball prefab
        hitWiggle = GetComponent<TweenAbstract>();
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
        }
    }
}
