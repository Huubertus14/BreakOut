﻿#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using UnityEngine;

public enum BrickColor
{
    Blue,
    Green,
    Orange,
    Pink,
    Purple,
    Red,
    None
}

public class BricksManager : SingetonMonobehaviour<BricksManager>
{
    [Header("Refs:")]
    [SerializeField] private BricksAbstract brickPrefab;
    [SerializeField] private Sprite[] brickSprites; //Are set in the inspector


    private Dictionary<BrickColor, Sprite> brickDictionairy;

    private BricksAbstract[] singleBrickArray; //used to loop through all bricks at once, like create and destroy
    private BricksAbstract[,] brickArray; //ref all bricks used in the game right now, used to check if a pos is empty
    private List<BricksAbstract> activeBricks;
    private Queue<BricksAbstract> unusedBricks;

    private bool bricksCreated = false;

    protected override void Awake()
    {
        base.Awake();
        bricksCreated = false;
        singleBrickArray = new BricksAbstract[GameConstants.BRICK_AMOUNT];
        unusedBricks = new Queue<BricksAbstract>();
        brickArray = new BricksAbstract[GameConstants.COLUMNCOUNT, GameConstants.ROWSCOUNT];
        activeBricks = new List<BricksAbstract>();

        brickDictionairy = new Dictionary<BrickColor, Sprite>();
        brickDictionairy.Add(BrickColor.Blue, brickSprites[0]);
        brickDictionairy.Add(BrickColor.Green, brickSprites[1]);
        brickDictionairy.Add(BrickColor.Orange, brickSprites[2]);
        brickDictionairy.Add(BrickColor.Pink, brickSprites[3]);
        brickDictionairy.Add(BrickColor.Purple, brickSprites[4]);
        brickDictionairy.Add(BrickColor.Red, brickSprites[5]);
    }

    private IEnumerator Start()
    {
        StartCoroutine(CreateBeginBricks(GameConstants.BRICK_AMOUNT));
        yield return new WaitUntil(() => bricksCreated == true);
        CreateBox(20);
        for (int i = 0; i < activeBricks.Count; i++)
        {
            activeBricks[i].GetNeighbours();
        }
    }

    public IEnumerator CreateBeginBricks(int _amount)
    {
        float _curX = -2f; ;
        float _curY = 4;
        //Creeate brick queue
        for (int i = 0; i < _amount; i++)
        {
            GameObject tempBrick = Instantiate(brickPrefab.gameObject, transform.position, Quaternion.identity, transform);
            singleBrickArray[i] = tempBrick.GetComponent<BricksAbstract>();

            Vector3 newPos = new Vector3(_curX, _curY, 0);
            _curX += GameConstants.XSTEP;

            tempBrick.transform.localPosition = newPos;
            //tempBrick.GetComponent<BricksAbstract>().SetGoalPosition(newPos);
            if (_curX >= 2.0f)
            {
                _curX = -2.0f;
                _curY -= GameConstants.YSTEP;
                //yield return 0;
            }
        }

        for (int i = 0; i < singleBrickArray.Length; i++)
        {
            unusedBricks.Enqueue(singleBrickArray[i]);
            singleBrickArray[i].gameObject.SetActive(false);
        }
        yield return 0;

        bricksCreated = true;
    }

    public void RemoveBrick(BricksAbstract _brick)
    {
        if (_brick != null)
        {
            unusedBricks.Enqueue(_brick);
            brickArray[(int)_brick.GetIndex.x, (int)_brick.GetIndex.y] = null;
            activeBricks.Remove(_brick);
            _brick.gameObject.SetActive(false);
        }
    }

    public void CreateBox(int _yLength)
    {
        for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
        {
            for (int j = 0; j < _yLength; j++)
            {
                BricksAbstract _temp = unusedBricks.Dequeue();
                brickArray[i, j] = _temp;
                _temp.gameObject.SetActive(true);
                _temp.SetPosition(i, j);
                _temp.SetPosToSide(Random.Range(-15f, 15f));
                Sprite _tempSpr = GetRandomBrickSprite();
                _temp.SetSprite(_tempSpr, GetColorFromSprite(_tempSpr));
                activeBricks.Add(_temp);
            }
        }
    }

    public bool isEmpty(Vector2 _index)
    {
        //Check if x & Y are in range
        try
        {
            bool empty = false;
            if (brickArray[(int)_index.x, (int)_index.y] == null || !brickArray[(int)_index.x, (int)_index.y].gameObject.activeSelf)
            {
                empty = true;
            }
            return empty;
        }
        catch (System.Exception e)
        {
            //Debug.LogError(e.Message);
            return false;
            //throw;
        }
    }

