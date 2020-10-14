using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTween : TweenAbstract
{
    private Color orginColor;
    private Color goalColor;

    private void Awake()
    {
        orginColor = Color.white;
    }

    public void StartTween(float _duration, Color newColor)
    {
        //base.StartTween(_duration);

        StartCoroutine(DoTween());
    }

    protected override IEnumerator DoTween()
    {


        yield return 0;
    }
}
