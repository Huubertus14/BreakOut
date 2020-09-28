using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BricksAbstract : MonoBehaviour
{
    [Header("BrickValues")]
    [SerializeField] protected int health;
    [SerializeField] protected float moveSpeed = 5f;


    private SpriteRenderer spriteRenderer;


    protected Vector3 goalPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        goalPosition = transform.localPosition;
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
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, goalPosition, Time.deltaTime * moveSpeed);
    }

    public virtual void SetColor(Color _col)
    {
        spriteRenderer.color = _col;
    }


    public virtual void SetGoalPosition(int _x, int _y)
    {
        
        //_x = Mathf.Clamp(_x, 0, GameConstants.ROWSCOUNT);
        //_y = Mathf.Clamp(_x, 0, GameConstants.COLUMNCOUNT);
        //Set goal pos
        float xCalculated = -2f, yCalculated = 4f;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
        Debug.Log("SetGoal " + _x + " / " + _y);
    }

    public virtual void SetPosition(int _x, int _y)
    {
        float xCalculated = -2f, yCalculated = 4f;

        xCalculated = xCalculated + (_x * GameConstants.XSTEP);
        yCalculated = yCalculated - (_y * GameConstants.YSTEP);

        goalPosition = new Vector3(xCalculated, yCalculated, transform.localPosition.z);
        transform.localPosition = goalPosition;
    }

    protected void DestroyBrick()
    {
        gameObject.SetActive(false);
    }
}
