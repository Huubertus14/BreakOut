using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Values")]
    [SerializeField] private float wideSize = 1.5f;

    private void Start()
    {
        //Create ball prefab
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
}
