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

    private bool bricksCreated = false;

    protected override void Awake()
    {
        base.Awake();
        bricksCreated = false;
        singleBrickArray = new BricksAbstract[GameConstants.BRICK_AMOUNT];
        brickArray = new BricksAbstract[GameConstants.COLUMNCOUNT, GameConstants.ROWSCOUNT];
    }

    private IEnumerator Start()
    {
        StartCoroutine(CreateBeginBricks(GameConstants.BRICK_AMOUNT));
        yield return new WaitUntil(() => bricksCreated == true);
        CreateBox(20);
        //Debug.Log("empty: " + isEmpty(0,0)+ " " + isEmpty(500,500));
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
            singleBrickArray[i].gameObject.SetActive(false);
        }
        bricksCreated = true;
        yield return 0;
    }

    public void CreateBox(int _yLength)
    {
        int counter = 0;
        Color setCol = Color.red;

        for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
        {
            setCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            for (int j = 0; j < _yLength; j++)
            {
                brickArray[i, j] = singleBrickArray[counter];

                singleBrickArray[counter].gameObject.SetActive(true);
                singleBrickArray[counter].SetPosition(i, j);
                singleBrickArray[counter].SetPosToSide(Random.Range(-15f, 15f));
                singleBrickArray[counter].SetColor(setCol);
                counter++;
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

    public BricksAbstract GetBrickAt(int x, int y)
    {
        BricksAbstract tempBrick = null;
        if (isEmpty(x, y))
        {
            //No brick found
            return tempBrick;
        }
        else
        {
            tempBrick = brickArray[x, y];
        }

        return tempBrick;
    }

    public Vector2 FindEmptyPlace()
    {
        for (int i = 0; i < GameConstants.COLUMNCOUNT; i++)
        {
            for (int j = 0; j < GameConstants.ROWSCOUNT; j++)
            {
                if (brickArray[i,j] != null )
                {
                    if (brickArray[i,j].gameObject.activeSelf)
                    {
                       return new Vector2(i,j);
                    }
                }
                else
                {
                    return new Vector2(i, j);
                }
            }
        }
        Debug.Log("Game Over?");
        return new Vector2(0,0);
    }

    public void MoveBrick(BricksAbstract _movedBrick, int _newPosX, int _newPosY, bool _forceMove = false)
    {
        if (isEmpty(_newPosX, _newPosY))
        {
            _movedBrick.SetGoalPosition(_newPosX, _newPosY);
        }
        else
        {
            if (_forceMove)
            {
                BricksAbstract _temp = GetBrickAt(_newPosX, _newPosY);
                _movedBrick.SetGoalPosition(_newPosX, _newPosY);
                Vector2 _newPlace = FindEmptyPlace();
                _temp.SetGoalPosition((int)_newPlace.x, (int)_newPlace.y);
            }
        }
    }

    //Score /1000 is speed of block replacing
    public void CreateNewBlocks()
    {
        Vector2 newCoordiantes = new Vector2(Random.Range(0, GameConstants.COLUMNCOUNT), Random.Range(0, GameConstants.ROWSCOUNT));

        //Find an empty place
    }



}
