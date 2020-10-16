using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class BricksAbstract : MonoBehaviour
{
    [Header("BrickValues")]
    [SerializeField] protected int health;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected int brickScore = 100;
    [SerializeField] protected BrickColor brickColor;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Vector2 brickIndexes;
    [SerializeField] protected Vector3 goalPosition;

    private float delayTimer;

    [SerializeField] private BricksAbstract[] neighbours;
    private List<BricksAbstract> toDestroy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        toDestroy = new List<BricksAbstract>();

        transform.localScale = new Vector3(0.15f, 0.1f, 1);
    }

    public void GetNeighbours()
    {
        Vector2[] indexes = new Vector2[] {
            new Vector2((int)brickIndexes.x, (int)brickIndexes.y - 1),
            new Vector2((int)brickIndexes.x + 1, (int)brickIndexes.y),
            new Vector2((int)brickIndexes.x, (int)brickIndexes.y + 1),
            new Vector2((int)brickIndexes.x - 1, (int)brickIndexes.y) }; //top, right, bottom, left

        BricksAbstract[] bricks = new BricksAbstract[indexes.Length];
        for (int i = 0; i < bricks.Length; i++)
        {
            bricks[i] = BricksManager.SP.GetBrickAt(indexes[i]);
        }
        neighbours = bricks;
    }

    public virtual void HitBrick(BallPower powerUp)
    {
        toDestroy.Clear();

        switch (powerUp)
        {
            case BallPower.Bomb:
                //Destroy all surounding neighbours
                Explode(2);
                break;
            case BallPower.NoCollision:
                break;
            case BallPower.None:
                break;
            default:
                break;
        }

        health -= 1;
        if (health <= 0)
        {
            PowerUpController.SP.SpawnRandomPowerUp(transform.position);
            MatchManager.SP.AddBrickHit(brickColor);
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

    public void Explode(int explosionSize = 1) //1 is the basic size
    {
        
        GetNeighbours();
        Debug.Log("Explode " + explosionSize);
        explosionSize--;
        //Find top
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours != null)
            {
                toDestroy.Add(neighbours[i]);
                /*if (explosionSize > 0)
                {
                    neighbours[i].Explode(explosionSize);
                }*/
            }
        }
    }

    public virtual void SetSprite(Sprite _sprite, BrickColor _color)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        brickColor = _color;
        spriteRenderer.sprite = _sprite;
    }

    public void SetPosToSide(float _side)
    {
        transform.localPosition = new Vector3(transform.localPosition.x + _side, transform.localPosition.y, transform.localPosition.z);
    }

    public void SetPosToTop(float _top)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + _top, transform.localPosition.z);
    }

    public virtual void SetGoalPosition(int _x, int _y, bool randomDelay = false)
    {
        if (randomDelay)
        {
            delayTimer = Random.Range(0.5f, 1.2f);
        }
        else
        {
            delayTimer = -1;
        }

        brickIndexes.x = _x;
        brickIndexes.y = _y;
        //Set goal pos
        float xCalculated = GameConstants.XBEGIN, yCalculated = GameConstants.YBEGIN;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
        GetNeighbours();
    }

    public virtual void SetPosition(int _x, int _y)
    {
        brickIndexes.x = _x;
        brickIndexes.y = _y;
        float xCalculated = GameConstants.XBEGIN, yCalculated = GameConstants.YBEGIN;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
        transform.localPosition = goalPosition;

        GetNeighbours();
    }

    protected void DestroyBrick()
    {
        BricksManager.SP.AddToDestroy(toDestroy.ToArray());
        BricksManager.SP.RemoveBrick(this);

        //Create a powerup if needed

    }

    public int GetScore => brickScore;

    public Vector2 GetIndex => brickIndexes;

    public BrickColor GetColor => brickColor;
}
