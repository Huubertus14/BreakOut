using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawner : MonoBehaviour
{
    private float hearthBeat;
    [SerializeField] private float hearthTimer;

    private void Start()
    {
        hearthBeat = 5;
        hearthTimer = hearthBeat;
    }

    private void Update()
    {
        if (MatchManager.SP.GameStarted)
        {
            hearthTimer -= Time.deltaTime;
            if (hearthTimer < 0)
            {
                hearthTimer = hearthBeat;

                //Do things
                SpawnBricks();
            }
        }
    }

    private void SpawnBricks()
    {
        BricksManager.SP.AddRowOnTop();
    }
}
