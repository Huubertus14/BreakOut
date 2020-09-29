using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BricksManager : SingetonMonobehaviour<BricksManager>
{
    [Header("Refs:")]
    [SerializeField]
    private BricksAbstract brickPrefab;

    private BricksAbstract[] singleBrickArray; //used to loop through all bricks at once, like create and destroy
    [SerializeField] private BricksAbstract[,] brickArray; //ref all bricks used in the game right now, used to check if a pos is empty
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
    }

    private IEnumerator Start()
    {
        StartCoroutine(CreateBeginBricks(GameConstants.BRICK_AMOUNT));
        yield return new WaitUntil(() => bricksCreated == true);
        CreateBox(20);
        //Debug.Log("empty: " + isEmpty(0,0)+ " " + isEmpty(500,500));
        yield return new WaitForSeconds(3f);
       /* MoveAllBricksOneDown();
        yield return new WaitForSeconds(1.5f);*/
        AddRowOnTop();
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
        bricksCreated = true;
        yield return 0;
    }

    public void CreateBox(int _yLength)
    {
        for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
        {
            Color setCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            for (int j = 0; j < _yLength; j++)
            {
                BricksAbstract _temp = unusedBricks.Dequeue();
                brickArray[i, j] = _temp;
                _temp.gameObject.SetActive(true);
                _temp.SetPosition(i, j);
                _temp.SetPosToSide(Random.Range(-15f, 15f));
                _temp.SetColor(setCol);
                activeBricks.Add(_temp);
            }
        }
    }

    public bool isEmpty(int _x, int _y)
    {
        //Check if x & Y are in range
        try
        {
            bool empty = true;
            if (brickArray[_x, _y] == null || !brickArray[_x, _y].gameObject.activeSelf)
            {
                empty = false;
            }
            return empty;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
            throw;
        }

    }

    public BricksAbstract GetBrickAt(Vector2 _index)
    {
        BricksAbstract tempBrick = null;
        if (isEmpty((int)_index.x, (int)_index.y))
        {
            //No brick found
            return tempBrick;
        }
        else
        {
            tempBrick = brickArray[(int)_index.x, (int)_index.y];
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
                MoveBrick(activeBricks[i], _movePlace, 3, true, true);
            }
        }
    }

    public void MoveBrick(BricksAbstract _movedBrick, Vector2 _newPos, int _tryCount, bool randomDelay = false, bool _forceMove = false)
    {
        if (isEmpty((int)_newPos.x, (int)_newPos.y))
        {
            brickArray[(int)_movedBrick.GetIndex.x, (int)_movedBrick.GetIndex.y] = null;
            _movedBrick.SetGoalPosition((int)_newPos.x, (int)_newPos.y, randomDelay);
            brickArray[(int)_newPos.x, (int)_newPos.y] = _movedBrick;
        }
        else
        {
            if (_forceMove)
            {
                BricksAbstract _temp = GetBrickAt(_newPos);

                brickArray[(int)_movedBrick.GetIndex.x, (int)_movedBrick.GetIndex.y] = null;
                _movedBrick.SetGoalPosition((int)_newPos.x, (int)_newPos.y, randomDelay);
                brickArray[(int)_newPos.x, (int)_newPos.y] = _movedBrick;

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
                Color setCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                _tempBrick.SetColor(setCol);
            }
        }
        //

    }
}
