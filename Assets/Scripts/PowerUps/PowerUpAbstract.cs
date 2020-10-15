using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Abstract class for a powerup. 
/// </summary>
public abstract class PowerUpAbstract : MonoBehaviour
{
    [Header("PowerUp Values:")]
    [SerializeField] private float fallSpeed;
    [SerializeField] private Sprite powerUpSprite;
    private SpriteRenderer ren;

    private void Awake()
    {
        ren = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ren.sprite = powerUpSprite;
    }

    public void SpawnPowerUp(Vector3 spawnPos)
    {
        transform.position = spawnPos;
    }

    public void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        Vector3 movePos = new Vector3(0, -fallSpeed * Time.deltaTime, 0) ;
        transform.position += movePos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If it hits the paddle or the deadthzone
        DeathZoneBehaviour dz = collision.gameObject.GetComponent<DeathZoneBehaviour>();
        if (dz != null)
        {
            //Remove here, aka place in the new queueue
            gameObject.SetActive(false);

            return;
        }

        PlayerBehaviour pb = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (pb != null)
        {
            //Apply powerup
            pb.PowerUp(this);
            gameObject.SetActive(false);
            return;
        }
    }

    public abstract void ApplyPowerUp();
}
