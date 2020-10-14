using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearthBehaviour : MonoBehaviour
{
    [SerializeField] private TweenAbstract fadeIn;
    private void Awake()
    {
       // fadeIn = GetComponent<TweenAbstract>();
    }

    public void EnableHearth()
    {
        fadeIn.StartTween(0.8f);
    }

    public void DisableHearth()
    {

    }
}
