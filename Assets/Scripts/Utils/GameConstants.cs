using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstants
{
    public const int BRICK_AMOUNT = 462;

    public const float XSTEP = 0.5f;

    public const float YSTEP = 0.3f;

    public const float XBEGIN = -2f;
    public const float YBEGIN = 3.8f;

    public const int ROWSCOUNT = 42;

    public const int COLUMNCOUNT = 9;

    public static Color[] GetColors
    {
        get { return new Color[] { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow }; }
    }

}
