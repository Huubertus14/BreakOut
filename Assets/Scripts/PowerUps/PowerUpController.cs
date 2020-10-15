#pragma warning disable 0649

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : SingetonMonobehaviour<PowerUpController>
{
    [Header("PowerUp Prefabs:")]
    [SerializeField] private PowerUpAbstract[] powerUpPrefabs;

    protected override void Awake()
    {
        base.Awake();

    }

    public PowerUpAbstract SpawnRandomPowerUp(Vector3 brickPos)
    {
        int r = Random.Range(0, powerUpPrefabs.Length - 1);
        PowerUpAbstract pu = Instantiate(powerUpPrefabs[r], transform.position, Quaternion.identity, transform);
        pu.SpawnPowerUp(brickPos);

        return pu;
    }

}
