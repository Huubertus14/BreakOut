using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BricksAbstract : MonoBehaviour
{
    [Header("BrickValues")]
    [SerializeField] protected int health;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int brickScore  = 100;

    private SpriteRenderer spriteRenderer;

    [SerializeField]private Vector2 brickIndexes;

    [SerializeField]protected Vector3 goalPosition;

    private float delayTimer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void HitBrick()
    {
        health -= 1;
        if (health <= 0)
        {
            DestroyBrick();
        }
    }

    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, goalPosition, Time.deltaTime * moveSpeed);
        }
    }

    public virtual void SetColor(Color _col)
    {
        spriteRenderer.color = _col;
    }

    public void SetPosToSide(float _side)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + _side, transform.localPosition.y, transform.localPosition.z);
    }

    public void SetPosToTop(float _top)
    {
        transform.localPosition = new Vector3(transform.localPosition.x , transform.localPosition.y +_top, transform.localPosition.z);
    }

    public virtual void SetGoalPosition(int _x, int _y, bool randomDelay = false)
    {
        if (randomDelay)
        {
            delayTimer = Random.Range(0.5f,1.2f);
        }
        else
        {
            delayTimer = -1;
        }

        brickIndexes.x = _x;
        brickIndexes.y = _y;
        //Set goal pos
        float xCalculated = -2f, yCalculated = 4f;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
    }

    public virtual void SetPosition(int _x, int _y)
    {
        brickIndexes.x = _x; 
        brickIndexes.y = _y;
        float xCalculated = -2f, yCalculated = 4f;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
        transform.localPosition = goalPosition;
    }

    protected void DestroyBrick()
    {
        BricksManager.SP.RemoveBrick(this);
    }

    public int GetScore => brickScore;

    public Vector2 GetIndex => brickIndexes;
}
