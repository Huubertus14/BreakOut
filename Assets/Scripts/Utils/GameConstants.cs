using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    //Brick values
    public const int BRICK_AMOUNT = 462;
    public const float XSTEP = 0.5f;
    public const float YSTEP = 0.3f;
    public const float XBEGIN = -2f;
    public const float YBEGIN = 3.8f;
    public const int ROWSCOUNT = 42;
    public const int COLUMNCOUNT = 9;

    //Game values
    public const int MAXLIVES = 6;
    public const int MAXBALLS = 50;

    public static Color[] GetColors
    {
        get { return new Color[] { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow }; }
    }

    public static Color GetColorFromBrick(BrickColor color)
    {
        switch (color)
        {
            case BrickColor.Blue:
                return Color.blue;
            case BrickColor.Green:
                return Color.green;
            case BrickColor.Orange:
                Debug.Log("Set right color");
                return Color.white;
            case BrickColor.Pink:
                Debug.Log("Set right color");
                return Color.white;
            case BrickColor.Purple:
                Debug.Log("Set right color");
                return Color.white;
            case BrickColor.Red:
                return Color.red;
            case BrickColor.None:
                return Color.white;
            default:
                return Color.white;
        }
    }
}