    public BricksAbstract GetBrickAt(Vector2 _index)
    {
        BricksAbstract tempBrick = null;
        try
        {
            tempBrick = brickArray[(int)_index.x, (int)_index.y];
        }
        catch (System.Exception)
        {
        }

        return tempBrick;
    }

    public Vector2 FindEmptyPlace()
    {
        for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
        {
            for (int j = 0; j < GameConstants.ROWSCOUNT; j++)
            {
                if (brickArray[i, j] != null)
                {
                    if (brickArray[i, j].gameObject.activeSelf)
                    {
                        return new Vector2(i, j);
                    }
                }
                else
                {
                    return new Vector2(i, j);
                }
            }
        }
        Debug.Log("Game Over?");
        return new Vector2(0, 0);
    }

    public void MoveAllBricksOneDown()
    {
        for (int i = 0; i < activeBricks.Count; i++)
        {
            if ((int)activeBricks[i].GetIndex.x + 1 < GameConstants.ROWSCOUNT) //check if row exists
            {
                Vector2 _movePlace = activeBricks[i].GetIndex;
                _movePlace.y++;
                MoveBrick(activeBricks[i], _movePlace, 3, false, true);
            }
        }
    }

    public void AddToDestroy(BricksAbstract[] bricks)
    {
        foreach (var item in bricks)
        {
            RemoveBrick(item);
        }
    }

    public void MoveBrick(BricksAbstract _movedBrick, Vector2 _newPos, int _tryCount, bool randomDelay = false, bool _forceMove = false)
    {
        if (isEmpty(_newPos))
        {
            Vector2 _prefPos = _movedBrick.GetIndex;
            _movedBrick.SetGoalPosition((int)_newPos.x, (int)_newPos.y, randomDelay);
            brickArray[(int)_newPos.x, (int)_newPos.y] = _movedBrick;
            brickArray[(int)_prefPos.x, (int)_prefPos.y] = null;
        }
        else
        {
            if (_forceMove)
            {
                BricksAbstract _temp = GetBrickAt(_newPos);

                Vector2 _prefPos = _movedBrick.GetIndex;
                _movedBrick.SetGoalPosition((int)_newPos.x, (int)_newPos.y, randomDelay);
                brickArray[(int)_newPos.x, (int)_newPos.y] = _movedBrick;
                brickArray[(int)_prefPos.x, (int)_prefPos.y] = null;

                if (_temp != null)
                {
                    if (_tryCount <= 0)
                    {
                        Vector2 _tempPos = FindEmptyPlace();
                        MoveBrick(_temp, _tempPos, 1);
                    }
                    else
                    {
                        _newPos.y++;
                        MoveBrick(_temp, _newPos, _tryCount--);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Ads a row on top of all (at y == 0) 
    /// </summary>
    public void AddRowOnTop()
    {
        if (unusedBricks.Count > GameConstants.COLUMNCOUNT)
        {
            MoveAllBricksOneDown();
            for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
            {
                BricksAbstract _tempBrick = unusedBricks.Dequeue();
                _tempBrick.gameObject.SetActive(true);

                brickArray[i, 0] = _tempBrick;
                _tempBrick.SetPosition(i, 0);
                _tempBrick.SetPosToTop(Random.Range(2f, 5f));

                Sprite _temp = GetRandomBrickSprite();
                _tempBrick.SetSprite(_temp, GetColorFromSprite(_temp));
                activeBricks.Add(_tempBrick);
            }
        }
    }

    public Sprite GetRandomBrickSprite()
    {
        int _index = Random.Range(0, brickSprites.Length);
        return brickSprites[_index];
    }

    public BrickColor GetColorFromSprite(Sprite _sprite)
    {
        foreach (var item in brickDictionairy)
        {
            if (item.Value == _sprite)
            {
                return item.Key;
            }
        }
        Debug.LogWarning("Could not find a Sprite");
        return BrickColor.None;
    }

    public Sprite GetSpecificBrickSprite(BrickColor _color)
    {
        if (_color == BrickColor.None) //If none color than dont try to find anything
        {
            Debug.LogWarning("Tried to get Color NONE");
            return brickSprites[0];
        }
        if (brickDictionairy.TryGetValue(_color, out Sprite value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Tried to get Color: " + _color.ToString());
            return brickSprites[0];
        }
    }

}
