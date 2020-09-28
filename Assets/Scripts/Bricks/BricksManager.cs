using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BricksManager : SingetonMonobehaviour<BricksManager>
{
    [Header("Refs:")]
    [SerializeField]
    private BricksAbstract brickPrefab;

    private BricksAbstract[] bricksArray;

    private bool bricksCreated = false;

    protected override void Awake()
    {
        base.Awake();
        bricksCreated = false;
        bricksArray = new BricksAbstract[GameConstants.BRICK_AMOUNT];
    }

    private IEnumerator Start()
    {
        StartCoroutine(CreateBeginBricks(GameConstants.BRICK_AMOUNT));
        yield return new WaitUntil(() => bricksCreated == true);
        CreateBox(10);
    }

    public IEnumerator CreateBeginBricks(int _amount)
    {
        float _curX = -2f; ;
        float _curY = 4;
        //Creeate brick queue
        for (int i = 0; i < _amount; i++)
        {
            GameObject tempBrick = Instantiate(brickPrefab.gameObject, transform.position, Quaternion.identity, transform);
            bricksArray[i] = tempBrick.GetComponent<BricksAbstract>();

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

        for (int i = 0; i < bricksArray.Length; i++)
        {
            bricksArray[i].gameObject.SetActive(false);
        }
        bricksCreated = true;
        yield return 0;
    }

    public void CreateBox(int _yLength)
    {
        int counter = 0;
        Color setCol = Color.red;

        for (int i = 0; i <= _yLength ; i++)
        {
            setCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            
            for (int j = 0; j <= GameConstants.COLUMNCOUNT; j++)
            {
                bricksArray[counter].gameObject.SetActive(true);
                bricksArray[counter].SetPosition(i, j);
                bricksArray[counter].SetColor(setCol);
                counter++;
            }
        }
    }
}
